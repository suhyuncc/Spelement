using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpellAction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Text Name;
    [SerializeField]
    private Text description;
    [SerializeField]
    private GameObject spell;
    [SerializeField]
    private GameObject[] costs;
    [SerializeField]
    private Sprite[] sprites;

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

    public int spell_id;

    private void Awake()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("mouse over");
        description.gameObject.SetActive(true);
        Name.gameObject.SetActive(true);

        spell.SetActive(false);

        for (int i = 0; i < total; i++) {
            costs[i].SetActive(false);
        }
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("mouse Exit");

        description.gameObject.SetActive(false);
        Name.gameObject.SetActive(false);

        spell.SetActive(true);
        for (int i = 0; i < total; i++)
        {
            costs[i].SetActive(true);
        }
    }

    public void getfire() {
        int count = 0;
        for(int i = 0; i < costs.Length; i++)
        {
            if (costs[i].GetComponent<SpriteRenderer>().sprite.name == "원소_불"
                && costs[i].GetComponent<SpriteRenderer>().color.a != 1)
            {
                costs[i].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                break;
            }
            count++;
            if(count > Fire_num)
            {
                getnull();
                break;
            }
        }
        Debug.Log($"불 작동");
    }

    public void getwater()
    {
        int count = 0;
        for (int i = 0; i < costs.Length; i++)
        {
            if (costs[i].GetComponent<SpriteRenderer>().sprite.name == "원소_물"
                && costs[i].GetComponent<SpriteRenderer>().color.a != 1)
            {
                costs[i].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                break;
            }
            count++;
            if (count > Water_num)
            {
                getnull();
                break;
            }
        }
        Debug.Log($"물 작동");
    }

    public void getair()
    {
        int count = 0;
        for (int i = 0; i < costs.Length; i++)
        {
            if (costs[i].GetComponent<SpriteRenderer>().sprite.name == "원소_바람"
                && costs[i].GetComponent<SpriteRenderer>().color.a != 1)
            {
                costs[i].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                break;
            }
            count++;
            if (count > Air_num)
            {
                getnull();
                break;
            }
        }
        Debug.Log($"바람 작동");
    }

    public void getearth()
    {
        int count = 0;
        for (int i = 0; i < costs.Length; i++)
        {
            if (costs[i].GetComponent<SpriteRenderer>().sprite.name == "원소_땅"
                && costs[i].GetComponent<SpriteRenderer>().color.a != 1)
            {
                costs[i].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                break;
            }
            count++;
            if (count > Earth_num)
            {
                getnull();
                break;
            }
        }
        Debug.Log($"땅 작동");
    }

    public void getnull()
    {
        for (int i = 0; i < costs.Length; i++)
        {
            if (costs[i].GetComponent<SpriteRenderer>().sprite.name == "원소_무"
                && costs[i].GetComponent<SpriteRenderer>().color.a != 1)
            {
                costs[i].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                break;
            }
        }
        Debug.Log($"무 작동");
    }

    public void spellSetting() {
        Name.text = $"<{BattleManager.instance.Name[spell_id]}>";

        description.text = $"{BattleManager.instance.discription[spell_id]}";

        Null_num = BattleManager.instance.Null[spell_id];
        Air_num = BattleManager.instance.Air[spell_id];
        Earth_num = BattleManager.instance.Earth[spell_id];
        Water_num = BattleManager.instance.Water[spell_id];
        Fire_num = BattleManager.instance.Fire[spell_id];


        Total_num[0] = Null_num;
        Total_num[1] = Air_num;
        Total_num[2] = Earth_num;
        Total_num[3] = Water_num;
        Total_num[4] = Fire_num;

        total = Null_num + Air_num + Earth_num + Water_num + Fire_num;
        if(total == 0)
        {
            return;
        }

        int index = 0;
        for (int i = Total_num.Length - 1; i >= 0;i--)
        {
            if (Total_num[i] != 0) {
                for (int j = index; j < index + Total_num[i]; j++)
                {
                    costs[j].SetActive(true);
                    costs[j].GetComponent<SpriteRenderer>().sprite = sprites[i];
                    costs[j].GetComponent<SpriteRenderer>().color = new Color(1,1,1,0.3f);
                    
                }
                index += Total_num[i];
            }
        }
    }
}
