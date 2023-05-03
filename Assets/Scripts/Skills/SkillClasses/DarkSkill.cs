using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace RythmGame
{
    public class DarkSkill : ISkill
    {
        public override string _name { get { return "Dark"; } }

        protected override void Awake()
        {
            base.Awake();
            Type declaringType = this.GetType();
            Debug.Log($"This is class {declaringType.Name}");
            Debug.Log($"Constructor for {declaringType.Name} called");
        } 

        protected override void PowerUpDecorator()
        {
            Debug.Log("Made more powerful");
        }

        public async override Task Cast(string side, string[] activeDecorators)
        {
            await base.Cast(side, activeDecorators);
            GameObject _prefab = _skillInfo.GetGameObject();
            Debug.Log($"{_skillInfo.GetGameObject()}");
            if (side == "left")
            {
                if (!SkillManager.Instance._activeSkillsOnLeft.ContainsKey(_name))
                {
                    GameObject skillPrefab = Instantiate(_prefab);
                    skillPrefab.GetComponent<DarkPrefab>().SetPrefab(_name, side, SkillManager.Instance._leftWandTip);
                    SkillManager.Instance._activeSkillsOnLeft.Add(_name, skillPrefab);
                }
                else
                {
                    GameObject skillPrefab = SkillManager.Instance._activeSkillsOnLeft[_name];
                    skillPrefab.GetComponent<DarkPrefab>().ResetDuration();
                }
            }
            else
            {
                if (!SkillManager.Instance._activeSkillsOnRight.ContainsKey(_name))
                {
                    Debug.Log("Cast dark skill on right");
                    GameObject skillPrefab = Instantiate(_prefab);
                    skillPrefab.GetComponent<DarkPrefab>().SetPrefab(_name, side, SkillManager.Instance._rightWandTip);
                    SkillManager.Instance._activeSkillsOnRight.Add(_name, skillPrefab);
                }
                else
                {
                    GameObject skillPrefab = SkillManager.Instance._activeSkillsOnRight[_name];
                    skillPrefab.GetComponent<DarkPrefab>().ResetDuration();
                }
            }
        }
    }
}
