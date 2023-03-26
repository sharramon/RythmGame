using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RythmGame
{
    [CreateAssetMenu(menuName = "SkillList")]
    public class SkillScriptable : ScriptableObject
    {
        public List<SkillInfo> _skillInfoList = new List<SkillInfo>();
        public List<SkillInfo> _decoratorInfoList = new List<SkillInfo>();

        public List<string> GetSkillList()
        {
            List<string> skillList = new List<string>();

            for (int i = 0; i < _skillInfoList.Count; i++)
            {
                skillList.Add(_skillInfoList[i].GetSkillName());
            }

            return skillList;
        }

        public List<string> GetDecoratorList()
        {
            List<string> decoratorList = new List<string>();

            for (int i = 0; i < _decoratorInfoList.Count; i++)
            {
                decoratorList.Add(_decoratorInfoList[i].GetSkillName());
            }

            return decoratorList;
        }
    }

    [Serializable]
    public class SkillInfo
    {
        [SerializeField] string _skillName;
        [SerializeField] GameObject _skillPrefab;
        [SerializeField] int[] _skillInput;

        public string GetSkillName()
        {
            return _skillName;
        }

        public GameObject GetGameObject()
        {
            return _skillPrefab;
        }

        public int[] GetSkillInput()
        {
            return _skillInput;
        }
    }
}

