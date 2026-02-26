using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopMusicPlayer : MonoBehaviour
{
    private static LoopMusicPlayer _instance;
    public static LoopMusicPlayer Instance => _instance;
    
    [SerializeField]private AudioSource audioSource;

    private float startTime;
    
    private float endTime;

    private float timer;
    
    private const float MusicVolume = 0.4f;
    
    public float VolumeChangeTime;
    
    private void Awake()
    {
        _instance = this;
        audioSource = GetComponent<AudioSource>();
    }


    public void PlayLoop(AudioClip clip, float start, float end)
    {
        startTime = start;
        endTime = end;
        
        audioSource.Stop();
        audioSource.clip = clip;
        timer = startTime-0.3f;
        audioSource.Play();
        audioSource.time = startTime;
    }


    private void Update()
    {
        if (timer < endTime)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = startTime;
            audioSource.time = startTime;
        }
        
        
        if (timer < startTime)
        {
            audioSource.volume = 0;
        }
        else if (timer < startTime + VolumeChangeTime)
        {
            audioSource.volume = (timer-startTime)/VolumeChangeTime*MusicVolume;
        }
        else if (timer > endTime - VolumeChangeTime)
        {
            audioSource.volume = (endTime-timer)/VolumeChangeTime*MusicVolume;
        }
    }
}
