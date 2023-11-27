using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellSetting : MonoBehaviour
{
    [SerializeField]
    private Image spell_icon;
    [SerializeField]
    private Image cost_default;
    [SerializeField]
    private Image plame;
    [SerializeField]
    private Sprite defualt;
    [SerializeField]
    private GameObject[] costs;
    [SerializeField]
    private Sprite[] Jam_sprites;

    [SerializeField]
    private bool isUpper;
    [SerializeField]
    private int page_id;

    private int Null_num;
    private int Air_num;
    private int Earth_num;
    private int Water_num;
    private int Fire_num;

    private int[] Total_num = new int[5];
    private int total;

    public bool set_Anim;

    public void Spellsetting(int spell_id)
    {
        SpellCustom_Manager.instance.spell_set(spell_id, page_id);

        spell_icon.sprite = SpellCustom_Manager.instance.sprites[spell_id];

        for (int i = 0; i < costs.Length; i++)
        {
            costs[i].SetActive(false);
        }

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

        cost_default.gameObject.SetActive(false);

        //스펠 코스트 표현
        //코스트 수
        Null_num = SpellCustom_Manager.instance.Null[spell_id];
        Air_num = SpellCustom_Manager.instance.Air[spell_id];
        Earth_num = SpellCustom_Manager.instance.Earth[spell_id];
        Water_num = SpellCustom_Manager.instance.Water[spell_id];
        Fire_num = SpellCustom_Manager.instance.Fire[spell_id];


        Total_num[0] = Null_num;
        Total_num[1] = Air_num;
        Total_num[2] = Earth_num;
        Total_num[3] = Water_num;
        Total_num[4] = Fire_num;

        total = Null_num + Air_num + Earth_num + Water_num + Fire_num;
        if (total == 0)
        {
            return;
        }

        int index = 0;
        for (int i = Total_num.Length - 1; i >= 0; i--)
        {
            if (Total_num[i] != 0)
            {
                for (int j = index; j < index + Total_num[i]; j++)
                {
                    costs[j].SetActive(true);
                    costs[j].GetComponent<SpriteRenderer>().sprite = Jam_sprites[i];
                    costs[j].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);

                }
                index += Total_num[i];
            }
        }

        if (set_Anim)
        {
            //마법진 그리기
            StartCoroutine("Set");
            set_Anim = false;
        }
        
    }

    public void Initialized()
    {

        spell_icon.sprite = defualt;

        cost_default.gameObject.SetActive(true);

        if (isUpper)
        {
            plame.sprite = SpellCustom_Manager.instance.Upper_sprites[0];
        }
        else
        {
            plame.sprite = SpellCustom_Manager.instance.Down_sprites[0];
        }

        for (int i = 0; i < costs.Length; i++)
        {
            costs[i].SetActive(false);
        }
    }

    IEnumerator Set()
    {
        plame.fillAmount = 0f;

        SpellCustom_Manager.instance.next.interactable = false;
        SpellCustom_Manager.instance.back.interactable = false;

        while (plame.fillAmount < 1)
        {
            plame.fillAmount += 0.0032f;
            yield return null;
        }

        SpellCustom_Manager.instance.next.interactable = true;
        SpellCustom_Manager.instance.back.interactable = true;
    }
}
