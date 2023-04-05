using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RythmGame
{
    [CreateAssetMenu(menuName = "SkillList")]
    public class SkillScriptable : ScriptableObject
    {
        [SerializeField] private List<SkillInfo> _skillInfoList = new List<SkillInfo>();
        [SerializeField] private List<SkillInfo> _decoratorInfoList = new List<SkillInfo>();

        /// <summary>
        /// Returns the string names of the skills
        /// </summary>
        public List<string> GetSkillNameList()
        {
            List<string> skillList = new List<string>();

            for (int i = 0; i < _skillInfoList.Count; i++)
            {
                skillList.Add(_skillInfoList[i].GetSkillName());
            }

            return skillList;
        }
        /// <summary> Retuns the SkillInfo list of skills </summary>
        public List<SkillInfo> GetSkillList()
        {
            return _skillInfoList;
        }
        /// <summary>
        /// Returns the string names of the decorators
        /// </summary>
        public List<string> GetDecoratorNameList()
        {
            List<string> decoratorList = new List<string>();

            for (int i = 0; i < _decoratorInfoList.Count; i++)
            {
                decoratorList.Add(_decoratorInfoList[i].GetSkillName());
            }

            return decoratorList;
        }
        /// <summary> Returns the SkillInfo list for decorators </summary>
        public List<SkillInfo> GetDecoratorList()
        {
            return _decoratorInfoList;
        }
    }

    [Serializable]
    public class SkillInfo
    {
        [SerializeField] string _skillName;
        [SerializeField] GameObject _skillPrefab;
        [SerializeField] List<int> _skillInput;

        public string GetSkillName()
        {
            return _skillName;
        }

        public GameObject GetGameObject()
        {
            return _skillPrefab;
        }

        public List<int> GetSkillInput()
        {
            return _skillInput;
        }

        public string GetSkillKey()
        {
            string key ="";

            for(int i = 0; i < _skillInput.Count; i++)
            {
                key += $"{_skillInput[i]}_";
            }

            return key;
        }
    }
}

