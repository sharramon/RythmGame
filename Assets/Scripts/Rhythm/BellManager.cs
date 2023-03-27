using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RythmGame
{
    public class BellManager : Singleton<BellManager>
    {
        [SerializeField] GameObject _trackedCenterTransform;
        [SerializeField] GameObject _bellCenterObject;
        [SerializeField] float _bellDistance;
        [SerializeField] int _noteArraySize;

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
            Debug.Log($"avatar pos is {_trackedCenterTransform.transform.position} in update");
            KeepBellsCenteredUpdate();
        }

        /// <summary> Keeps Bells centered on the _trackedCenterTransform position </summary>
        private IEnumerator KeepBellsCentered()
        {
            while (true)
            {
                //Vector3 forwardDirection = _trackedCenterTransform.up;
                //forwardDirection = new Vector3(forwardDirection.x, 0, forwardDirection.z);
                //Vector3 bellLocalPosition = forwardDirection * _bellDistance;
                //_bellCenterObject.transform.localPosition = _trackedCenterTransform.transform.localPosition;
                Debug.Log($"avatar pos is {_trackedCenterTransform.transform.position} in coroutine");

                yield return null;

            }
        }

        /// <summary> Keeps Bells centered on the _trackedCenterTransform position </summary>
        private void KeepBellsCenteredUpdate()
        {
            Vector3 forwardDirection = _trackedCenterTransform.transform.up;
            forwardDirection = new Vector3(forwardDirection.x, 0, forwardDirection.z);
            Vector3 bellLocalPosition = forwardDirection * _bellDistance;
            _bellCenterObject.transform.position = _trackedCenterTransform.transform.position + bellLocalPosition;
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
