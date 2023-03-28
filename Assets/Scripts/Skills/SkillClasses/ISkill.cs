using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace RythmGame
{
    public abstract class ISkill : MonoBehaviour
    {
        private static List<string> _decoratorList = new List<string> {
            "PowerUp",
            "Multiply"
        };
        protected Dictionary<string, MethodInfo> _methodDictionary = new Dictionary<string, MethodInfo>();
        public abstract string _name { get; } //name of skill is abstract to make sure inheriting class implements

        protected virtual void Start()
        {
            MethodInfo[] methods = GetType().GetMethods().Where(m => m.Name.Contains("Decorator")).ToArray();
            _methodDictionary = CreateMethodDictionary(methods, _decoratorList);
        }
        /// <summary> Gets the decorator List /// </summary>
        public static List<string> GetDecoratorList()
        {
            return _decoratorList;
        }

        private Dictionary<string, MethodInfo> CreateMethodDictionary(MethodInfo[] methods, List<string> decoratorList)
        {
            HashSet<string> decoratorSet = new HashSet<string>(decoratorList);
            Dictionary<string, MethodInfo> methodDictionary = new Dictionary<string, MethodInfo>();

            for (int listInd = 0; listInd < decoratorList.Count; listInd++)
            {
                string decoratorName = decoratorList[listInd];
                var matchingMethods = methods.Where(m => m.Name.Contains(decoratorName)).ToArray();
                if (matchingMethods.Length == 1)
                {
                    methodDictionary.Add(decoratorName, matchingMethods[0]);
                }
                else if(matchingMethods.Length > 1)
                {
                    Debug.LogError($"Too many decorator methods include {decoratorName}");
                }
                else
                {
                    Debug.LogError($"No decorator methods include {decoratorName}");
                }
            }

            return methodDictionary;
        }

        #region Decorators
        protected virtual void PowerUpDecorator() 
        {
            FailSkillDecorator();
        }

        protected virtual void MultiplyDecorator()
        {
            FailSkillDecorator();
        }

        public void FailSkillDecorator()
        {
            //some fail action/animation/effect
        }
        #endregion

        public virtual void Cast(string[] activeDecorators)
        {
            AddDecorators(activeDecorators);
        }

        protected void AddDecorators(string[] activeDecorators)
        {
            for(int i = 0; i < activeDecorators.Length; i++)
            {
                MethodInfo method = _methodDictionary[activeDecorators[i]];
                object[] arguments = new object[] { };
                method.Invoke(this, arguments);
            }
        }
    }
}
