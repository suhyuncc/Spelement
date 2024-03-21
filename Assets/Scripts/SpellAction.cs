using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
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
    private Image spell_page;
    [SerializeField]
    private Button spell_icon;
    [SerializeField]
    private GameObject[] costs;
    [SerializeField]
    private Sprite[] Jam_sprites;
    
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

    [SerializeField]
    private bool isUpper;

    private int[] Total_num = new int[5];
    private int total;
    private int ready_count;

    public bool isDone;

    public int spell_id;

    private BoxCollider2D collide_Area;

    private void Awake()
    {
        collide_Area = this.GetComponent<BoxCollider2D>();
    }
    private void Update()
    {
        if(ready_count >= total && !isDone)
        {
            spell_icon.interactable = true;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(isDone)
        {
            return;
        }

        if (ready_count < total)
        {
            description.gameObject.SetActive(true);
            Name.gameObject.SetActive(true);

            spell.SetActive(false);

            for (int i = 0; i < total; i++)
            {
                costs[i].SetActive(false);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isDone)
        {
            return;
        }

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
            if (costs[i].GetComponent<SpriteRenderer>().sprite.name == "����_��"
                && costs[i].GetComponent<SpriteRenderer>().color.a != 1)
            {
                costs[i].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                ready_count++;
                Debug.Log($"�� �۵�");
                break;
            }
            else
            {
                count++;
            }
        }
        if (count > total - Null_num)
        {
            getnull();
        }
        
    }

    public void getwater()
    {
        int count = 0;
        for (int i = 0; i < costs.Length; i++)
        {
            if (costs[i].GetComponent<SpriteRenderer>().sprite.name == "����_��"
                && costs[i].GetComponent<SpriteRenderer>().color.a != 1)
            {
                costs[i].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                ready_count++;
                Debug.Log($"�� �۵�");
                break;
            }
            else
            {
                count++;
            }
        }
        if (count > total - Null_num)
        {
            getnull();
        }
        
    }

    public void getair()
    {
        int count = 0;
        for (int i = 0; i < costs.Length; i++)
        {
            if (costs[i].GetComponent<SpriteRenderer>().sprite.name == "����_�ٶ�"
                && costs[i].GetComponent<SpriteRenderer>().color.a != 1)
            {
                costs[i].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                ready_count++;
                Debug.Log($"�ٶ� �۵�");
                break;
            }
            else
            {
                count++;
            }

        }
        if (count > total - Null_num)
        {
            getnull();
        }

        
    }

    public void getearth()
    {
        int count = 0;
        for (int i = 0; i < costs.Length; i++)
        {
            if (costs[i].GetComponent<SpriteRenderer>().sprite.name == "����_��"
                && costs[i].GetComponent<SpriteRenderer>().color.a != 1)
            {
                costs[i].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                ready_count++;
                Debug.Log($"�� �۵�");
                break;
            }
            else
            {
                count++;
            }
        }

        if (count > total - Null_num)
        {
            getnull();
        }
        
    }

    public void getnull()
    {
        for (int i = 0; i < costs.Length; i++)
        {
            if (costs[i].GetComponent<SpriteRenderer>().sprite.name == "����_��"
                && costs[i].GetComponent<SpriteRenderer>().color.a != 1)
            {
                costs[i].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                ready_count++;
                Debug.Log($"�� �۵�");
                break;
            }
        }
        
    }

    public void spellSetting() {

        //this.gameObject.SetActive(true);
        collide_Area.enabled = true;

        spell_icon.interactable = true;
        spell_icon.gameObject.SetActive(true);

        spell_page.color = new Color(1, 1, 1, 1f);

        
        for (int i = 0; i < costs.Length; i++)
        {
            costs[i].SetActive(false);
        }


        isDone = false;
        ready_count = 0;


        //���� �̸�
        Name.text = $"<{BattleManager.instance.Name[spell_id]}>";

        //���� ����
        description.text = $"{BattleManager.instance.discription[spell_id]}";

        //���� �޹�� ����
        if(spell_id - 3 < 0)
        {
            if (isUpper)
            {
                spell_page.sprite = BattleManager.instance.Upper_sprites[0];
            }
            else 
            {
                spell_page.sprite = BattleManager.instance.Down_sprites[0];
            }
        }
        else
        {
            if (isUpper)
            {
                spell_page.sprite = BattleManager.instance.Upper_sprites[((spell_id - 3) / 4) + 1];
            }
            else
            {
                spell_page.sprite = BattleManager.instance.Down_sprites[((spell_id - 3) / 4) + 1];
            }
        }

        //���� ������ ����
        spell_icon.image.sprite = BattleManager.instance.icons[spell_id];

        spell_icon.interactable = false;

        //�ڽ�Ʈ ��
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
                    costs[j].GetComponent<SpriteRenderer>().sprite = Jam_sprites[i];
                    costs[j].GetComponent<SpriteRenderer>().color = new Color(1,1,1,0.3f);
                    
                }
                index += Total_num[i];
            }
        }
    }

    //���� ��ư���� �۵�(���� ��� �� ����)
    public void spellDone()
    {
        SkillManager.instance.SFX_On(0, 6);
        spell_icon.interactable = false;
        spell_icon.gameObject.SetActive(false);

        spell_page.color = new Color(1, 1, 1, 0.5f);
        for (int i = 0; i < total; i++)
        {
            costs[i].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.3f);
            costs[i].SetActive(false);
        }

        collide_Area.enabled = false;

        BattleManager.instance.spell_count--;

        SkillManager.instance.player_turn = true;
        SkillManager.instance.spell_id = spell_id;
        SkillManager.instance.isActive = true;

        isDone = true;
    }

}
