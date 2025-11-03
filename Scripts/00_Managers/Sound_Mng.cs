using System;
using UnityEngine;

[Serializable]
public class SoundClip
{
    public string name;
    public AudioClip clip;
    [Range(0f, 1f)] public float volume = 1.0f;
}

public class Sound_Mng : Singleton<Sound_Mng>
{
    [SerializeField] private SoundClip[] sfxClips;
    [SerializeField] private SoundClip[] bgmClips;
    private AudioSource _audioSource;
    private AudioSource _bgmSource;

    protected override void Awake()
    {
        base.Awake();
        AudioSource[] sources = GetComponents<AudioSource>();
        if (sources.Length >= 2)
        {
            _audioSource = sources[0];
            _bgmSource = sources[1];
            _bgmSource.loop = true;
        }
        else
        {
            Debug.LogError("AudioSource가 2개 필요합니다!");
        }
    }

    public void PlaySFX(string clipName)
    {
        if (_audioSource == null)
            return;

        SoundClip clip = null;

        foreach (var c in sfxClips)
        {
            if (c.name == clipName)
            {
                clip = c;
                break;
            }
        }
        if (clip == null)
        {
            Debug.LogWarning("효과음을 찾을 수 없습니다: " + clipName);
            return;
        }
        _audioSource.PlayOneShot(clip.clip, clip.volume);
    }

    public void PlayBGM(string clipName)
    {
        if (_bgmSource == null)
            return;

        SoundClip clip = null;

        foreach (var c in bgmClips)
        {
            if (c.name == clipName)
            {
                clip = c;
                break;
            }
        }
        if (clip == null)
        {
            Debug.LogWarning("BGM을 찾을 수 없습니다: " + clipName);
            return;
        }
        _bgmSource.clip = clip.clip;
        _bgmSource.volume = clip.volume;
        _bgmSource.Play();
    }
}
