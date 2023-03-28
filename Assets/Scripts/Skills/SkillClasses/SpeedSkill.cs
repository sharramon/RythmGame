using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RythmGame
{
    public class SpeedSkill : ISkill
    {
        public override string _name { get { return "Speed"; } }

        protected override void PowerUpDecorator()
        {
            //some power up script
        }

        public override void Cast(string[] activeDecorators)
        {

        }
    }
}
