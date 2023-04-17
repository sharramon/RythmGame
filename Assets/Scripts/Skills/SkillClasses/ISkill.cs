using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace RythmGame
{
    public abstract class ISkill
    {
        protected static List<string> _decoratorList = new List<string>();
        protected Dictionary<string, MethodInfo> _methodDictionary = new Dictionary<string, MethodInfo>();
        public abstract string _name { get; } //name of skill is abstract to make sure inheriting class implements
        protected bool _isInitialized = false;
        protected SkillInfo _skillInfo;

        #region initialization
        //need to replace this with an initializer somehow
        protected ISkill()
        {
            //InitializeISkill();
        }
        public async Task InitializeISkill()
        {
            Debug.Log("Initialization started");

            await InitializeDecoratorList();

            MethodInfo[] methods = this.GetType().GetMethods
                (BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(m => m.Name.Contains("Decorator")).ToArray();
            _methodDictionary = CreateMethodDictionary(methods, _decoratorList);

            _isInitialized = true;
        }

        /// <summary> Gets the string list of decorators defined in the scriptable object. /// </summary>
        private async Task InitializeDecoratorList()
        {
            List<string> decoratorList = new List<string>();
            AsyncOperationHandle<SkillScriptable> loadHandle = Addressables.LoadAssetAsync<SkillScriptable>("Scriptables/SkillList");
            await loadHandle.Task;

            if (loadHandle.IsDone && loadHandle.Status == AsyncOperationStatus.Succeeded)
            {
                Debug.Log($"addressable {loadHandle.Result.name} loaded successfully");
                SkillScriptable skillScriptable = loadHandle.Result;
                _decoratorList = skillScriptable.GetDecoratorNameList();
                foreach(string decorator in _decoratorList)
                {
                    Debug.Log($"Decorator is {decorator}");
                }
            }
            else
            {
                // Asset failed to load
                Debug.LogError($"Failed to load asset: {loadHandle.OperationException.Message}");
            }
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
        #endregion


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

        #region Cast methods
        public async virtual Task Cast(string[] activeDecorators = null)
        {
            Debug.Log($"Initialization state of skill is : {_isInitialized}");
            if (!_isInitialized)
                await InitializeISkill();

            Debug.Log($"active decorator state is {activeDecorators == null} for null, with length {activeDecorators.Length}");
            if (activeDecorators != null && activeDecorators.Length > 0)
                AddDecorators(activeDecorators);
        }

        protected virtual void AddDecorators(string[] activeDecorators)
        {
            for (int i = 0; i < activeDecorators.Length; i++)
            {
                if(activeDecorators[i] == null || !_methodDictionary.ContainsKey(activeDecorators[i]))
                {
                    Debug.Log($"The method dictionary does not contain {activeDecorators[i]}");
                    continue;
                }
                else
                {
                    Debug.Log($"The method dictionary does contain {activeDecorators[i]}");
                    MethodInfo method = _methodDictionary[activeDecorators[i]];
                    object[] arguments = new object[] { };
                    method.Invoke(this, arguments);
                }
            }
        }
        #endregion
    }
}
