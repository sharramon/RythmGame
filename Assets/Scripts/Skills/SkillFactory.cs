using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace RythmGame
{
    public static class SkillFactory
    {

        private static Dictionary<string, ISkill> _abilitiesByName;
        private static bool _isInitialized => _abilitiesByName != null;


        //Constructor
        static SkillFactory()
        {
            InitializeFactory();
        }

        private static void InitializeFactory()
        {
            if (_isInitialized)
                return;

            Debug.Log("Factory being initialized");

            //gets all the non-abstract versions of ISkill
            var abilityTypes = Assembly.GetAssembly(typeof(ISkill)).GetTypes().
                Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(ISkill)));

            _abilitiesByName = new Dictionary<string, ISkill>();

            //creates the dictionary of initialized ISkills
            foreach(var type in abilityTypes)
            {
                GameObject managerObject = SkillManager.Instance.transform.gameObject;
                var tempSkill = managerObject.AddComponent(type) as ISkill;
                _abilitiesByName.Add(tempSkill._name, tempSkill);
                Debug.Log(type.FullName);
            }
        }

        public static ISkill GetSkill(string skillName)
        {
            if(_abilitiesByName.ContainsKey(skillName))
            {
                ISkill skill = _abilitiesByName[skillName];
                return skill;
            }

            return null;
        }
        /// <summary> Casts the skill <paramref name="skillName"/> </summary>
        /// <param name="skillName"></param>
        /// <param name="decorators"></param>
        public static void CastSkill(string skillName, string side, string[] decorators = null)
        {
            ISkill selectedSkill = GetSkill(skillName);

            if (selectedSkill != null)
                selectedSkill.Cast(side, decorators);
            else
                Debug.LogError($"Skill with name {skillName} not found");
        }
        /// <summary> Gets the skill names from the factory after initializing </summary>
        internal static string[] GetSkillNames()
        {
            Debug.Log("Get skill names started");
            string[] keyArray = _abilitiesByName.Keys.ToArray();
            return keyArray;
        }
    }
}
