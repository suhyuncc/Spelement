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
                //��Ÿ
                Debug.Log($"��Ÿ!! {nomal_demage}�� ������!!");
            }
            else
            {
                //��ų �� �۵�
                current_cool = spell_cool;
                Debug.Log($"{BattleManager.instance.Name[spell_id]}�� ��ų �ߵ����� {current_cool}�� ���ҽ��ϴ�");
            }
        }
        else
        {
            current_cool--;
            if (current_cool == 0)
            {
                Debug.Log($"��ų �ߵ�!!");
            }
            else
            {
                Debug.Log($"{BattleManager.instance.Name[spell_id]}�� ��ų �ߵ����� {current_cool}�� ���ҽ��ϴ�");
            }
        }
        
    }
}
