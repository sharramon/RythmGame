using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

namespace RythmGame
{
    public class UserInpuManager : Singleton<UserInpuManager>
    {
        [SerializeField] private ManualActionController _rightActionController;
        [SerializeField] private ManualActionController _leftActionController;

        private void Start()
        {
            _rightActionController.primaryButton.action.performed += OnRightPrimaryPressed;
            _leftActionController.primaryButton.action.performed += OnLeftPrimaryPressed;
        }

        private void OnRightPrimaryPressed(InputAction.CallbackContext context)
        {
            Debug.Log("Right Primary pressed");
            SkillManager.Instance.CastSkillOnRight();
        }

        private void OnLeftPrimaryPressed(InputAction.CallbackContext context)
        {
            Debug.Log("Left Primary pressed");
            SkillManager.Instance.CastSkillOnLeft();
        }
    }
}
