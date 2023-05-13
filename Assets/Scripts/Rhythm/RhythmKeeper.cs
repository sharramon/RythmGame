using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmKeeper : Singleton<RhythmKeeper>
{
    [SerializeField] float _beatTime;
    [SerializeField] float _windowTime;
    [SerializeField] AudioClip _beatAudioClipOne;
    [SerializeField] AudioClip _beatAudioClipTwo;

    private bool _isOneBeat = true;

    float _startTime;
    float _currentTime;
    [SerializeField] bool _isStartBeat = false;

    void Update()
    {
        if (_isStartBeat)
            KeepTime();
    }

    private void KeepTime()
    {
        _currentTime += Time.deltaTime;
        if (_currentTime >= _beatTime)
        {
            _currentTime -= _beatTime;
            PlayBeatAudio();
        }
    }
    public bool CheckIfInWindow()
    {
        if (!_isStartBeat)
            return false;

        if (_currentTime < _windowTime || _currentTime > _beatTime - _windowTime)
            return true;
        else
            return false;
    }
    private void PlayBeatAudio()
    {
        if (_isOneBeat)
        {
            _isOneBeat = false;
            SoundManager.Instance.PlayAudio(_beatAudioClipOne);
        }
        else
        {
            _isOneBeat = true;
            SoundManager.Instance.PlayAudio(_beatAudioClipTwo);
        }
    }
    public void StartBeat()
    {
        _currentTime = 0;
        _isStartBeat = true;
    }
    public void StopBeat()
    {
        _currentTime = 0;
        _isStartBeat = false;
    }
}
