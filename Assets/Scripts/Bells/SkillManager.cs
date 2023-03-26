using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RythmGame
{
    public class SkillManager : Singleton<SkillManager>
    {
        [SerializeField] SkillScriptable _skillList;
        [SerializeField] int _decoratorSkillNumber; //The number of decorators a skill is allowed to have

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
            List<string> scriptableDecoratorList = _skillList.GetDecoratorList();
            CheckIfDecoratorExists(decoratorList, scriptableDecoratorList);

            //initialize the decorator string arrays
            _decoratorOnRight = new string[_decoratorSkillNumber];
            _decoratorOnLeft = new string[_decoratorSkillNumber];
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
    }
}
