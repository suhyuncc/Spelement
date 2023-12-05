using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
//gh
public class AudioSlider : MonoBehaviour
{
    [SerializeField] private AudioMixer Mixer;
    [SerializeField] private Slider Master;
    [SerializeField] private Slider BGM;
    [SerializeField] private Slider EffectV;
    [SerializeField] private Toggle masterT;
    [SerializeField] private Toggle BGMT;
    [SerializeField] private Toggle EffectT;
    [SerializeField] private TMP_Text masterText;
    [SerializeField] private TMP_Text BGMText;
    [SerializeField] private TMP_Text EffectText;

    [SerializeField]
    private GameObject gm;

    private void Start()
    {
        gm = GameObject.Find("GameManager");
    }

    public void OnSceneChangedSettingAudio(float MasterA, float BGMA, float EffectA, bool ToggleM, bool ToggleB, bool ToggleE)
    {
        Master.value = MasterA;
        BGM.value = BGMA;
        EffectV.value = EffectA;
        masterT.isOn = ToggleM;
        BGMT.isOn = ToggleB;
        EffectT.isOn = ToggleE;
        SetMasterVolume();
        SetBGMVolume();
        SetEffectVolume();
    }

    public void SetMasterVolume ()
    {
        float sound = Master.value;
        masterText.text = (int)sound + "%";
        if (masterT.isOn)
        {
            Mixer.SetFloat("Master", -80);
        }
        else
        {
            if (sound == 0) Mixer.SetFloat("Master", -80);
            else
            {
                sound = -40 + sound * 4 / 10;
                Mixer.SetFloat("Master", sound);
            }
        }
        gm.GetComponent<GameManager>().SetVolM(Master.value, masterT.isOn);
    }

    public void SetBGMVolume()
    {
        float sound = BGM.value;
        BGMText.text = (int)sound + "%";
        if (BGMT.isOn)
        {
            Mixer.SetFloat("Background", -80);
        }
        else
        {
            if (sound == 0) Mixer.SetFloat("Background", -80);
            else
            {
                sound = -40 + sound * 4 / 10;
                Mixer.SetFloat("Background", sound);
            }
        }
        gm.GetComponent<GameManager>().SetVolB(BGM.value, BGMT.isOn);
    }

    public void SetEffectVolume()
    {

        float sound = EffectV.value;
        EffectText.text = (int)sound + "%";
        if (EffectT.isOn)
        {
            Mixer.SetFloat("Effect", -80);
        }
        else
        {
            if (sound == 0) Mixer.SetFloat("Effect", -80);
            else
            {
                sound = -40 + sound * 4 / 10;
                Mixer.SetFloat("Effect", sound);
            }
        }
        gm.GetComponent<GameManager>().SetVolE(EffectV.value, EffectT.isOn);
    }

    public void MasterToggleClicked()
    {
        if(masterT.isOn)
        {
            masterText.color = new Color(masterText.color.r, masterText.color.g, masterText.color.b, 0.1f);
            Mixer.SetFloat("Master", -80);
        }
        else
        {
            masterText.color = new Color(masterText.color.r, masterText.color.g, masterText.color.b, 1f);
            float sound = Master.value * 4 / 10;
            if (sound == 0) Mixer.SetFloat("Master", -80);
            else
            {
                sound -= 40;
                Mixer.SetFloat("Master", sound);
            }
        }
        gm.GetComponent<GameManager>().SetVolM(Master.value, masterT.isOn);
    }
    public void BackToggleClicked()
    {
        if (BGMT.isOn)
        {
            BGMText.color = new Color(BGMText.color.r, BGMText.color.g, BGMText.color.b, 0.1f);
            Mixer.SetFloat("Background", -80);
        }
        else
        {
            BGMText.color = new Color(BGMText.color.r, BGMText.color.g, BGMText.color.b, 1f);
            float sound = BGM.value * 4 / 10;
            if (sound == 0) Mixer.SetFloat("Background", -80);
            else
            {
                sound -= 40;
                Mixer.SetFloat("Background", sound);
            }
        }
        gm.GetComponent<GameManager>().SetVolM(Master.value, masterT.isOn);
    }
    public void EffectToggleClicked()
    {
        if (EffectT.isOn)
        {
            EffectText.color = new Color(EffectText.color.r, EffectText.color.g, EffectText.color.b, 0.1f);
            Mixer.SetFloat("Effect", -80);
        }
        else
        {
            EffectText.color = new Color(EffectText.color.r, EffectText.color.g, EffectText.color.b, 1f);
            float sound = EffectV.value * 4 / 10;
            if (sound == 0) Mixer.SetFloat("Effect", -80);
            else
            {
                sound -= 40;
                Mixer.SetFloat("Effect", sound);
            }
        }
        gm.GetComponent<GameManager>().SetVolE(EffectV.value, EffectT.isOn);
    }
}
