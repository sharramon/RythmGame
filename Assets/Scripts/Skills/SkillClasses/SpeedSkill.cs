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

        public SpeedSkill() : base()
        {
            Type declaringType = this.GetType();
            Debug.Log($"This is class {declaringType.Name}");
        }

        protected override void PowerUpDecorator()
        {
            Debug.Log("Powered Up");
        }

        public async override Task Cast(string[] activeDecorators)
        {
            await base.Cast(activeDecorators);
            Debug.Log("Speed skill cast");
        }
    }
}
