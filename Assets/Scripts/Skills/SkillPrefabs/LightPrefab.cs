using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace RythmGame
{
    public class LightPrefab : MonoBehaviour
    {
        [Header("Light fade in/out")]
        [SerializeField] private float _startupTime;
        [SerializeField] private float _windDownTime;
        [SerializeField] private float _skillDuration;
        [SerializeField] private float _easingFactor;

        [Header("Lighting Objects")]
        [SerializeField] private float _detectRadius;
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private ParticleSystem _embers;

        private float _remainingDuration;
        private string _name;
        private string _side;

        Material _material;

        private void Start()
        {
            Renderer renderer = GetComponent<Renderer>();
            _material = renderer.material;
            StartCoroutine(FadeIn(_startupTime, _easingFactor));
        }

        private void Update()
        {
            GetClosestCollider();
        }

        public void SetPrefab(string name, string side)
        {
            _name = name;
            _side = side;
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

        private void GetClosestCollider()
        {
            Collider[] colliders = new Collider[1];
            int numColliders = Physics.OverlapSphereNonAlloc(transform.position, _detectRadius, colliders, _layerMask);

            if (numColliders > 0)
            {
                Collider closestCollider = colliders[0];
                // Do something with the closest collider...
            }
        }
    }
}
