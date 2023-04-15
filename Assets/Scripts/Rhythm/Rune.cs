using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RythmGame
{
    public class Rune : MonoBehaviour
    {
        [SerializeField] private int _runeID;
        private SkillManager _skillManager;

        private void OnTriggerEnter(Collider other)
        {
            //lazily assign
            if(_skillManager == null)
            {
                //just in case I forgot to include the skillmanager in the scene
                if (SkillManager.Instance == null)
                {
                    Debug.LogError("No skill manager exists in scene");
                    return;
                }
                _skillManager = SkillManager.Instance;
            }

            //if an object tagged with Wand hits, check if the gameobject has left or right in its name then
            //send the rune ID to the correct one
            if (other.tag == "Wand" && RhythmKeeper.Instance.CheckIfInWindow())
            {
                //Debug.Log("hit acquired, sending signal");
                if(other.name.Contains("right", StringComparison.InvariantCultureIgnoreCase))
                {
                    Debug.Log($"right, {_runeID}");
                    _skillManager.UpdateRunes("right", _runeID);
                }
                else if(other.name.Contains("left", StringComparison.InvariantCultureIgnoreCase))
                {
                    Debug.Log($"left, {_runeID}");
                    _skillManager.UpdateRunes("left", _runeID);
                }
                else
                {
                    Debug.Log($"The wand object {other.name} does not include either left or right");
                }
            }

        }
    }
}
