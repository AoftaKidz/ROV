using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class Sound
{
    public string fileName;
    public AudioClip clip;
}
public class SoundManager : MonoBehaviour
{
    public Sound[] BGMs;
    public Sound[] SFX;
    public AudioSource audioBGM;
    public AudioSource audioSFX;
    public AudioSource audioSlot;
    public AudioSource audioSlotGirlRed;
    public AudioSource audioSlotGirlGold;

    public static SoundManager Instance = null;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }
    public void PlayBGM(string audioName)
    {
        if (!UserProfile.Instance.isOnSpeaker) return;

        Sound s = Array.Find(BGMs,x=>x.fileName == audioName);
        if (s == null) return;

        audioBGM.clip = s.clip;
        audioBGM.Play();
        audioBGM.loop = true;
    }
    public void PlaySFX(string audioName,bool loop = false)
    {
        if (!UserProfile.Instance.isOnEffect) return;

        Sound s = Array.Find(SFX, x => x.fileName == audioName);

        if (s == null) return;
        AudioSource source;

        if (audioName.Contains("GirlRed"))
            return;//source = audioSlotGirlRed;
        else if (audioName.Contains("GirlGold"))
            return;//source = audioSlotGirlGold;
        else if (audioName.Contains("Slot"))
            source = audioSlot;
        else
            source = audioSFX;

        if (loop)
        {
            source.clip = s.clip;
            source.Play();
            source.loop = loop;
        }
        else
        {
            source.PlayOneShot(s.clip);
        }
    }
    public void StopSFX(string audioName)
    {
        Sound s = Array.Find(SFX, x => x.fileName == audioName);
        if (s == null) return;
        AudioSource source;

        if (audioName.Contains("GirlRed"))
            return;//source = audioSlotGirlRed;
        else if (audioName.Contains("GirlGold"))
            return;//source = audioSlotGirlGold;
        else if (audioName.Contains("Slot"))
            source = audioSlot;
        else
            source = audioSFX;

        source.Stop();
    }
    public void MuteBGM()
    {
        audioBGM.mute = true;
    }
    public void UnmuteBGM()
    {
        audioBGM.mute = false;
    }
}
