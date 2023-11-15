using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spell_discript : MonoBehaviour
{
    [SerializeField]
    private Text name;
    [SerializeField]
    private Text discript;
    [SerializeField]
    private Image spell_icon;
    [SerializeField]
    private Image[] gems;
    [SerializeField]
    private Sprite[] Jam_sprites;
    [SerializeField]
    private GameObject[] costs;

    [SerializeField]
    private int Null_num;
    [SerializeField]
    private int Air_num;
    [SerializeField]
    private int Earth_num;
    [SerializeField]
    private int Water_num;
    [SerializeField]
    private int Fire_num;

    private int[] Total_num = new int[5];
    private int total;

    [SerializeField]
    private int spell_id;

    private void OnEnable()
    {
        spell_id = SpellCustom_Manager.instance.cur_id;

        name.text = SpellCustom_Manager.instance.Name[spell_id];
        discript.text = SpellCustom_Manager.instance.discription[spell_id];

        spell_icon.sprite = SpellCustom_Manager.instance.sprites[spell_id];

        for (int i = 0; i < costs.Length; i++)
        {
            costs[i].SetActive(false);
        }

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
                    gems[j].sprite = Jam_sprites[i];

                }
                index += Total_num[i];
            }
        }
    }
}
