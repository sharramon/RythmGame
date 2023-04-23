using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RythmGame
{
    public class SkillTest : MonoBehaviour
    {
        public bool _runTest = false;
        public string _skill;
        public string[] _decorators = new string[0];
        public string _side;

        private void Update()
        {
            if(_runTest)
            {
                _runTest = false;
                TestSkill();
            }
        }

        private void TestSkill()
        {
            SkillManager.Instance.CastSkill(_skill, _side, _decorators);
        }
    }
}
