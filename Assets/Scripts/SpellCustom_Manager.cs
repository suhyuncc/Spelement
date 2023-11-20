using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SpellCustom_Manager : MonoBehaviour
{
    public static SpellCustom_Manager instance;

    [SerializeField]
    private TextAsset csvFile = null;

    [SerializeField]
    private Button next;
    [SerializeField]
    private Button back;

    [SerializeField]
    private Skill_icon[] icons;
    [SerializeField]
    private GameObject[] spellPages;

    public Sprite[] sprites;
    public Sprite[] Upper_sprites;
    public Sprite[] Down_sprites;

    public int stage_lv;
    public int[] page_list;
    private int page_index;

    public string[] Name;
    public int[] Null;
    public int[] Air;
    public int[] Earth;
    public int[] Water;
    public int[] Fire;
    public string[] discription;

    public int cur_id;

    public void Set_Spell_Data()
    {
        string csvText = csvFile.text.Substring(0, csvFile.text.Length - 1); //get csv file as string type, except last line(empty)
        string[] row = csvText.Split(new char[] { '\n' }); //split by enter sign

        Name = new string[row.Length - 1];
        Null = new int[row.Length - 1];
        Air = new int[row.Length - 1];
        Earth = new int[row.Length - 1];
        Water = new int[row.Length - 1];
        Fire = new int[row.Length - 1];
        discription = new string[row.Length - 1];

        for (int i = 1; i < row.Length; i++)
        {
            string[] data = row[i].Split(new char[] { ',' }); //split by (,)

            Name[i - 1] = data[0];
            Null[i - 1] = int.Parse(data[1]);
            Air[i - 1] = int.Parse(data[2]);
            Earth[i - 1] = int.Parse(data[3]);
            Water[i - 1] = int.Parse(data[4]);
            Fire[i - 1] = int.Parse(data[5]);
            discription[i - 1] = data[7];
        }

    }

    private void Awake()
    {
        cur_id = -1;
        instance = this;
        Set_Spell_Data();
        if (stage_lv > 10)
        {
            page_list = new int[12];
        }
        else
        {
            page_list = new int[stage_lv + 2];
        }

        for(int i = 0; i < page_list.Length; i++) {
            //비어있다면 -1
            page_list[i] = -1;
        }

        

        pageSetting(page_index);
    }

    // Start is called before the first frame update
    void Start()
    {
        page_index = 0;
    }

    // Update is called once per frame
    void Update()
    {
        switch(page_index)
        {
            case 0:
                //스킬의 가지수가 5개 이하일때 넘어가기 버튼 숨김
                if (page_list.Length < 5)
                {
                    back.gameObject.SetActive(false);
                    next.gameObject.SetActive(false);
                }
                else
                {
                    back.gameObject.SetActive(false);
                    next.gameObject.SetActive(true);
                }
                
                break; 
            case 1:
                //스킬의 가지수가 9개 이하일때 넘어가기 버튼 숨김
                if (page_list.Length < 9)
                {
                    back.gameObject.SetActive(true);
                    next.gameObject.SetActive(false);
                }
                else
                {
                    back.gameObject.SetActive(true);
                    next.gameObject.SetActive(true);
                }
                
                break;
            case 2:
                back.gameObject.SetActive(true);
                next.gameObject.SetActive(false);
                break;
        }
    }

    public void nextBtn()
    {
        page_index++;
        pageSetting(page_index);
    }

    public void backBtn()
    {
        page_index--;
        pageSetting(page_index);
    }

    public void resetBtn()
    {
        for (int i = 0; i < page_list.Length; i++)
        {
            if(page_list[i] != -1)
            {
                icons[page_list[i]].Active();
                page_list[i] = -1;
            }
            
        }
        pageSetting(page_index);
    }

    public void spell_set(int spell_id, int page_id)
    {
        if (page_list[page_index * 4 + page_id] == -1)
        {
            page_list[page_index * 4 + page_id] = spell_id;
        }
        else
        {
            if(page_list[page_index * 4 + page_id] != spell_id)
            {
                icons[page_list[page_index * 4 + page_id]].Active();
            }
            
            page_list[page_index * 4 + page_id] = spell_id;
        }
    }

    private void pageSetting(int page_i)
    {
        for (int i = 0; i < 4; i++)
        {
            spellPages[i].gameObject.SetActive(true);
            if (i < (page_list.Length - (4 * page_i)))
            {
                if (page_list[i + (4 * page_i)] != -1)
                {
                    spellPages[i].GetComponent<SpellSetting>().Spellsetting(page_list[i + (4 * page_i)]);
                }
                else
                {
                    spellPages[i].GetComponent<SpellSetting>().Initialized();
                }
                

            }
            else
            {
                spellPages[i].gameObject.SetActive(false);
            }

        }
    }
}
