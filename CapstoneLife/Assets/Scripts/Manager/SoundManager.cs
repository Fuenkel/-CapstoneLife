using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;
using UnityEngine.Audio;
[Serializable]
public class Sound{
    public AudioMixerGroup audioMixerGroup;
    private AudioSource source;
    public string clipName;
    public AudioClip clip;

    [Range(0f,1f)]
    public float volume;
    [Range(0f,3f)]
    public float pitch;
    public bool loop = false;
    public bool playOnAwake = false;

    public void SetSource(AudioSource _source)
    {
        source = _source;
        source.clip = clip;
        source.pitch = pitch;
        source.volume = volume;
        source.playOnAwake = playOnAwake;
        source.loop = loop;
        source.outputAudioMixerGroup = audioMixerGroup;
    }

    public void Play() => source.Play();
}
public class SoundManager : Singleton<SoundManager>
{
    [SerializeField]
    Sound[] sounds;

    void Start(){
        for (int i = 0; i < sounds.Length; i++)
        {
            GameObject _go = new GameObject(string.Format("Sound_{0}_{1}",i,sounds[i].clipName));
            _go.transform.SetParent(this.transform);

            sounds[i].SetSource(_go.AddComponent<AudioSource>());
        }

        
    }

    public void PlaySound(string _Name){
        Debug.Log("Play " + _Name);
        for (int i = 0; i < sounds.Length; i++)        {
        Debug.Log(sounds[i].clipName == _Name);
        if (sounds[i].clipName == _Name)
        {
            sounds[i].Play();
            return;
        }
        }
    }
}
