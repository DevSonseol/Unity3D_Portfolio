using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System.IO;

public enum SoundType
{
    Master,
    BGM,
    UI,
    SFX,
    Count
}

public partial class Main : MonoBehaviour
{
    [Header("Sound")]
    public AudioMixer audioMixer;
    public AudioSource[] audioSources = new AudioSource[(int)SoundType.Count];

    [SerializeField] private Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

    void InitSound()
    {
        AddSoundClip();
    }

    void AddSoundClip()
    {
        AudioClip[] Clips = Resources.LoadAll<AudioClip>("Audio");

        foreach(AudioClip clip in Clips)
        {
            audioClips.Add(clip.name, clip);
            Debug.Log(clip.name + " : " + audioClips[clip.name] +"음악이 등록되었습니다");
        }
    }

    public void PlaySound(SoundType type , string SoundName)
    {
        if (audioClips.TryGetValue(SoundName, out AudioClip value))
        {
            audioSources[(int)type].clip = audioClips[SoundName];
            audioSources[(int)type].Play();
        }
        else
        {
            Debug.Log("없는 사운드입니다");
        }

    }
    public void StopSound(SoundType type)
    {
        audioSources[(int)type].Stop();
    }

}
