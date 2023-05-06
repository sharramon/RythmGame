using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FirstLevelManager : ILevelManager
{
    [SerializeField] List<Animator> _wallAnimator;

    [SerializeField] bool _testWallTrigger;
    

    private void Update()
    {
        if(_testWallTrigger)
        {
            _testWallTrigger = false;
            OnLightLit();
        }
    }

    public void OnLightLit()
    {
        for(int i = 0; i < _wallAnimator.Count; i++)
        {
            _wallAnimator[i].SetTrigger("OpenOneWalls");
        }
    }
}
