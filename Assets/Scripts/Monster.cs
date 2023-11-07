using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public int nomal_demage;
    public int spell_id;
    public int spell_cool;

    private int current_cool;

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
        if (current_cool == 0) {
            float ran = Random.Range(0f, 1f);
            if (ran > 0.5f)
            {
                //평타
                SkillManager.instance.player_turn = false;
                SkillManager.instance.spell_id = 19;
                SkillManager.instance.isActive = true;
                Debug.Log($"평타!! {nomal_demage}의 데미지!!");
            }
            else
            {
                //스킬 쿨 작동
                current_cool = spell_cool;
                Debug.Log($"{BattleManager.instance.Name[spell_id]}의 스킬 발동까지 {current_cool}턴 남았습니다");
                BattleManager.instance.monster_done = true;
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
            }
            else
            {
                Debug.Log($"{BattleManager.instance.Name[spell_id]}의 스킬 발동까지 {current_cool}턴 남았습니다");
                BattleManager.instance.monster_done = true;
            }
        }

    }
}
