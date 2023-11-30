using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    public bool is_Boss;

    public int nomal_demage;
    public int spell_id;
    public int spell_cool;

    private int current_spell_id;
    private int current_cool;

    [SerializeField]
    private Text sys_text;

    // Start is called before the first frame update
    void Start()
    {
        current_cool = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void monster_active() {
        if (is_Boss)
        {
            StartCoroutine("Boss_Monster_turn");
        }
        else
        {
            StartCoroutine("Monster_turn");
        }
        
        

    }

    IEnumerator turn_pass(string s)
    {
        sys_text.text = s;
        sys_text.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.7f);
        BattleManager.instance.phase = Phase.End;
        BattleManager.instance.monster_done = true;

    }


    IEnumerator Monster_turn()
    {
        yield return new WaitForSeconds(0.5f);
        if (current_cool == 0)
        {
            float ran = Random.Range(0f, 1f);
            if (ran > 0.5f)
            {
                //평타
                SkillManager.instance.player_turn = false;
                SkillManager.instance.spell_id = 19;
                SkillManager.instance.isActive = true;
                SkillManager.instance.nomal_Dem = nomal_demage;
                Debug.Log($"평타!! {nomal_demage}의 데미지!!");
                sys_text.text = $"평타!! {nomal_demage}의 데미지!!";
                sys_text.gameObject.SetActive(true);
            }
            else
            {
                //스킬 쿨 작동
                current_cool = spell_cool;
                StartCoroutine(turn_pass($"{BattleManager.instance.Name[spell_id]}의 스킬 발동까지 {current_cool}턴 남았습니다"));
                Debug.Log($"{BattleManager.instance.Name[spell_id]}의 스킬 발동까지 {current_cool}턴 남았습니다");

            }
        }
        else
        {
            current_cool--;
            if (current_cool == 0)
            {
                SkillManager.instance.player_turn = false;
                SkillManager.instance.spell_id = spell_id;
                SkillManager.instance.isActive = true;
                Debug.Log($"스킬 발동!!");
                sys_text.text = $"{BattleManager.instance.Name[spell_id]}!!";
                sys_text.gameObject.SetActive(true);
            }
            else
            {
                StartCoroutine(turn_pass($"{BattleManager.instance.Name[spell_id]}의 스킬 발동까지 {current_cool}턴 남았습니다"));
                Debug.Log($"{BattleManager.instance.Name[spell_id]}의 스킬 발동까지 {current_cool}턴 남았습니다");
            }
        }
    }

    IEnumerator Boss_Monster_turn()
    {
        yield return new WaitForSeconds(0.5f);
        if (current_cool == 0)
        {
            float ran = Random.Range(0f, 1f);
            if (ran > 0.5f)
            {
                //평타
                SkillManager.instance.player_turn = false;
                SkillManager.instance.spell_id = 19;
                SkillManager.instance.isActive = true;
                SkillManager.instance.nomal_Dem = nomal_demage;
                Debug.Log($"평타!! {nomal_demage}의 데미지!!");
                sys_text.text = $"평타!! {nomal_demage}의 데미지!!";
                sys_text.gameObject.SetActive(true);
            }
            //스킬1
            else if(ran > 0.25f)
            {
                current_spell_id = (spell_id / 2) - 1;
                //스킬 쿨 작동
                current_cool = spell_cool;
                StartCoroutine(turn_pass($"{BattleManager.instance.Name[current_spell_id]}의 스킬 발동까지 {current_cool}턴 남았습니다"));
                Debug.Log($"{BattleManager.instance.Name[current_spell_id]}의 스킬 발동까지 {current_cool}턴 남았습니다");

            }
            //스킬2
            else
            {
                current_spell_id = (spell_id / 2);
                //스킬 쿨 작동
                current_cool = spell_cool;
                StartCoroutine(turn_pass($"{BattleManager.instance.Name[current_spell_id]}의 스킬 발동까지 {current_cool}턴 남았습니다"));
                Debug.Log($"{BattleManager.instance.Name[current_spell_id]}의 스킬 발동까지 {current_cool}턴 남았습니다");
            }
        }
        else
        {
            current_cool--;
            if (current_cool == 0)
            {
                SkillManager.instance.player_turn = false;
                SkillManager.instance.spell_id = current_spell_id;
                SkillManager.instance.isActive = true;
                Debug.Log($"스킬 발동!!");
                sys_text.text = $"{BattleManager.instance.Name[current_spell_id]}!!";
                sys_text.gameObject.SetActive(true);
            }
            else
            {
                StartCoroutine(turn_pass($"{BattleManager.instance.Name[current_spell_id]}의 스킬 발동까지 {current_cool}턴 남았습니다"));
                Debug.Log($"{BattleManager.instance.Name[current_spell_id]}의 스킬 발동까지 {current_cool}턴 남았습니다");
            }
        }
    }
}
