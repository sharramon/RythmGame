using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using System.Threading.Tasks;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;

namespace RythmGame
{
    public class SkillManager : Singleton<SkillManager>
    {
        [SerializeField] bool _isTestingWindow = false;

        [SerializeField] SkillScriptable _skillScriptable;
        [SerializeField] int _maxRuneNumber = 10;
        [SerializeField] int _decoratorSkillNumber; //The number of decorators a skill is allowed to have


        private bool _isSkillInfoInitialized = false;
        //made a dictionary of all the skills and decorators
        Dictionary<string, SkillInfo> _skillDictionary = new Dictionary<string, SkillInfo>();
        Dictionary<string, SkillInfo> _decoratorDictionary = new Dictionary<string, SkillInfo>();

        //dictionary of note inputs to skill/decorator names
        Dictionary<string, string> _skillNoteDictionary = new Dictionary<string, string>();
        Dictionary<string, string> _decoratroNoteDictionary = new Dictionary<string, string>();
        List<int> _noteLengths; //this is a list of all possible lengths of notes for skill activation
                                //used when checking if any skills have to be updated

        //runes that are saved on each hand
        public int[] _runesOnRight;
        public int[] _runesOnLeft;

        //skills that are saved on each hand
        [HideInInspector] public string _skillOnRight;
        [HideInInspector] public string _skillOnLeft;

        //decorators that are saved on each hand
        [HideInInspector] public string[] _decoratorOnRight;
        [HideInInspector] public string[] _decoratorOnLeft;

        //Event to invoke when rune has been hit
        public UnityEvent<int[], string> _onLeftRuneUpdated;
        public UnityEvent<int[], string> _onRightRuneUpdated;

        private bool _isSkillTested = false;
        protected override void Awake()
        {
            base.Awake(); // this is for the singleton
            _onRightRuneUpdated = new UnityEvent<int[], string>();
            _onLeftRuneUpdated = new UnityEvent<int[], string>();
        }

        void Start()
        {
            _onRightRuneUpdated.AddListener(CheckForSkills);
            _onLeftRuneUpdated.AddListener(CheckForSkills);

            //create the dictionaries for the skills and decorators
            _skillDictionary = GetSkillDictionary(_skillScriptable.GetSkillList());
            _decoratorDictionary = GetSkillDictionary(_skillScriptable.GetDecoratorList());

            //initialize the rune arrays with -1
            _runesOnRight = Enumerable.Repeat(-1, _maxRuneNumber).ToArray();
            _runesOnLeft = Enumerable.Repeat(-1, _maxRuneNumber).ToArray();

            //initialize the decorator string arrays
            _decoratorOnRight = new string[_decoratorSkillNumber];
            _decoratorOnLeft = new string[_decoratorSkillNumber];
        }

        void Update()
        {
            if(!_isSkillTested)
            {
                _isSkillTested = true;
                SkillFactoryTest();
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
                //CastSkill(key);
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

        /// <summary> Updates the rune array and the invokes </summary>
        public void UpdateRunes(string side, int runeNumber)
        {
            if (_isTestingWindow && !RhythmKeeper.Instance.CheckIfInWindow())
                return;

            if(side == "right")
            {
                Array.Copy(_runesOnRight, 0, _runesOnRight, 1, _runesOnRight.Length - 1);
                _runesOnRight[0] = runeNumber;
                _onRightRuneUpdated.Invoke(_runesOnRight, side);

            }
            else if(side == "left")
            {
                Array.Copy(_runesOnLeft, 0, _runesOnLeft, 1, _runesOnLeft.Length - 1);
                _runesOnLeft[0] = runeNumber;
                _onLeftRuneUpdated.Invoke(_runesOnLeft, side);
            }
            else
            {
                Debug.LogError($"string given is {side}");
            }
        }

        /// <summary> Initializes the list of skills and decorators from the scriptable object.
        /// Scriptable object is a scriptable object just so I can update it on the fly if I want</summary>
        private async Task InitializeNotesDictionary()
        {
            AsyncOperationHandle<SkillScriptable> loadHandle = Addressables.LoadAssetAsync<SkillScriptable>("Scriptables/SkillList");
            await loadHandle.Task;

            if (loadHandle.IsDone && loadHandle.Status == AsyncOperationStatus.Succeeded)
            {
                Debug.Log($"addressable {loadHandle.Result.name} loaded successfully");
                SkillScriptable skillScriptable = loadHandle.Result;
                HashSet<int> inputLengthSet = new HashSet<int>();

                List<SkillInfo> _skillList = skillScriptable.GetSkillList();
                foreach(SkillInfo skill in _skillList)
                {
                    List<int> skillInput = skill.GetSkillInput();
                    int skillInputLength = skillInput.Count();
                    inputLengthSet.Add(skillInputLength);

                    _skillNoteDictionary.Add(string.Join(",", skillInput), skill.GetSkillName());
                    _skillDictionary.Add(skill.GetSkillName(), skill);

                    Debug.Log($"Skill Input : {string.Join(",", skillInput)}");
                }

                List<SkillInfo> _decoratorList = skillScriptable.GetDecoratorList();
                foreach(SkillInfo decorator in _decoratorList)
                {
                    List<int> decoratorInput = decorator.GetSkillInput();
                    int decoratorInputLength = decoratorInput.Count();
                    inputLengthSet.Add(decoratorInputLength);

                    _decoratroNoteDictionary.Add(string.Join(",", decoratorInput), decorator.GetSkillName());
                    _decoratorDictionary.Add(decorator.GetSkillName(), decorator);

                    Debug.Log($"Skill Input : {string.Join(",", decoratorInput)}");
                }

                _noteLengths = inputLengthSet.OrderBy(x => x).ToList(); //get all the input lengths in ascending order

            }
            else
            {
                // Asset failed to load
                Debug.LogError($"Failed to load asset: {loadHandle.OperationException.Message}");
            }

            _isSkillInfoInitialized = true;
        }
        /// <summary> Checks for skills on whatever stick it's on with <paramref name="runeArray"/> being
        /// the array from either left or right wand</summary>
        /// <param name="runeArray"></param>
        private async void CheckForSkills(int[] runeArray, string side)
        {
            Debug.Log("Check for skills started");
            //lazy instantiation
            if (!_isSkillInfoInitialized)
                await InitializeNotesDictionary();

            bool skillFound = false;
            bool decoratorFound = false;

            Debug.Log($"Length of note lengths is {_noteLengths.Count}");

            foreach(int noteLength in _noteLengths)
            {
                Debug.Log($"length : {noteLength}");
                List<int> currentNotesList = runeArray.Take(noteLength).ToList();
                string currentNotes = string.Join(",", currentNotesList);
                Debug.Log($"Checking with {currentNotes}");
                if(_skillNoteDictionary.ContainsKey(currentNotes))
                {
                    Debug.Log("Found Key");
                    if(skillFound == true)
                    {
                        Debug.LogError($"Skill has already been found once, which means there's an overlap between");
                        if (side == "right")
                            Debug.LogError($"{_skillOnRight} and {_skillNoteDictionary[currentNotes]}");
                        else
                            Debug.LogError($"{_skillOnLeft} and {_skillNoteDictionary[currentNotes]}");
                    }

                    if(side == "right")
                    {
                        _skillOnRight = _skillNoteDictionary[currentNotes];
                        Debug.Log($"Skill {_skillNoteDictionary[currentNotes]} is stored on right");
                    }
                    else if(side == "left")
                    {
                        _skillOnLeft = _skillNoteDictionary[currentNotes];
                        Debug.Log($"Skill {_skillNoteDictionary[currentNotes]} is stored on left");
                    }
                    else
                    {
                        Debug.LogError($"side is {side}");
                    }

                    skillFound = true;
                    return;
                }

                if(_decoratroNoteDictionary.ContainsKey(currentNotes))
                {
                    if (decoratorFound == true)
                    {
                        Debug.LogError($"Decorator has already been found once, which means there's an overlap between");
                        if (side == "right")
                            Debug.LogError($"{_decoratorOnRight[0]} and {_skillNoteDictionary[currentNotes]}");
                        else
                            Debug.LogError($"{_decoratorOnRight[0]} and {_skillNoteDictionary[currentNotes]}");
                    }

                    if (side == "right")
                    {
                        if(_decoratorOnRight.Length > 1)
                            Array.Copy(_decoratorOnRight, 0, _decoratorOnRight, 1, _decoratorOnRight.Length - 1);

                        _decoratorOnRight[0] = _decoratroNoteDictionary[currentNotes];

                        Debug.Log($"Decorator {_decoratroNoteDictionary[currentNotes]} is stored on right");
                    }
                    else if (side == "left")
                    {
                        if (_decoratorOnLeft.Length > 1)
                            Array.Copy(_decoratorOnLeft, 0, _decoratorOnLeft, 1, _decoratorOnLeft.Length - 1);

                        _decoratorOnLeft[0] = _decoratroNoteDictionary[currentNotes];

                        Debug.Log($"Decorator {_decoratroNoteDictionary[currentNotes]} is stored on left");
                    }
                    else
                    {
                        Debug.LogError($"side is {side}");
                    }

                    decoratorFound = true;
                    return;
                }

            }
        }

        private IEnumerator WaitForSecs(float seconds)
        {
            yield return new WaitForSeconds(seconds);
        }


    }
}
