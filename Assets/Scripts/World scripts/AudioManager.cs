using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public List<AudioClip> songs;
    public AudioSource audioSource;
    private int currentSongIndex = 0;

    void Start()
    {
        if (songs.Count > 0 && audioSource != null)
        {
            PlaySong(currentSongIndex);
        }
        else
        {
            Debug.LogWarning("No songs found or AudioSource is not assigned!");
        }
    }

    void Update()
    {
        if (!audioSource.isPlaying)
        {
            PlayNextSong();
        }
    }

    public void PlaySong(int index)
    {
        if (index >= 0 && index < songs.Count)
        {
            audioSource.clip = songs[index];
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("Song index out of range!");
        }
    }

    public void PlayNextSong()
    {
        currentSongIndex = (currentSongIndex + 1) % songs.Count;
        PlaySong(currentSongIndex);
    }

    public void PlayPreviousSong()
    {
        currentSongIndex = (currentSongIndex - 1 + songs.Count) % songs.Count;
        PlaySong(currentSongIndex);
    }

    public void PlayRandomSong()
    {
        currentSongIndex = Random.Range(0, songs.Count);
        PlaySong(currentSongIndex);
    }

    public void NextSong()
    {
        PlayNextSong();
    }

    public void PreviousSong()
    {
        PlayPreviousSong();
    }

    public void RandomSong()
    {
        PlayRandomSong();
    }
}