using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

namespace RythmGame
{
    public class SkillManager : Singleton<SkillManager>
    {
        [SerializeField] SkillScriptable _skillScriptable;
        [SerializeField] int _maxRuneNumber;
        [SerializeField] int _decoratorSkillNumber; //The number of decorators a skill is allowed to have

        //made a dictionary of all the skills and decorators
        Dictionary<string, SkillInfo> _skillDictionary = new Dictionary<string, SkillInfo>();
        Dictionary<string, SkillInfo> _decoratorDictionary = new Dictionary<string, SkillInfo>();

        //runes that are saved on each hand
        [HideInInspector] public int[] _runesOnRight;
        [HideInInspector] public int[] _runesOnLeft;

        //skills that are saved on each hand
        [HideInInspector] public string _skillOnRight;
        [HideInInspector] public string _skillOnLeft;

        //decorators that are saved on each hand
        [HideInInspector] public string[] _decoratorOnRight;
        [HideInInspector] public string[] _decoratorOnLeft;

        //Event to invoke when rune has been hit
        public UnityEvent<int[]> _onLeftRuneUpdated;
        public UnityEvent<int[]> _onRightRuneUpdated;

        protected override void Awake()
        {
            base.Awake(); // this is for the singleton
            _onRightRuneUpdated = new UnityEvent<int[]>();
            _onLeftRuneUpdated = new UnityEvent<int[]>();
        }

        void Start()
        {
            _onRightRuneUpdated.AddListener(CheckForSkills);
            _onLeftRuneUpdated.AddListener(CheckForSkills);

            //check if decorators exist in ISkill
            List<string> decoratorList = ISkill.GetDecoratorList();
            List<string> scriptableDecoratorList = _skillScriptable.GetDecoratorNameList();
            CheckIfDecoratorExists(decoratorList, scriptableDecoratorList);

            //create the dictionaries for the skills and decorators
            _skillDictionary = GetSkillDictionary(_skillScriptable.GetSkillList());
            _decoratorDictionary = GetSkillDictionary(_skillScriptable.GetDecoratorList());

            //initialize the rune arrays with -1
            _runesOnRight = Enumerable.Repeat(-1, _maxRuneNumber).ToArray();
            _runesOnLeft = Enumerable.Repeat(-1, _maxRuneNumber).ToArray();

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

        /// <summary> this is just a test method. Comment out or erase later. </summary>
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

        public void UpdateRunes(string side, int runeNumber)
        {
            if(side == "right")
            {
                Array.Copy(_runesOnRight, 0, _runesOnRight, 1, _runesOnRight.Length - 1);
                _runesOnRight[0] = runeNumber;
                _onRightRuneUpdated.Invoke(_runesOnRight);

            }
            else if(side == "left")
            {
                Array.Copy(_runesOnLeft, 0, _runesOnLeft, 1, _runesOnLeft.Length - 1);
                _runesOnLeft[0] = runeNumber;
                _onLeftRuneUpdated.Invoke(_runesOnLeft);
            }
            else
            {
                Debug.LogError($"string given is {side}");
            }
        }

        private void CheckForSkills(int[] runeArray)
        {

        }
    }
}
