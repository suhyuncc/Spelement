using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellSetting : MonoBehaviour
{
    [SerializeField]
    private Text spell_name;
    [SerializeField]
    private Image spell_icon;
    [SerializeField]
    private Image plame;
    [SerializeField]
    private Sprite defualt;


    [SerializeField]
    private bool isUpper;
    [SerializeField]
    private int page_id;

    public void Spellsetting(int spell_id)
    {
        SpellCustom_Manager.instance.spell_set(spell_id, page_id);

        spell_name.text = SpellCustom_Manager.instance.Name[spell_id];

        spell_icon.sprite = SpellCustom_Manager.instance.sprites[spell_id];

        //스펠 뒷배경 설정
        if (spell_id - 3 < 0)
        {
            if (isUpper)
            {
                plame.sprite = SpellCustom_Manager.instance.Upper_sprites[0];
            }
            else
            {
                plame.sprite = SpellCustom_Manager.instance.Down_sprites[0];
            }
        }
        else
        {
            if (isUpper)
            {
                plame.sprite = SpellCustom_Manager.instance.Upper_sprites[((spell_id - 3) / 4) + 1];
            }
            else
            {
                plame.sprite = SpellCustom_Manager.instance.Down_sprites[((spell_id - 3) / 4) + 1];
            }
        }

        //마법진 그리기
        StartCoroutine("Set");
    }

    public void Initialized()
    {
        spell_name.text = "<????>";

        spell_icon.sprite = defualt;

        if (isUpper)
        {
            plame.sprite = SpellCustom_Manager.instance.Upper_sprites[0];
        }
        else
        {
            plame.sprite = SpellCustom_Manager.instance.Down_sprites[0];
        }
    }

    IEnumerator Set()
    {
        plame.fillAmount = 0f;


        while (plame.fillAmount < 1)
        {
            plame.fillAmount += 0.0032f;
            yield return null;
        }
    }
}
