using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BGMManager : AudioManagerBase
{

    public BGMManager(AudioSource source) : base(source) { }



    /// <summary>
    /// ���݂�BGM�̉��ʂ��擾
    /// </summary>
    /// <returns>���݂�BGM�̉���</returns>
    public float GetAudioVolume()
    {
        return audioSource.volume;
    }

    /// <summary>
    /// �w�艹�ʏ���������
    /// </summary>
    /// <param name="decVol">�������鉹��</param>
    public void FadeOutAudio(float decVol)
    {
        audioSource.volume -= decVol;
    }

    /// <summary>
    /// ���ʂ̎������ߋ@�\��ǉ�����
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="isLoop"></param>
    public override void PlayAudio(AudioClip clip, bool isLoop)
    {
        var key = Sounds.BGM_PATHS.FirstOrDefault(kvp => kvp.Value == clip.name).Key;
        volumeDefault = Sounds.BGM_VOLUMES[key];

        audioSource.volume = volumeDefault * volumeRate;

        base.PlayAudio(clip, isLoop);
    }
}
