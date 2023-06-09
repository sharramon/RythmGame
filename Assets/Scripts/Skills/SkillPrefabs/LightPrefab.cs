using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace RythmGame
{
    public class LightPrefab : MonoBehaviour
    {
        [Header("Light fade in/out")]
        [SerializeField] private GameObject _lightOrb; 
        [SerializeField] private float _startupTime;
        [SerializeField] private float _windDownTime;
        [SerializeField] private float _skillDuration;
        [SerializeField] private float _easingFactor;

        [Header("Lighting Objects")]
        [SerializeField] private float _detectRadius;
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private ParticleSystem _embers;
        private GameObject _currentlySelectedLamp;
        private HashSet<Flower> m_selectedFlowers = new HashSet<Flower>();
        private HashSet<Flower> m_foundFlowers = new HashSet<Flower>();
        private List<Flower> m_flowersToRemove = new List<Flower>();

        private ParticleSystem.Particle[] particles;
        private int numParticlesAlive;

        private Transform _parentTransform;
        private float _remainingDuration;
        private string _name;
        private string _side;

        Material _material;

        private void Start()
        {
            Renderer renderer = _lightOrb.GetComponent<Renderer>();
            _material = renderer.material;
            particles = new ParticleSystem.Particle[_embers.main.maxParticles];
            StartCoroutine(FadeIn(_startupTime, _easingFactor));
        }

        private void Update()
        {
            KeepOnParent();
            GetLightInteractables();
        }

        public void SetPrefab(string name, string side, Transform parentTransform)
        {
            _name = name;
            _side = side;
            _parentTransform = parentTransform;
        }

        private IEnumerator FadeIn(float duration, float easingFactor)
        {
            float elapsedTime = 0f;
            Color cachedColor = _material.color;
            while (elapsedTime < duration)
            {
                float alpha = Utilities.EaseInOut(elapsedTime / duration, easingFactor);
                alpha = Mathf.Clamp01(alpha);
                cachedColor.a = alpha;
                _material.color = cachedColor;
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            StartCoroutine(KeepAlpha(_skillDuration));
        }

        private IEnumerator KeepAlpha(float duration)
        {
            _remainingDuration = duration;
            Color cachedColor = _material.color;
            float alpha = 1f;
            cachedColor.a = alpha;
            _material.color = cachedColor;
            while (_remainingDuration > 0f)
            {
                _remainingDuration -= Time.deltaTime;
                yield return null;
            }

            StartCoroutine(FadeOut(_windDownTime, _easingFactor));
        }

        private IEnumerator FadeOut(float duration, float easingFactor)
        {
            float elapsedTime = 0f;
            float alpha;
            Color cachedColor = _material.color;
            while (elapsedTime < duration)
            {
                if(_remainingDuration <= 0)
                {
                    alpha = Utilities.EaseInOut(1f - (elapsedTime / duration), easingFactor);
                }
                else
                {
                    alpha = Utilities.EaseInOut(elapsedTime / duration, easingFactor);
                }
                alpha = Mathf.Clamp01(alpha);
                cachedColor.a = alpha;
                _material.color = cachedColor;
                elapsedTime += Time.deltaTime;

                yield return null;
            }

            if(_remainingDuration > 0)
            {
                StartCoroutine(KeepAlpha(_skillDuration));
            }
            else
            {
                DestroyPrefab();
            }
        }

        public void ResetDuration()
        {
            _remainingDuration = _skillDuration;
        }

        private void DestroyPrefab()
        {
            _material.color = new Color(_material.color.r, _material.color.g, _material.color.b, 0f);
            if(_side == "right")
            {
                if (SkillManager.Instance._activeSkillsOnRight.ContainsKey(_name))
                    SkillManager.Instance._activeSkillsOnRight.Remove(_name);
            }
            else
            {
                if (SkillManager.Instance._activeSkillsOnLeft.ContainsKey(_name))
                    SkillManager.Instance._activeSkillsOnLeft.Remove(_name);
            }

            Destroy(this.gameObject);
        }

        private void OnDestroy()
        {
            if (_currentlySelectedLamp != null)
            {
                _currentlySelectedLamp.GetComponent<Lamp>().DeselectLampOn(_side);
            }

            foreach (Flower flower in m_selectedFlowers)
            {
                flower.CloseFlower();
            }
        }

        public void SetParentTransform()
        {

        }

        private void KeepOnParent()
        {
            if (_parentTransform == null)
                return;

            transform.position = _parentTransform.position;
        }

        private void GetLightInteractables()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, _detectRadius, _layerMask);

            ShowClosestLamp(colliders);
            UpdateSelectedFlowers(colliders);
        }

        private void ShowClosestLamp(Collider[] colliders)
        {
            int numColliders = colliders.Length;

            if (numColliders == 0)
            {
                if(_currentlySelectedLamp != null)
                {
                    _currentlySelectedLamp.GetComponent<Lamp>().DeselectLampOn(_side);
                    _currentlySelectedLamp = null;
                }
                return;
            }

            List<Collider> lampColliders = new List<Collider>();

            for (int i = 0; i < numColliders; i++)
            {
                if (colliders[i].tag == "Lamp")
                {
                    lampColliders.Add(colliders[i]);
                    Debug.Log($"added lamp object {colliders[i].gameObject.name}");
                }
            }

            if (lampColliders.Count == 0)
            {
                if (_currentlySelectedLamp != null)
                {
                    _currentlySelectedLamp.GetComponent<Lamp>().DeselectLampOn(_side);
                    _currentlySelectedLamp = null;
                }
                return;
            }

            numColliders = lampColliders.Count;

            numParticlesAlive = _embers.GetParticles(particles);

            Debug.Log($"number of lamp colliers is {numColliders}");
            if (numColliders > 0)
            {
                Collider closestCollider;

                if (numColliders < 1)
                    closestCollider = lampColliders[0];
                else
                    closestCollider = FindClosestLamp(lampColliders);

                if (_currentlySelectedLamp == null || _currentlySelectedLamp != closestCollider.gameObject)
                    ChangeSelectedLamp(closestCollider.gameObject);


                // Find the closest point on the collider to the particle system
                Vector3 targetTransform = transform.InverseTransformPoint(closestCollider.transform.position);

                // Move each particle towards the closest point on the collider
                for (int i = 0; i < numParticlesAlive; i++)
                {
                    particles[i].position = Vector3.MoveTowards(particles[i].position, targetTransform, 0.01f);
                }

                // Set the modified particles back to the system
                _embers.SetParticles(particles, numParticlesAlive);
            }
        }

        private Collider FindClosestLamp(List<Collider> colliders)
        {
            float closestDistance = -1f;
            Collider closestCollider = null;

            for(int i = 0; i < colliders.Count; i++)
            {
                float distance = Vector3.Distance(colliders[i].transform.position, this.transform.position);
                if(closestDistance < 0 || closestDistance > distance)
                {
                    closestCollider = colliders[i];
                    closestDistance = distance;
                }
            }

            return closestCollider;
        }

        private void ChangeSelectedLamp(GameObject closestLamp)
        {
            closestLamp.GetComponent<Lamp>().SelectLampOn(_side);
            if(_currentlySelectedLamp != null)
                _currentlySelectedLamp.GetComponent<Lamp>().DeselectLampOn(_side);

            _currentlySelectedLamp = closestLamp;
        }

        private void UpdateSelectedFlowers(Collider[] colliders)
        {
            int numColliders = colliders.Length;

            if (numColliders == 0)
            {
                foreach(Flower flower in m_selectedFlowers)
                {
                    flower.CloseFlower();
                }
                return;
            }

            m_foundFlowers.Clear();

            for (int i = 0; i < numColliders; i++)
            {
                if (colliders[i].tag == "Flower" && colliders[i].gameObject.GetComponent<Flower>())
                {
                    Debug.Log($"Got flower {colliders[i].gameObject.name}");
                    Flower currentFlower = colliders[i].gameObject.GetComponent<Flower>();
                    m_foundFlowers.Add(currentFlower);
                    currentFlower.OpenFlower();
                    if (!m_selectedFlowers.Contains(currentFlower))
                    {
                        Debug.Log($"Opening flower {currentFlower.gameObject.name}");
                        m_selectedFlowers.Add(currentFlower);
                    }
                }
            }

            m_flowersToRemove.Clear();

            if(m_foundFlowers.Count != 0)
            {
                foreach(Flower flower in m_selectedFlowers)
                {
                    if(!m_foundFlowers.Contains(flower))
                    {
                        Debug.Log($"Closing flower {flower.gameObject.name}");
                        flower.CloseFlower();
                        m_flowersToRemove.Add(flower);
                    }
                }

                foreach(Flower flower in m_flowersToRemove)
                {
                    m_selectedFlowers.Remove(flower);
                }
            }
        }

        private void OpenSelectedFlowers()
        {

        }

        private void CloseDeselectedFlowers()
        {

        }
    }
}
