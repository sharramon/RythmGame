using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Lamp : MonoBehaviour
{
    [SerializeField] private GameObject m_lightOrb;
    [SerializeField] private float m_startupTime;
    [SerializeField] private float m_windDownTime;
    [SerializeField] private float m_easingFactor;
    [SerializeField] private GameObject m_particles;

    public UnityEvent _lampIsLit;

    private Material m_material;
    private bool m_isOn = false;
    private bool m_isChosen = false;
    private bool m_isFade = false;

    private void Start()
    {

        Renderer renderer = m_lightOrb.GetComponent<Renderer>();
        m_material = renderer.material;
    }

    private IEnumerator FadeIn(float duration, float easingFactor, float completionRate)
    {
        float elapsedTime = completionRate * duration;
        Color cachedColor = m_material.color;

        while (elapsedTime < duration)
        {
            if(m_isChosen)
            {
                float alpha = Utilities.EaseInOut(elapsedTime / duration, easingFactor);
                alpha = Mathf.Clamp01(alpha);
                cachedColor.a = alpha;
                m_material.color = cachedColor;
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            else
            {
                m_isFade = true;
                StartCoroutine(FadeOut(m_windDownTime, m_easingFactor, 1f - (elapsedTime / duration)));
                break;
            }
        }

        if (elapsedTime >= duration)
        {
            TurnOn();
        }
    }

    private IEnumerator FadeOut(float duration, float easingFactor, float completionRate)
    {
        float elapsedTime = completionRate * duration;
        float alpha;
        Color cachedColor = m_material.color;
        while (elapsedTime < duration)
        {
            if (!m_isChosen)
            {
                alpha = Utilities.EaseInOut(1f - (elapsedTime / duration), easingFactor);
                alpha = Mathf.Clamp01(alpha);
                cachedColor.a = alpha;
                m_material.color = cachedColor;
                elapsedTime += Time.deltaTime;
            }
            else
            {
                m_isFade = false;
                StartCoroutine(FadeIn(m_startupTime, m_easingFactor, 1f - (elapsedTime / duration)));
                break;
            }

            m_isFade = false;
            yield return null;
        }
    }

    private void TurnOn()
    {
        m_isOn = true;
        m_particles.SetActive(true);
        _lampIsLit.Invoke();
    }

    public void SelectLamp()
    {
        m_isChosen = true;

        if (!m_isOn)
        {
            if(!m_isFade)
                StartCoroutine(FadeIn(m_startupTime, m_easingFactor, 0));
        }
    }

    public void DeselectLamp()
    {
        m_isChosen = false;
    }
}
