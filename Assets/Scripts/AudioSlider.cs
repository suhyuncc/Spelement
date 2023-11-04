using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSlider : MonoBehaviour
{
    [SerializeField] private AudioMixer Mixer;
    [SerializeField] private Slider Master;
    [SerializeField] private Slider BGM;
    [SerializeField] private Slider Effect;


    public void SetMasterVolume ()
    {
        float sound = Master.value;

        if (sound == 0) Mixer.SetFloat("Master", -80);
        else
        {
            sound = -40 + sound * 4 / 10;
            Mixer.SetFloat("Master", sound);
        }
    }

    public void SetBGMVolume()
    {
        float sound = BGM.value;

        if (sound == 0) Mixer.SetFloat("Background", -80);
        else
        {
            sound = -40 + sound * 4 / 10;
            Mixer.SetFloat("Background", sound);
        }
    }

    public void SetEffectVolume()
    {

        float sound = Effect.value;

        if (sound == 0) Mixer.SetFloat("Effect", -80);
        else
        {
            sound = -40 + sound * 4 / 10;
            Mixer.SetFloat("Effect", sound);
        }
    }
}
