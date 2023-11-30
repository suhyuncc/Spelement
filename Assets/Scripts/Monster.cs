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
    [SerializeField]
    private Sprite[] sprites;
    [SerializeField]
    private GameObject[] shadows;

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

    public void monster_Setting(int stage_num)
    {
        this.GetComponent<SpriteRenderer>().sprite = sprites[stage_num];
        shadows[stage_num].SetActive(true);

        if(stage_num == 2)
        {
            this.GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            this.GetComponent<SpriteRenderer>().flipX = false;
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
                //��Ÿ
                SkillManager.instance.player_turn = false;
                SkillManager.instance.spell_id = 19;
                SkillManager.instance.isActive = true;
                SkillManager.instance.nomal_Dem = nomal_demage;
                Debug.Log($"��Ÿ!! {nomal_demage}�� ������!!");
                sys_text.text = $"��Ÿ!! {nomal_demage}�� ������!!";
                sys_text.gameObject.SetActive(true);
            }
            else
            {
                //��ų �� �۵�
                current_cool = spell_cool;
                StartCoroutine(turn_pass($"{BattleManager.instance.Name[spell_id]}�� ��ų �ߵ����� {current_cool}�� ���ҽ��ϴ�"));
                Debug.Log($"{BattleManager.instance.Name[spell_id]}�� ��ų �ߵ����� {current_cool}�� ���ҽ��ϴ�");

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
                Debug.Log($"��ų �ߵ�!!");
                sys_text.text = $"{BattleManager.instance.Name[spell_id]}!!";
                sys_text.gameObject.SetActive(true);
            }
            else
            {
                StartCoroutine(turn_pass($"{BattleManager.instance.Name[spell_id]}�� ��ų �ߵ����� {current_cool}�� ���ҽ��ϴ�"));
                Debug.Log($"{BattleManager.instance.Name[spell_id]}�� ��ų �ߵ����� {current_cool}�� ���ҽ��ϴ�");
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
                //��Ÿ
                SkillManager.instance.player_turn = false;
                SkillManager.instance.spell_id = 19;
                SkillManager.instance.isActive = true;
                SkillManager.instance.nomal_Dem = nomal_demage;
                Debug.Log($"��Ÿ!! {nomal_demage}�� ������!!");
                sys_text.text = $"��Ÿ!! {nomal_demage}�� ������!!";
                sys_text.gameObject.SetActive(true);
            }
            //��ų1
            else if(ran > 0.25f)
            {
                current_spell_id = (spell_id / 2) - 1;
                //��ų �� �۵�
                current_cool = spell_cool;
                StartCoroutine(turn_pass($"{BattleManager.instance.Name[current_spell_id]}�� ��ų �ߵ����� {current_cool}�� ���ҽ��ϴ�"));
                Debug.Log($"{BattleManager.instance.Name[current_spell_id]}�� ��ų �ߵ����� {current_cool}�� ���ҽ��ϴ�");

            }
            //��ų2
            else
            {
                current_spell_id = (spell_id / 2);
                //��ų �� �۵�
                current_cool = spell_cool;
                StartCoroutine(turn_pass($"{BattleManager.instance.Name[current_spell_id]}�� ��ų �ߵ����� {current_cool}�� ���ҽ��ϴ�"));
                Debug.Log($"{BattleManager.instance.Name[current_spell_id]}�� ��ų �ߵ����� {current_cool}�� ���ҽ��ϴ�");
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
                Debug.Log($"��ų �ߵ�!!");
                sys_text.text = $"{BattleManager.instance.Name[current_spell_id]}!!";
                sys_text.gameObject.SetActive(true);
            }
            else
            {
                StartCoroutine(turn_pass($"{BattleManager.instance.Name[current_spell_id]}�� ��ų �ߵ����� {current_cool}�� ���ҽ��ϴ�"));
                Debug.Log($"{BattleManager.instance.Name[current_spell_id]}�� ��ų �ߵ����� {current_cool}�� ���ҽ��ϴ�");
            }
        }
    }
}
