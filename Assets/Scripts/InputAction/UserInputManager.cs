using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

namespace RythmGame
{
    public class UserInputManager : Singleton<UserInputManager>
    {
        ControlHandedness _controlHandedness = ControlHandedness.right;
        [SerializeField] private ManualActionController _rightActionController;
        [SerializeField] private ManualActionController _leftActionController;
        [SerializeField] private Transform _playerTransform;

        [SerializeField] float _moveSpeed;

        private GameObject _moveController;
        private GameObject _turnController;

        private void Start()
        {
            _rightActionController.primaryButton.action.performed += OnRightPrimaryPressed;
            _leftActionController.primaryButton.action.performed += OnLeftPrimaryPressed;
            SetMovementControls();
        }
        public void SetHandedness(ControlHandedness handedness)
        {
            _controlHandedness = handedness;
        }

        public void SetMovementControls()
        {
            switch (_controlHandedness)
            {
                case ControlHandedness.right:
                    _rightActionController.thumbStick.action.performed += OnMovementTriggered;
                    _leftActionController.thumbStick.action.performed += OnTurnTriggered;
                    _moveController = _rightActionController.gameObject;
                    _turnController = _leftActionController.gameObject;
                    break;
                case ControlHandedness.left:
                    _leftActionController.thumbStick.action.performed += OnMovementTriggered;
                    _rightActionController.thumbStick.action.performed += OnTurnTriggered;
                    _moveController = _leftActionController.gameObject;
                    _turnController = _rightActionController.gameObject;
                    break;
                default:
                    break;
            }
                
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
        private void OnMovementTriggered(InputAction.CallbackContext context)
        {
            Vector2 thumbstickInput = context.ReadValue<Vector2>();
            Transform controllerTransform = _moveController.transform;
            Vector3 movementDirection = new Vector3(controllerTransform.forward.x * thumbstickInput.x, 0f, controllerTransform.forward.z * thumbstickInput.y).normalized;
            if (_moveSpeed <= 0)
                _moveSpeed = 5f;
            Vector3 movementVelocity = movementDirection * _moveSpeed;
            _playerTransform.position += movementVelocity * Time.deltaTime;
        }
        private void OnTurnTriggered(InputAction.CallbackContext context)
        {

        }

    }

    public enum ControlHandedness
    {
        right = 0,
        left = 1 
    }
}
