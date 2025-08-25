using System.Collections.Generic;
using UnityEngine;

/// ����:
/// SoundManager.Instance.PlayBGM("MainBGM");     BGM �ѱ� (���ϸ���! Ȯ����/������ ����)
/// SoundManager.Instance.StopBGM();              BGM ����
/// SoundManager.Instance.PlaySFX("Click");       ȿ���� ��� (���ϸ���! Ȯ����/������ ����)
/// SoundManager.Instance.SetBGMVolume(0.7f);     BGM ���� ����
/// SoundManager.Instance.SetSFXVolume(0.5f);     ȿ���� ���� ����
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    private AudioSource bgmSource;
    private List<AudioSource> sfxSources = new List<AudioSource>();

    [Header("BGM ���� (0~1)")]
    [Range(0, 1)]
    public float bgmVolume = 1.0f;

    [Header("ȿ���� ���� (0~1)")]
    [Range(0, 1)]
    public float sfxVolume = 1.0f;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.loop = true;
        bgmSource.playOnAwake = false;
        bgmSource.volume = bgmVolume;
        bgmSource.spatialBlend = 0f;
    }

    /// BGM(mp3) ���
    public void PlayBGM(string bgmName)
    {
        AudioClip clip = Resources.Load<AudioClip>("BGM/" + bgmName);
        if (clip == null)
        {
            return;
        }
        if (bgmSource.clip == clip && bgmSource.isPlaying)
            return;
        bgmSource.clip = clip;
        bgmSource.volume = bgmVolume;
        bgmSource.Play();
    }

    /// BGM ����
    public void StopBGM()
    {
        bgmSource.Stop();
        bgmSource.clip = null;
    }

    /// BGM ���� ����
    public void SetBGMVolume(float volume)
    {
        bgmVolume = Mathf.Clamp01(volume);
        bgmSource.volume = bgmVolume;
    }

    /// ȿ����(ogg) ���
    public void PlaySFX(string sfxName)
    {
        if (sfxName.Contains("/") || sfxName.Contains("\\") || sfxName.Contains("."))
        {
            sfxName = System.IO.Path.GetFileNameWithoutExtension(sfxName); // �׷��� �ִ��� ����
        }

        AudioClip clip = Resources.Load<AudioClip>("SFX/" + sfxName);
        if (clip == null)
        {
            return;
        }

        AudioSource src = null;
        foreach (var s in sfxSources)
        {
            if (!s.isPlaying)
            {
                src = s;
                break;
            }
        }
        if (src == null)
        {
            src = gameObject.AddComponent<AudioSource>();
            sfxSources.Add(src);
        }
        src.clip = clip;
        src.volume = sfxVolume;
        src.loop = false;
        src.spatialBlend = 0f;
        src.Play();
    }

    /// ȿ���� ���� ����
    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        foreach (var src in sfxSources)
            src.volume = sfxVolume;
    }
}
