using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : ILightableObject
{
    public override string _name { get { return "Flower"; } }
    [SerializeField] private Animator m_flowerAnimator;
    [SerializeField] private string m_openTrigger;
    [SerializeField] private string m_closeTrigger;

    [Header("Animation state names")]
    [SerializeField] private List<string> m_states = new List<string>(); 

    List<GameObject> m_CurrentlyActiveLights = new List<GameObject>();

    [Header("For testing")]
    [SerializeField] private bool m_openFlower = false;
    [SerializeField] private bool m_closeFlower = false;

    private void Start()
    {
        m_flowerAnimator = this.GetComponent<Animator>();
    }

    private void Update()
    {
        if(m_openFlower)
        {
            m_openFlower = false;
            OpenFlower();
        }
        
        if(m_closeFlower)
        {
            m_closeFlower = false;
            CloseFlower();
        }
    }

    private void TriggerOpen() 
    {
        m_flowerAnimator.SetTrigger(m_openTrigger);
    }

    private void TriggerClose()
    {
        m_flowerAnimator.SetTrigger(m_closeTrigger);
    }

    public override void LightObject(GameObject lightObject)
    {
        if (m_CurrentlyActiveLights.Count == 0)
            OpenFlower();

        if (!m_CurrentlyActiveLights.Contains(lightObject))
            m_CurrentlyActiveLights.Add(lightObject);
    }

    public override void DimObject(GameObject lightObject)
    {
        if (m_CurrentlyActiveLights.Contains(lightObject))
            m_CurrentlyActiveLights.Remove(lightObject);

        if (m_CurrentlyActiveLights.Count == 0)
            CloseFlower();
    }

    public void OpenFlower()
    {
        if (GetCurrentAnimationName() == "Closed")
            TriggerOpen();
        if(GetCurrentAnimationName() == "Closing")
        {
            AnimatorStateInfo stateInfo = m_flowerAnimator.GetCurrentAnimatorStateInfo(0);
            float normalizedTime = stateInfo.normalizedTime;
            m_flowerAnimator.Play("Opening", 0, 1f - normalizedTime);
        }
    }

    public void CloseFlower()
    {
        if (GetCurrentAnimationName() == "Open")
            TriggerClose();
        if(GetCurrentAnimationName() == "Opening")
        {
            AnimatorStateInfo stateInfo = m_flowerAnimator.GetCurrentAnimatorStateInfo(0);
            float normalizedTime = stateInfo.normalizedTime;
            m_flowerAnimator.Play("Closing", 0, 1f - normalizedTime);
        }
    }

    private string GetCurrentAnimationName()
    {
        AnimatorStateInfo stateInfo = m_flowerAnimator.GetCurrentAnimatorStateInfo(0);

        for (int i = 0; i < m_states.Count; i++)
        {
            if (stateInfo.IsName(m_states[i]))
            {
                return m_states[i];
            }
        }
        return null;
    }
}
