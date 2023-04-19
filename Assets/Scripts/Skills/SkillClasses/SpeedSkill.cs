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
    public class SpeedSkill : ISkill
    {
        public override string _name { get { return "Speed"; } }

        protected override void Awake()
        {
            base.Awake();
            Type declaringType = this.GetType();
            Debug.Log($"This is class {declaringType.Name}");
        }

        protected override void PowerUpDecorator()
        {
            Debug.Log("Powered Up");
        }

        public async override Task Cast(string side, string[] activeDecorators)
        {
            await base.Cast(side, activeDecorators);
            Debug.Log("Speed skill cast");
        }
    }
}
