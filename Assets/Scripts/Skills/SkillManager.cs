using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RythmGame
{
    public class SkillManager : Singleton<SkillManager>
    {
        [SerializeField] SkillScriptable _skillScriptable;
        [SerializeField] int _decoratorSkillNumber; //The number of decorators a skill is allowed to have

        //made a dictionary of all the skills and decorators
        Dictionary<string, SkillInfo> _skillDictionary = new Dictionary<string, SkillInfo>();
        Dictionary<string, SkillInfo> _decoratorDictionary = new Dictionary<string, SkillInfo>();

        //skills that are saved on each hand
        [HideInInspector] public string _skillOnRight;
        [HideInInspector] public string _skillOnLeft;

        //decorators that are saved on each hand
        [HideInInspector] public string[] _decoratorOnRight;
        [HideInInspector] public string[] _decoratorOnLeft;

        void Start()
        {
            //check if decorators exist in ISkill
            List<string> decoratorList = ISkill.GetDecoratorList();
            List<string> scriptableDecoratorList = _skillScriptable.GetDecoratorNameList();
            CheckIfDecoratorExists(decoratorList, scriptableDecoratorList);

            //create the dictionaries for the skills and decorators
            _skillDictionary = GetSkillDictionary(_skillScriptable.GetSkillList());
            _decoratorDictionary = GetSkillDictionary(_skillScriptable.GetDecoratorList());

            //initialize the decorator string arrays
            _decoratorOnRight = new string[_decoratorSkillNumber];
            _decoratorOnLeft = new string[_decoratorSkillNumber];

            //ability factory test
            SkillFactoryTest();
        }

        void Update()
        {

        }

        private void CheckIfDecoratorExists(List<string> decoratorList, List<string> scriptableDecoratorList)
        {
            for(int i = 0; i < scriptableDecoratorList.Count; i++)
            {
                if(!decoratorList.Contains(scriptableDecoratorList[i]))
                {
                    Debug.LogError($"ISkill does not contain the decorator {scriptableDecoratorList[i]}. Consider adding");
                }
            }
        }

        private Dictionary<string, SkillInfo> GetSkillDictionary(List<SkillInfo> skillInfoList)
        {
            Dictionary<string, SkillInfo> skillDictionary = new Dictionary<string, SkillInfo>();

            for (int i = 0; i < skillInfoList.Count; i++)
            {
                string key = skillInfoList[i].GetSkillKey();
                skillDictionary.Add(key, skillInfoList[i]);
            }

            return skillDictionary;
        }

        private void SkillFactoryTest()
        {
            Debug.Log("Factory test started");
            string[] keyArray = SkillFactory.GetSkillNames();
            string[] testDecorator = new string[0];

            foreach(string key in keyArray)
            {
                Debug.Log($"Skill name is {key}");
                CastSkill(key);
            }

            string rightSKill = "Light";
            string[] rightDecorator = { "Multiply" };

            string leftSkill = "Speed";
            string[] leftDecorator = { "PowerUp" };

            CastSkill(rightSKill, rightDecorator);
            CastSkill(leftSkill, leftDecorator);
        }

        #region Skill Cast Methods
        /// <summary> casts the skills depending on the info saved on the left 'wand' </summary>
        public void CastSkillOnLeft()
        {
            CastSkill(_skillOnLeft, _decoratorOnLeft);
        }
        /// <summary> casts the skills depending on the info saved on the right 'wand' </summary>
        public void CastSkillOnRight()
        {
            CastSkill(_skillOnRight, _decoratorOnRight);
        }
        /// <summary> casts skill </summary>
        public void CastSkill(string skillName, string[] decorators = null)
        {
            SkillFactory.CastSkill(skillName, decorators);
        }
        #endregion
    }
}
