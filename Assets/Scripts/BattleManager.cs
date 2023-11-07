using System.Collections;
using System.Collections.Generic;
using System.Threading;
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

    [SerializeField]
    private TextAsset csvFile = null;
    [SerializeField]
    private TextAsset stage_info = null;

    public string[] Name;
    public int[] Null;
    public int[] Air;
    public int[] Earth;
    public int[] Water;
    public int[] Fire;
    public int[] number;
    public string[] discription;
    public int[] isAttack;

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

    public int[] page_list = { 0, 1, 2, 17, 9};
    public int spell_count;
    private int page_index;
    private int page_max;

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

    private void Awake()
    {
        //씬 시작시 초기세팅
        instance = this;

        page_index = 0;
        page_max = page_list.Length / 4;
        phase = Phase.StandBy;
        player_turn = true;
        Re_setting = false;

        Set_Spell_Data();
        Set_Stage_Data();

        Player_HP_Text.text = $"{player_HP[0]} / {player_HP[0]}";
        Monster_HP_Text.text = $"{monster_HP[0]} / {monster_HP[0]}";

        monster.spell_id = Monster_Spell_id[0];
        monster.spell_cool = Monster_Spell_cool[0];
        monster.nomal_demage = Monster_Attack[0];


        pageSetting(page_index);
        /*spellPages[0].GetComponent<SpellAction>().spell_id = 3;
        spellPages[1].GetComponent<SpellAction>().spell_id = 7;
        spellPages[2].GetComponent<SpellAction>().spell_id = 17;
        spellPages[3].GetComponent<SpellAction>().spell_id = 18;

        for(int i = 0; i < spellPages.Length; i++)
        {
            spellPages[i].GetComponent<SpellAction>().spellSetting();
        }*/
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
                StopCoroutine("endTostandby");
                if (player_turn) {
                    Player_state.GetComponent<StateManagement>().reduceState();
                    Book.gameObject.SetActive(true);
                    EleManager.instance.Reroll();
                    phase = Phase.Battle;
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
                
                //StartCoroutine("endTostandby");
                
                break;
        }
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

    IEnumerator endTostandby()
    {
        yield return new WaitForSeconds(0.7f);
        phase = Phase.StandBy;
    }

}
