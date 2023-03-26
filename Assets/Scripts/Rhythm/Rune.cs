using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rune : MonoBehaviour
{
    public int _runeID;

    public void OnTriggerEnter(Collider other)
    {
        if(RhythmKeeper.Instance.CheckIfInWindow())
        {
            Debug.Log("hit acquired, sending signal");
        }

    }
}
