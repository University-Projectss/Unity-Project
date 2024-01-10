using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    public AudioClip[] backgroundMusicClips;
    private static int currentClipIndex = 0;

    private void Start()
    {
        SetBackgroundMusic(currentClipIndex);
    }

    private void Update()
    {
        // Check if the CTRL key is released (key up event)
        if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.RightControl))
        {
            ChangeBackgroundMusic();
        }
    }

    private void ChangeBackgroundMusic()
    {
        ++currentClipIndex;

        if (currentClipIndex >= backgroundMusicClips.Length)
        {
            // If it exceeds, loop back to the first clip
            currentClipIndex = 0;
        }

        SetBackgroundMusic(currentClipIndex);
    }

    private void SetBackgroundMusic(int index)
    {
        GetComponent<AudioSource>().clip = backgroundMusicClips[index];
        GetComponent<AudioSource>().Play();
    }
}
