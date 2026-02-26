using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSE : MonoBehaviour
{
    [SerializeField]private AudioSource audioSource;
    
    private float timer = 0;
    
    private bool isStarted = false;
    
    private bool isPlayed = false;


    public void PlaySE(float volume = 1)
    {
        audioSource.volume = volume;
        audioSource.Play();
        //audioSource.Pause();
        timer = 0.5f;
        isStarted =  true;
        isPlayed = false;
    }

    private void Update()
    {
        if (isStarted)
        {
            if (timer <= 1 - 0.1f && isPlayed == false)
            {
                // audioSource.UnPause();
                // audioSource.time = 0;
                // isPlayed = true;
            }
            if (timer >= 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                isStarted = false;
                audioSource.Stop();
                HitSEPool.Instance.ReturnObject(gameObject);
            }
        }
    }

    private void Awake()
    {
        audioSource.clip.LoadAudioData();
    }
}
