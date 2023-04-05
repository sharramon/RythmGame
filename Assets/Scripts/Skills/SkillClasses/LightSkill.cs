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

        protected override void MultiplyDecorator()
        {
            Debug.Log("Made more multiple");
        }

        public async override Task Cast(string[] activeDecorators)
        {
            await base.Cast(activeDecorators);
            Debug.Log("Light skill cast");
        }
        //protected override void AddDecorators(string[] activeDecorators)
        //{
        //    base.AddDecorators(activeDecorators);
        //}
    }
}
