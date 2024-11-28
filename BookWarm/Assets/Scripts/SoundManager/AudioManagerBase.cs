using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <see cref="BGMManager"/>��<see cref="SEManager"/>�̋��ʊ��N���X
/// </summary>
public abstract class AudioManagerBase
{
    protected AudioSource audioSource;

    protected float volumeRate;
    protected float volumeDefault;

    public AudioManagerBase(AudioSource source)
    {
        audioSource = source;
        volumeRate = 1.0f;
        volumeDefault = 1.0f;
    }

    /// <summary>
    /// �����̍Đ�
    /// </summary>
    /// <param name="clip">�����t�@�C��</param>
    /// <param name="isLoop">���[�v�Đ����邩</param>
    public virtual void PlayAudio(AudioClip clip, bool isLoop)
    {
        audioSource.clip = clip;
        audioSource.loop = isLoop;
        audioSource.Play();
    }

    /// <summary>
    /// BGM�̒�~
    /// </summary>
    public void StopAudio()
    {
        audioSource.Stop();
    }

    /// <summary>
    /// BGM���Đ������ǂ���
    /// </summary>
    /// <returns>�Đ����Ȃ�true</returns>
    public bool IsPlayingAudio()
    {
        return audioSource.isPlaying;
    }

    /// <summary>
    /// ���ʂ̔{����ݒ�
    /// </summary>
    public void SetVolumeRate(float _rate)
    {
        volumeRate = _rate;
        //Debug.Log($"���� def:{volumeDefault}, rate:{volumeRate}");
        audioSource.volume = volumeDefault * volumeRate;
    }
}

