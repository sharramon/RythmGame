using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RythmGame
{
    public class LightSkill : ISkill
{
        public override string _name { get { return "Light"; } }

        protected override void PowerUpDecorator()
        {
            //some power up script
        }

        public override void Cast(string[] activeDecorators)
        {

        }
    }
}
