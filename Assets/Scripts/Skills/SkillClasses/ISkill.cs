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

        protected virtual void Awake()
        {
            MethodInfo[] methods = GetType().GetMethods().Where(m => m.Name.Contains("Decorator")).ToArray();
        }
        /// <summary> Gets the decorator List /// </summary>
        public static List<string> GetDecoratorList()
        {
            return _decoratorList;
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

        }

        private void AddDecorators()
        {

        }
    }
}
