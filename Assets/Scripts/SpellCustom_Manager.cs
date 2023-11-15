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
                back.gameObject.SetActive(false);
                next.gameObject.SetActive(true);
                break; 
            case 1:
                back.gameObject.SetActive(true);
                next.gameObject.SetActive(true);
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
    }

    public void backBtn()
    {
        page_index--;
    }

    public void spell_set(int spell_id, int page_id)
    {
        if (page_list[page_index * 4 + page_id] == -1)
        {
            page_list[page_index * 4 + page_id] = spell_id;
        }
        else
        {
            icons[page_list[page_index * 4 + page_id]].Active();
            page_list[page_index * 4 + page_id] = spell_id;
        }
    }
}
