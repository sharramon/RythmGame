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
    public class LightSkill : ISkill
    {
        public override string _name { get { return "Light"; } }

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
                Instantiate(_prefab, SkillManager.Instance._leftWandTip); 
            }
            else
            {
                Instantiate(_prefab, SkillManager.Instance._rightWandTip);
            }
        }

    }
}
