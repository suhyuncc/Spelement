using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;

public enum Phase
{
    StandBy = 0,
    Battle = 1,
    End = 2,
}
public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;

    private GameObject GM;

    [SerializeField]
    private TextAsset csvFile = null;
    [SerializeField]
    private TextAsset stage_info = null;
    [SerializeField]
    private TextAsset state_info = null;


    public string[] Name;
    public int[] Null;
    public int[] Air;
    public int[] Earth;
    public int[] Water;
    public int[] Fire;
    public int[] number;
    public string[] discription;
    public int[] isAttack;
    public int[] isHeal;//나중에 고치자(왜만들었지...)
    public int[] additional_act;

    public int state_id;
    public string[] State_Name;
    public string[] State_discription;

    public int[] monster_HP;
    public int[] player_HP;
    public int[] Monster_Attack;
    public int[] Monster_Spell_id;
    public int[] Monster_Spell_cool;

    public Sprite[] Upper_sprites;
    public Sprite[] Down_sprites;
    public Sprite[] icons;

    public bool player_turn;
    public bool monster_done;
    private bool Re_setting;

    public int[] page_list = { 0, 1, 2, 17, 9, 7};
    public int spell_count;
    public int page_index;
    private int page_max;

    public int stage_num;

    [SerializeField]
    private GameObject[] spellPages;
    [SerializeField]
    private Slider Player_HP;
    [SerializeField]
    private Text Player_HP_Text;
    [SerializeField]
    private GameObject Player_state;
    [SerializeField]
    private Slider Monster_HP;
    [SerializeField]
    private Text Monster_HP_Text;
    [SerializeField]
    private GameObject Monster_state;
    [SerializeField]
    private GameObject Book;
    [SerializeField]
    private Monster monster;
    [SerializeField]
    private GameObject Elements;

    [Header("운명관련")]
    public int[] F_list = new int[3];
    public int F_index;
    [SerializeField]
    private GameObject[] FortuneIcons;
    [SerializeField]
    private Sprite[] FortuneIconSprites;
    [SerializeField]
    private GameObject FortunePanel;

    [Header("페이즈")]
    public Phase phase;

    //csv파싱
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
        number = new int[row.Length - 1];
        discription = new string[row.Length - 1];
        isAttack = new int[row.Length];
        isHeal = new int[row.Length - 1];
        additional_act = new int[row.Length - 1];

        for (int i = 1; i < row.Length; i++)
        {
            string[] data = row[i].Split(new char[] { ',' }); //split by (,)

            Name[i - 1] = data[0];
            Null[i - 1] = int.Parse(data[1]);
            Air[i - 1] = int.Parse(data[2]);
            Earth[i - 1] = int.Parse(data[3]);
            Water[i - 1] = int.Parse(data[4]);
            Fire[i - 1] = int.Parse(data[5]);
            number[i - 1] = int.Parse(data[6]);
            discription[i - 1] = data[7];
            isAttack[i - 1] = int.Parse(data[8]);
            isHeal[i - 1] = int.Parse(data[9]);
            additional_act[i - 1] = int.Parse(data[10]);
        }

        isAttack[19] = 1;
    }

    public void Set_Stage_Data() {
        string csvText = stage_info.text.Substring(0, stage_info.text.Length - 1); //get csv file as string type, except last line(empty)
        string[] row = csvText.Split(new char[] { '\n' }); //split by enter sign

        monster_HP = new int[row.Length - 1];
        Monster_Attack = new int[row.Length - 1];
        player_HP = new int[row.Length - 1];
        Monster_Spell_id = new int[row.Length - 1];
        Monster_Spell_cool = new int[row.Length - 1];

        for (int i = 1; i < row.Length; i++)
        {
            string[] data = row[i].Split(new char[] { ',' }); //split by (,)

            monster_HP[i - 1] = int.Parse(data[1]);
            Monster_Attack[i - 1] = int.Parse(data[3]);
            player_HP[i - 1] = int.Parse(data[4]);
            Monster_Spell_id[i - 1] = int.Parse(data[5]);
            Monster_Spell_cool[i - 1] = int.Parse(data[6]);
        }
    }

    public void Set_State_Data()
    {
        string csvText = state_info.text.Substring(0, state_info.text.Length - 1); //get csv file as string type, except last line(empty)
        string[] row = csvText.Split(new char[] { '\n' }); //split by enter sign

        State_Name = new string[row.Length - 1];
        State_discription = new string[row.Length - 1];

        for (int i = 1; i < row.Length; i++)
        {
            string[] data = row[i].Split(new char[] { ',' }); //split by (,)

            State_Name[i - 1] = data[0];
            State_discription[i - 1] = data[1];
        }
    }

    private void Awake()
    {
        //씬 시작시 초기세팅
        instance = this;

        GM = GameObject.Find("GameManager"); //GameManager를 찾아서
        stage_num = GM.GetComponent<GameManager>().currentStageSerialNumber; //스테이지 넘버 가져오기
        

        //운명 id -1로 초기화
        F_index = 0;
        for (int i = 0; i < F_list.Length; i++)
        {
            F_list[i] = -1;
        }

        for (int i = 0; i < FortuneIcons.Length; i++)
        {
            FortuneIcons[i].SetActive(false);
        }

        page_index = 0;
        page_max = page_list.Length / 4;
        phase = Phase.StandBy;
        player_turn = true;
        Re_setting = false;

        Set_Spell_Data();
        Set_Stage_Data();
        Set_State_Data();

        //0있는 부분은 전부 스테이지 레벨로 교체
        Player_HP_Text.text = $"{player_HP[0]} / {player_HP[0]}";
        Monster_HP_Text.text = $"{monster_HP[0]} / {monster_HP[0]}";

        monster.spell_id = Monster_Spell_id[0];
        monster.spell_cool = Monster_Spell_cool[0];
        monster.nomal_demage = Monster_Attack[0];

        Book.SetActive(true);


        pageSetting(page_index);

    }

    private void Update()
    {
        if(spell_count== 0)
        {
            Re_setting = true;
        }

        switch (phase)
        {
            case Phase.StandBy:
                if (player_turn)
                {
                    if ((F_list[0] == -1 && Player_HP.value > 0.9f)
                        || (F_list[1] == -1 && Player_HP.value < 0.5f)
                        || (F_list[2] == -1 && Player_HP.value < 0.3f))
                    {
                        Book.SetActive(false);
                        FortunePanel.SetActive(true);
                    }
                    else
                    {
                        Elements.SetActive(true);
                        Book.gameObject.SetActive(true);
                        EleManager.instance.Reroll();
                        Player_state.GetComponent<StateManagement>().reduceState();
                        phase = Phase.Battle;
                    }
                    
                    
                    
                }
                else
                {
                    Monster_state.GetComponent<StateManagement>().reduceState();
                    phase = Phase.Battle;

                }
                break;

            case Phase.Battle:
                if (player_turn){
                    //스펠 다쓰면 페이지 전환
                    //엔드는 "턴 종료" 버튼으로
                    if(Re_setting)
                    {
                        if(page_index == page_max)
                        {
                            page_index = -1;
                        }
                        pageSetting(++page_index);
                    }
                }
                else {
                    monster.monster_active();
                    phase = Phase.End;
                    
                }
                break;

            case Phase.End:
                if (player_turn){
                    Elements.SetActive(false);
                    player_turn = !player_turn;
                    phase = Phase.StandBy;
                }
                else {
                    if (monster_done)
                    {
                        player_turn = !player_turn;
                        monster_done = false;
                        phase = Phase.StandBy;
                        
                    }
                }
                
                break;
        }
    }

    public void showIcon(int index)
    {
        FortuneIcons[index].GetComponent<Image>().sprite = FortuneIconSprites[F_list[index] / 6];
        FortuneIcons[index].SetActive(true);
        F_index++;
    }

    public void turn_end()
    {
        phase = Phase.End;
    }

    private void pageSetting(int page_i)
    {
        for (int i = 0; i < 4; i++)
        {
            if (i < (page_list.Length - (4 * page_i))) {
                spellPages[i].GetComponent<SpellAction>().spell_id = page_list[i + (4 * page_i)];
                spellPages[i].GetComponent<SpellAction>().spellSetting();
                spell_count++;
                
            }
            else
            {
                spellPages[i].gameObject.SetActive(false);
            }
            
        }

        Re_setting = false;
    }

    IEnumerator monster_term()
    {
        yield return new WaitForSeconds(0.7f);
        phase = Phase.Battle;
    }

    public void Rebutton()
    {
        Book.SetActive(false);
        page_index = 0;
        pageSetting(page_index);
        Book.SetActive(true);
    }
}
