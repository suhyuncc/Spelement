using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public enum Phase
{
    StandBy = 0,
    Battle = 1,
    End = 2,
}
public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;

    public GameObject GM;

    [Header("�������� ����")]
    public int stage_num;
    [SerializeField]
    private GameObject Backgorund;
    [SerializeField]
    private Sprite[] Backgorund_sprites;

    [Header("CSV����")]
    [SerializeField]
    private TextAsset csvFile = null;
    [SerializeField]
    private TextAsset stage_info = null;
    [SerializeField]
    private TextAsset state_info = null;

    [Header("Spell_Info")]
    public string[] Name;
    public int[] Null;
    public int[] Air;
    public int[] Earth;
    public int[] Water;
    public int[] Fire;
    public int[] number;
    public string[] discription;
    public int[] isAttack;
    public int[] isHeal;
    public int[] additional_act;

    [Header("State_Info")]
    public int state_id;
    public string[] State_Name;
    public string[] State_discription;

    [Header("Stage_Info")]
    public int[] monster_HP;
    public int[] player_HP;
    public int[] Monster_Attack;
    public int[] Monster_Spell_id;
    public int[] Monster_Spell_cool;

    [Header("����� ����")]
    public Sprite[] Upper_sprites;
    public Sprite[] Down_sprites;
    public Sprite[] icons;
    [SerializeField]
    private GameObject[] spellPages;
    [SerializeField]
    private GameObject[] change_element_btns;
    [SerializeField]
    private GameObject return_btn;

    [Header("���� ����")]
    public bool player_turn;
    public bool monster_done;
    private bool Re_setting;

    public int[] page_list = { 0, 1, 2, 17, 9, 7};
    public int spell_count;
    public int page_index;
    private int page_max;

    [SerializeField]
    private GameObject Book;
    [SerializeField]
    private GameObject Elements;
    [SerializeField]
    private AudioClip[] BGM;

    [Header("�÷��̾� ����")]
    [SerializeField]
    private Slider Player_HP;
    [SerializeField]
    private Text Player_HP_Text;
    [SerializeField]
    private GameObject Player_state;

    [Header("�� ����")]
    [SerializeField]
    private Slider Monster_HP;
    [SerializeField]
    private Text Monster_HP_Text;
    [SerializeField]
    private GameObject Monster_state;
    [SerializeField]
    private Monster monster;
    

    [Header("������")]
    public int[] F_list = new int[3];
    public int F_index;
    [SerializeField]
    private GameObject[] FortuneIcons;
    [SerializeField]
    private Sprite[] FortuneIconSprites;
    [SerializeField]
    private GameObject FortunePanel;

    [Header("������")]
    public Phase phase;

    //csv�Ľ�
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
        //�� ���۽� �ʱ⼼��
        instance = this;

        GM = GameObject.Find("GameManager"); //GameManager�� ã�Ƽ�
        stage_num = GM.GetComponent<GameManager>().currentStageSerialNumber; //�������� �ѹ� ��������
        page_list = GM.GetComponent<GameManager>().spell_list;

        stage_num -= 1;

        switch (stage_num)
        {
            case 0:
                EleManager.instance.player_lv = 1;
                break;

            default:

                if(stage_num == 12)
                {
                    EleManager.instance.player_lv = 5;
                }
                else
                {
                    EleManager.instance.player_lv = ((stage_num - 1) / 3) + 2;
                }
                

                if(stage_num > 1)
                {
                    return_btn.SetActive(true);
                }
                break;
        }

        //��� ����
        this.gameObject.GetComponent<AudioSource>().clip = BGM[stage_num / 3];
        this.gameObject.GetComponent<AudioSource>().Play();


        //��� id -1�� �ʱ�ȭ
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

        //���������� ���� ��� ��ȯ
        Backgorund.GetComponent<SpriteRenderer>().sprite = Backgorund_sprites[stage_num / 3];

        //���������� ���� ��
        for(int i = 0; i < (stage_num / 3); i++)
        {
            change_element_btns[i].SetActive(true);
        }

        //0�ִ� �κ��� ���� �������� ������ ��ü
        Player_HP_Text.text = $"{player_HP[stage_num]} / {player_HP[stage_num]}";
        Monster_HP_Text.text = $"{monster_HP[stage_num]} / {monster_HP[stage_num]}";

        monster.spell_id = Monster_Spell_id[stage_num];
        monster.spell_cool = Monster_Spell_cool[stage_num];
        monster.nomal_demage = Monster_Attack[stage_num];


        //�����϶�
        if((stage_num % 3) == 2)
        {
            monster.is_Boss = true;
        }
        else if(stage_num == 12)
        {
            monster.is_King = true;
        }
        else
        {
            monster.is_Boss = false;
        }

        monster.monster_Setting(stage_num);

        Book.SetActive(true);


        pageSetting(page_index);

        Book.SetActive(false);


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
                        Check_fortune();
                        Player_state.GetComponent<StateManagement>().reduceState();
                        Elements.SetActive(true);
                        Book.gameObject.SetActive(true);
                        EleManager.instance.Reroll();

                        if(phase == Phase.StandBy)
                        {
                            phase = Phase.Battle;
                        }
                        
                    }
                }
                else
                {
                    Monster_state.GetComponent<StateManagement>().reduceState();

                    if (phase == Phase.StandBy)
                    {
                        monster.monster_active();
                        phase = Phase.Battle;
                    }

                }
                break;

            case Phase.Battle:
                if (player_turn){
                    //���� �پ��� ������ ��ȯ
                    //����� "�� ����" ��ư����
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

                    
                }

                break;

            case Phase.End:
                if (player_turn){
                    Elements.SetActive(false);
                    player_turn = !player_turn;
                    SkillManager.instance.check_HP();

                }
                else {
                    if (monster_done)
                    {
                        player_turn = !player_turn;
                        monster_done = false;
                        SkillManager.instance.check_HP();
                        
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

                //�����Ҷ� spell_count�� 4�̻��� �Ǵ°� ����
                if (spell_count < 4)
                {
                    spell_count++;
                }
            }
            else
            {
                spellPages[i].gameObject.SetActive(false);
            }
            
        }
        
        Re_setting = false;
    }

    private void Check_fortune()
    {
        //��� ���
        for (int i = 0; i < F_list.Length; i++)
        {
            SkillManager.instance.fortune_Heal(F_list[i]);
        }
    }

    public void GoBackToIdleScene()
    {
        if(Monster_HP.value < 0.00001f)
        {
            GM.GetComponent<GameManager>().currentStageCleared = true;
            GM.GetComponent<GameManager>().Addspell(stage_num);
        }
        else
        {
            GM.GetComponent<GameManager>().currentStageCleared = false;
        }

        if (GM != null)
        {
            GM.GetComponent<GameManager>().IdleSceneChange();
        }
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
