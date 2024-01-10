using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    public AudioClip[] backgroundMusicClips;
    private static int _currentClipIndex = 0;
    public AudioSource _source;

    private void Start()
    {
        SetBackgroundMusic(_currentClipIndex);
    }

    private void Update()
    {
        // Check if the CTRL key is released (key up event)
        if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.RightControl))
        {
            ChangeBackgroundMusic();
        }

        // Check if the DownArrow key is released (key up event)
        if (Input.GetKeyUp(KeyCode.DownArrow) && _source.volume > 0)
        {
            _source.volume -= 0.1f;
        }

        // Check if the UpArrow key is released (key up event)
        if (Input.GetKeyUp(KeyCode.UpArrow) && _source.volume < 1)
        {
            _source.volume += 0.1f;
        }
    }

    private void ChangeBackgroundMusic()
    {
        ++_currentClipIndex;

        if (_currentClipIndex >= backgroundMusicClips.Length)
        {
            // If it exceeds, loop back to the first clip
            _currentClipIndex = 0;
        }

        SetBackgroundMusic(_currentClipIndex);
    }

    private void SetBackgroundMusic(int index)
    {
        _source.clip = backgroundMusicClips[index];
        _source.Play();
    }
}
