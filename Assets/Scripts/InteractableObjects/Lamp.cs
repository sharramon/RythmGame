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

    [Header ("For testing")]
    [SerializeField] private bool m_isTurnOn = false;
    [SerializeField] private bool m_isTurnOff = false;

    public UnityEvent _lampIsLit;
    public UnityEvent _lampIsOff;

    private Material m_material;
    private bool m_isOn = false;
    [SerializeField] private bool m_isChosenOn = false; //is chosen to be on
    [SerializeField] private bool m_isChosenOff = false; //is chosen to be off
    private bool m_isFade = false;
    private bool m_isBrighten = false;

    private void Start()
    {
        Renderer renderer = m_lightOrb.GetComponent<Renderer>();
        m_material = renderer.material;
    }

    private void Update()
    {
        if(m_isTurnOn)
        {
            m_isTurnOn = false;
            TurnOn();
        }
        if(m_isTurnOff)
        {
            m_isTurnOff = false;
            TurnOff();
        }

    }

    private IEnumerator OnFadeIn(float duration, float easingFactor, float completionRate)
    {
        float elapsedTime = completionRate * duration;
        Color cachedColor = m_material.color;

        while (elapsedTime < duration)
        {
            if(m_isChosenOn)
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
                StartCoroutine(OnFadeOut(m_windDownTime, m_easingFactor, 1f - (elapsedTime / duration)));
                break;
            }
        }

        if (elapsedTime >= duration)
        {
            TurnOn();
        }
    }

    private IEnumerator OnFadeOut(float duration, float easingFactor, float completionRate)
    {
        float elapsedTime = completionRate * duration;
        float alpha;
        Color cachedColor = m_material.color;
        while (elapsedTime < duration)
        {
            if (!m_isChosenOn)
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
                StartCoroutine(OnFadeIn(m_startupTime, m_easingFactor, 1f - (elapsedTime / duration)));
                break;
            }

            m_isFade = false;
            yield return null;
        }
    }

    private IEnumerator OffFadeOut(float duration, float easingFactor, float completionRate)
    {
        Debug.Log("Entered off fade out");
        while(m_isChosenOn && m_isOn == false) //turning on takes precedence
        {
            yield return null;
        }

        float elapsedTime = completionRate * duration;
        float alpha;
        Color cachedColor = m_material.color;

        while (elapsedTime < duration)
        {
            if (m_isChosenOff && !m_isChosenOn)
            {
                alpha = Utilities.EaseInOut(1f - (elapsedTime / duration), easingFactor);
                alpha = Mathf.Clamp01(alpha);
                cachedColor.a = alpha;
                m_material.color = cachedColor;
                elapsedTime += Time.deltaTime;
            }
            else
            {
                m_isBrighten = true;
                StartCoroutine(OffFadeIn(m_startupTime, m_easingFactor, 1f - (elapsedTime / duration)));
                break;
            }

            m_isBrighten = false;

            if (elapsedTime >= duration)
            {
                TurnOff();
            }
            yield return null;
        }
    }

    private IEnumerator OffFadeIn(float duration, float easingFactor, float completionRate)
    {
        Debug.Log("Entered off fade in");
        float elapsedTime = completionRate * duration;
        Color cachedColor = m_material.color;

        while (elapsedTime < duration)
        {
            if (!m_isChosenOff)
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
                m_isBrighten = false;
                StartCoroutine(OffFadeOut(m_windDownTime, m_easingFactor, 1f - (elapsedTime / duration)));
                break;
            }
        }

        if (m_isChosenOff && !m_isChosenOn)  
        {
            StartCoroutine(OffFadeOut(m_windDownTime, m_easingFactor, 0f));
        }
    }

    public ParticleSystem GetLampEmbers()
    {
        return m_particles.GetComponent<ParticleSystem>();
    }

    private void TurnOn()
    {
        m_isOn = true;
        m_particles.SetActive(true);
        SetAlpha(1f);
        _lampIsLit.Invoke();
    }

    private void TurnOff()
    {
        m_isOn = false;
        m_particles.SetActive(false);
        SetAlpha(0f);
        _lampIsOff.Invoke();
    }

    private void SetAlpha(float value)
    {
        Color cachedColor = m_material.color;
        cachedColor.a = value;
        m_material.color = cachedColor;
    }

    public void SelectLampOn()
    {
        m_isChosenOn = true;

        if (!m_isOn)
        {
            if(!m_isFade)
                StartCoroutine(OnFadeIn(m_startupTime, m_easingFactor, 0));
        }
    }

    public void SelectLampOff()
    {
        m_isChosenOff = true;

        if(m_isOn && !m_isBrighten)
        {
            StartCoroutine(OffFadeOut(m_startupTime, m_easingFactor, 0));
        }
    }

    public void DeselectLampOn()
    {
        m_isChosenOn = false;
    }

    public void DeselectLampOff()
    {
        Debug.Log("Deselect Lamp off entered");
        m_isChosenOff = false;
    }

    public bool GetOnState()
    {
        return m_isOn;
    }
}
