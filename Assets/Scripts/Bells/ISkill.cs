using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RythmGame
{
    public abstract class ISkill : MonoBehaviour
    {
        private static List<string> _decoratorList = new List<string> {
            "PowerUp",
            "Multiply"
        };
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

        public abstract void Cast();
    }
}
