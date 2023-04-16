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

        public LightSkill() : base()
        {
            Type declaringType = this.GetType();
            Debug.Log($"This is class {declaringType.Name}");
            Debug.Log($"Constructor for {declaringType.Name} called");
        } 

        protected override void PowerUpDecorator()
        {
            Debug.Log("Made more powerful");
        }

        public async override Task Cast(string[] activeDecorators)
        {
            Debug.Log("Light skill entered");
            await base.Cast(activeDecorators);
            Debug.Log("Light skill cast");
        }

    }
}
