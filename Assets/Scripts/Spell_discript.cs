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
    private int spell_id;

    private void OnEnable()
    {
        name.text = SpellCustom_Manager.instance.Name[spell_id];
        discript.text = SpellCustom_Manager.instance.discription[spell_id];
    }
}
