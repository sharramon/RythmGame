using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelOneEventManger : MonoBehaviour
{
    [SerializeField] private GameObject _levelOneDoor;
    
    public void DestroyDoor()
    {
        if(_levelOneDoor != null)
            Destroy(_levelOneDoor);
    }
}
