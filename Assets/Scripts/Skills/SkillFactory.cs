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
        private static Dictionary<string, Type> _abilitiesByName;
        private static bool _isInitialized => _abilitiesByName != null;

        private static void InitializeFactory()
        {
            if (_isInitialized)
                return;

            //gets all the non-abstract versions of ISkill
            var abilityTypes = Assembly.GetAssembly(typeof(ISkill)).GetTypes().
                Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(ISkill)));

            _abilitiesByName = new Dictionary<string, Type>();

            //creates the dictionary of initialized ISkills
            foreach(var type in abilityTypes)
            {
                var tempSkill = Activator.CreateInstance(type) as ISkill;
                _abilitiesByName.Add(tempSkill.name, type);
            }
        }
    }
}
