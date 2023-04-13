using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RythmGame
{
    public class BellManager : Singleton<BellManager>
    {
        [Header ("Bell Position variables")]
        [SerializeField] GameObject _trackedCenterTransform;
        [SerializeField] GameObject _bellCenterObject;
        [SerializeField] float _bellDistance;
        [SerializeField] int _noteArraySize;
        [SerializeField] float _xOffset;
        [SerializeField] float _yOffset;
        [SerializeField] float _zOffset;
        [SerializeField] float _yRotOffset;

        private int[] _rightNotes;
        [HideInInspector] public string _rightStoredAbility;
        [HideInInspector] public string _rightStoredDecortator;

        private int[] _leftNotes;
        [HideInInspector] public string _leftStoredAbility;
        [HideInInspector] public string _leftStoredDecorator;

        private void Start()
        {
            _leftNotes = new int[_noteArraySize];
            _rightNotes = new int[_noteArraySize];
            //Coroutine keepBellsCentered = StartCoroutine(KeepBellsCentered());
        }
        private void Update()
        {
            //Debug.Log($"avatar pos is {_trackedCenterTransform.transform.position} in update");
            KeepBellsCenteredUpdate();
        }

        /// <summary> Keeps Bells centered on the _trackedCenterTransform position </summary>
        private void KeepBellsCenteredUpdate()
        {
            _bellCenterObject.transform.position = _trackedCenterTransform.transform.position + _yOffset * _trackedCenterTransform.transform.up + _xOffset * _trackedCenterTransform.transform.right + _zOffset * _trackedCenterTransform.transform.forward;
            _bellCenterObject.transform.eulerAngles = new Vector3(_bellCenterObject.transform.eulerAngles.x, _yRotOffset + _trackedCenterTransform.transform.eulerAngles.y, _bellCenterObject.transform.eulerAngles.z);
        }
        /// <summary> Updates the right note array with a new note </summary>
        public void UpdateRightNoteArray(int newNote)
        {
            UpdateNoteArray(_rightNotes, newNote);
        }
        /// <summary> Updates the left note array with a new note </summary>
        public void UpdateLeftNoteArray(int newNote)
        {
            UpdateNoteArray(_leftNotes, newNote);
        }
        /// <summary> Updates the note array with a new note </summary>
        private void UpdateNoteArray(int[] noteArray, int newNote)
        {
            for (int index = noteArray.Length - 1; index > 0; index--)
            {
                noteArray[index] = noteArray[index - 1];
            }
            noteArray[0] = newNote;
        }

        private void CheckForSkills(int[] noteArray)
        {

        }
    }
}
