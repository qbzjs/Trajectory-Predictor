using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource startTrials;
    
    void Awake(){
        instance = this;
    }
    private void Start(){
        
    }

    public void PlayInteraction(AudioSource source){
        AudioSource s = source;
        s.Play();
    }
    public void PlayInteraction(AudioSource source, float volume){
        AudioSource s = source;
        s.volume = volume;
        s.Play();
    }
    public void PlayInteraction(AudioSource source, float volume, float pitch){
        AudioSource s = source;
        s.volume = volume;
        s.pitch = pitch;
        s.Play();
    }
}
