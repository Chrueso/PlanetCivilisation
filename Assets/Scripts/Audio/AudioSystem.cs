using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum AudioKey
{
    SFX, BGM, MASTER
}
public class AudioSystem : Singleton<AudioSystem>
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioSource SFXAudioSource;
    [SerializeField] private AudioSource BGMAudioSource;
    
    private Dictionary<AudioKey, string> AudioMixerLookup = new Dictionary<AudioKey, string>() {
        {AudioKey.MASTER, "MASTER"},
        {AudioKey.BGM, "BGM"},
        {AudioKey.SFX, "SFX"},
    };

    private Dictionary<string, List<AudioClip>> BatchQueue = new();

    public void AdjustVolume(AudioKey key, float volume)
    {
        if (!AudioMixerLookup.ContainsKey(key)) return;
        audioMixer.SetFloat(AudioMixerLookup[key], volume);
    }

    public void PlayOneShot(AudioClip clip, float volumeScale = 1.0f)
    {
        SFXAudioSource.PlayOneShot(clip, volumeScale);
    }

    public void PlaySoundBatch(AudioClip clip)
    {
        if (!BatchQueue.ContainsKey(clip.name))
        {
            BatchQueue[clip.name] = new List<AudioClip>() { clip };
        } else
        {
            BatchQueue[clip.name].Add(clip);
        }
    }

    public void PlayMusic(AudioClip music)
    {
        BGMAudioSource.Stop();
        BGMAudioSource.clip = music;
        BGMAudioSource.loop = true;
        BGMAudioSource.Play();
    }

    public void StopMusic()
    {
        BGMAudioSource.loop = false;
        BGMAudioSource.clip = null;
        BGMAudioSource.Stop();
    }

    private void LateUpdate()
    {
        if (BatchQueue.Count > 0) 
        {
            foreach (var audioQueue in BatchQueue)
            {
                AudioClip clip = audioQueue.Value[0];
                SFXAudioSource.PlayOneShot(clip, 1f + (clip.length));
            }
            BatchQueue.Clear();
        }
    }
}
