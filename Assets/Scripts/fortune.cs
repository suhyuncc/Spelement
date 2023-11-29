using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class fortune : MonoBehaviour
{
    public static fortune instance;

    [SerializeField]
    private TextAsset fortune_info = null;
    [SerializeField]
    private Sprite[] fortune_sprites;
    [SerializeField]
    private Image[] images;
    [SerializeField]
    private Text[] names;
    [SerializeField]
    private Text[] discriptions;

    public string[] Name;
    public string[] discription;

    public int[] fortune_list = new int[3];
    public int Show_F_index;

    private void OnEnable()
    {
        set_Fortune();
    }

    private void OnDisable()
    {

    }

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        Set_Fortune_Data();
    }

    public void Set_Fortune_Data()
    {
        string csvText = fortune_info.text.Substring(0, fortune_info.text.Length - 1); //get csv file as string type, except last line(empty)
        string[] row = csvText.Split(new char[] { '\n' }); //split by enter sign

        Name = new string[row.Length - 1];
        discription = new string[row.Length - 1];

        for (int i = 1; i < row.Length; i++)
        {
            string[] data = row[i].Split(new char[] { ',' }); //split by (,)

            Name[i - 1] = data[3];
            discription[i - 1] = data[4];
           
        }

    }

    private void set_Fortune()
    {
        int ran1 = Random.Range(0, 18);
        int ran2 = Random.Range(0, 18);
        int ran3 = Random.Range(0, 18);

        if(ran1 != ran2 &&  ran3 != ran1 && ran2 != ran3) {

            //가지고 있는 운명이 있는지 중복체크
            for (int i = 0; i < BattleManager.instance.F_list.Length; i++)
            {
                if (BattleManager.instance.F_list[i] == ran1
                    || BattleManager.instance.F_list[i] == ran2
                    || BattleManager.instance.F_list[i] == ran3)
                {
                    set_Fortune();
                    return;
                }
            }

            images[0].sprite = fortune_sprites[ran1 / 6];
            images[1].sprite = fortune_sprites[ran2 / 6];
            images[2].sprite = fortune_sprites[ran3 / 6];

            names[0].text = Name[ran1];
            names[1].text = Name[ran2];
            names[2].text = Name[ran3];

            discriptions[0].text = discription[ran1];
            discriptions[1].text = discription[ran2];
            discriptions[2].text = discription[ran3];

            fortune_list[0] = ran1;
            fortune_list[1] = ran2;
            fortune_list[2] = ran3;
        }
        else
        {
            set_Fortune();
            return;
        }
    }

    public void buttonOne() {
        BattleManager.instance.F_list[BattleManager.instance.F_index] = fortune_list[0];
        SkillManager.instance.fortune_Action(fortune_list[0]);

        for (int i = 0; i < fortune_list.Length; i++)
        {
            fortune_list[i] = -1;
        }
        BattleManager.instance.showIcon(BattleManager.instance.F_index);
        
        this.gameObject.SetActive(false);
    }

    public void buttonTwo()
    {
        BattleManager.instance.F_list[BattleManager.instance.F_index] = fortune_list[1];
        SkillManager.instance.fortune_Action(fortune_list[1]);

        for (int i = 0; i < fortune_list.Length; i++)
        {
            fortune_list[i] = -1;
        }
        BattleManager.instance.showIcon(BattleManager.instance.F_index);
        this.gameObject.SetActive(false);
    }

    public void buttonThree()
    {
        BattleManager.instance.F_list[BattleManager.instance.F_index] = fortune_list[2];
        SkillManager.instance.fortune_Action(fortune_list[2]);

        for (int i = 0; i < fortune_list.Length; i++)
        {
            fortune_list[i] = -1;
        }
        BattleManager.instance.showIcon(BattleManager.instance.F_index);
        this.gameObject.SetActive(false);
    }
}
