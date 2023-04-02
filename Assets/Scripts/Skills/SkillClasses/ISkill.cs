using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace RythmGame
{
    public abstract class ISkill
    {
        protected static List<string> _decoratorList = new List<string> {
            "PowerUp",
            "Multiply"
        };
        protected Dictionary<string, MethodInfo> _methodDictionary = new Dictionary<string, MethodInfo>();
        public abstract string _name { get; } //name of skill is abstract to make sure inheriting class implements

        //need to replace this with an initializer somehow
        protected ISkill()
        {
            MethodInfo[] methods = this.GetType().GetMethods
                (BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(m => m.Name.Contains("Decorator")).ToArray();
            _methodDictionary = CreateMethodDictionary(methods, _decoratorList);
        }
        /// <summary> Gets the decorator List /// </summary>
        public static List<string> GetDecoratorList()
        {
            return _decoratorList;
        }

        protected Dictionary<string, MethodInfo> CreateMethodDictionary(MethodInfo[] methods, List<string> decoratorList)
        {
            HashSet<string> decoratorSet = new HashSet<string>(decoratorList);
            Dictionary<string, MethodInfo> methodDictionary = new Dictionary<string, MethodInfo>();

            foreach (string decoratorName in decoratorSet)
            {
                MethodInfo method = null;
                Type declaringType = this.GetType();

                // Look for an overridden method in the derived class
                while (declaringType != typeof(ISkill))
                {
                    method = declaringType.GetMethod(decoratorName + "Decorator", BindingFlags.Instance
                        | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
                    if (method != null && method.IsVirtual)
                    {
                        break;
                    }

                    declaringType = declaringType.BaseType;
                    method = null;
                }

                if (method == null)
                {
                    // Get the base class method if the derived class doesn't override it
                    method = typeof(ISkill).GetMethod(decoratorName + "Decorator", BindingFlags.NonPublic
                        | BindingFlags.Instance);
                }

                if (method != null)
                {
                    methodDictionary.Add(decoratorName, method);
                }
                else
                {
                    Debug.LogError($"Could not find decorator method '{decoratorName}' in class '{GetType().Name}'");
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
            Debug.Log("Decorator Failed");
        }
        #endregion

        public virtual void Cast(string[] activeDecorators = null)
        {
            if(activeDecorators != null && activeDecorators.Length > 0)
                AddDecorators(activeDecorators);
        }

        protected virtual void AddDecorators(string[] activeDecorators)
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
