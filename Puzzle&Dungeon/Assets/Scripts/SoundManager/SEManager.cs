using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class SEManager : AudioManagerBase
{
    public SEManager(AudioSource source) : base(source) { source.volume = 1; }

    public override void PlayAudio(AudioClip clip, bool isLoop)
    {
        var key = Sounds.SE_PATHS.FirstOrDefault(kvp => kvp.Value == clip.name).Key;
        
        var defaultVol = Sounds.SE_VOLUMES[key];



        if (!IsPlayingAudio())
        {
            base.PlayAudio(clip, isLoop);
        }
    }
}
