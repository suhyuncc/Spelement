using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;

    [SerializeField]
    private Camera camera;

    [Header("플레이어 관련")]
    [SerializeField]
    private GameObject Player;
    [SerializeField]
    private Slider Player_HP;
    [SerializeField]
    private Text Player_HP_Text;
    [SerializeField]
    private GameObject Player_state;
    [SerializeField]
    private GameObject Player_apos;

    [Header("적 관련")]
    [SerializeField]
    private GameObject Monster;
    [SerializeField]
    private Slider Monster_HP;
    [SerializeField]
    private Text Monster_HP_Text;
    [SerializeField]
    private GameObject Monster_state;
    [SerializeField]
    private GameObject Monster_apos;
    [SerializeField]
    private GameObject[] Monster_noaml_apos;

    [Header("스킬 효과")]
    [SerializeField]
    private GameObject[] skill_Effects;
    [SerializeField]
    private GameObject[] P_hit_Effects;
    [SerializeField]
    private GameObject[] M_hit_Effects;

    [Header("효과음")]
    public AudioSource sfx_Manager;
    [SerializeField]
    private AudioClip[] UI_SFX;
    [SerializeField]
    private AudioClip[] hit_SFX;
    [SerializeField]
    private AudioClip[] attack_SFX;

    [Header("전투 관련")]
    [SerializeField]
    private GameObject Book;
    [SerializeField]
    private Text[] player_Damage_Text;
    [SerializeField]
    private Text[] monster_Damage_Text;

    private Color[] colors = {new Color(0.75f,0.75f,0.75f), new Color(0.75f, 0.75f, 1f),
        new Color(1, 0.75f, 0.75f), new Color(1f, 0.875f, 0.75f), new Color(0.875f, 1f, 0.75f) };

    

    public bool isActive;
    public bool isAttack;
    public bool player_turn;
    public int spell_id;
    public int nomal_Dem;

    public int player_max_hp;
    public int monster_max_hp;
    [SerializeField]
    private int player_current_hp;
    [SerializeField]
    private int monster_current_hp;
    [SerializeField]
    private GameObject spin_spell_field;
    [SerializeField]
    private GameObject winlose_panel;

    private int count;
    private int _percent;

    public int[] F_counts = new int[18];

    private Coroutine coroutine;

    private bool fortune_Add;
    private bool fortune_First;
    private bool fortune_Second;

    private bool M_Haste;
    private bool P_Haste;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        sfx_Manager = this.GetComponent<AudioSource>();
        _percent = 0;

        for(int i = 0; i < F_counts.Length; i++)
        {
            F_counts[i] = 0;
        }
        fortune_Add = true;
        fortune_First = false;
        fortune_Second = false;
    }

    private void Start()
    {
        //플레이어, 몬스터 체력 받아오기
        player_max_hp = BattleManager.instance.player_HP[BattleManager.instance.stage_num];
        monster_max_hp = BattleManager.instance.monster_HP[BattleManager.instance.stage_num];
        player_current_hp = player_max_hp;
        monster_current_hp = monster_max_hp;
    }

    // Update is called once per frame
    void Update()
    {
        if(isActive)
        {
            coroutine = StartCoroutine(Skill_Active(player_turn, spell_id));
            isActive = false;
        }

        
    }

    IEnumerator Skill_Active(bool player_turn, int spell_id)
    {
        Book.SetActive(false);
        //공격 기술일때
        if (BattleManager.instance.isAttack[spell_id] == 1)
        {
            //투사체 날리기
            if (player_turn)
            {
                //플레이어가 몬스터에게
                if(spell_id != 13)
                {
                    skill_Effects[spell_id].transform.position = Player_apos.transform.position;
                }
                skill_Effects[spell_id].GetComponent<Attack>().direct = 1;
                spell_field(spell_id);
                skill_Effects[spell_id].SetActive(true);
            }
            else
            {
                //몬스터가 플레이어에게
                if (spell_id == 13)
                {
                    skill_Effects[spell_id].transform.position = Player_apos.transform.position;
                }
                else if (spell_id == 19)
                {
                    skill_Effects[spell_id].transform.position 
                        = Monster_noaml_apos[BattleManager.instance.stage_num].transform.position;
                }
                else
                {
                    skill_Effects[spell_id].transform.position = Monster_apos.transform.position;
                }
                skill_Effects[spell_id].GetComponent<Attack>().direct = -1;
                skill_Effects[spell_id].SetActive(true);
                monster_spell_field(spell_id);
            }
            

            yield return new WaitForSeconds(0.5f);

            //피격
            if (player_turn)
            {
                //몬스터에게
                M_hit_Effects[spell_id].SetActive(true);

                //어스퀘이크 카메라 흔들기
                if(spell_id == 14)
                {
                    StartCoroutine("camera_shake");
                    
                }

                //피격당하는 원소의 색상으로 점멸 효과
                if (spell_id - 3 < 0)
                {
                    Monster.GetComponent<SpriteRenderer>().color = colors[0];
                }
                else
                {
                    Monster.GetComponent<SpriteRenderer>().color = colors[((spell_id - 3) / 4) + 1];
                }
                SFX_On(1, spell_id);
            }
            else
            {
                //플레이어에게
                P_hit_Effects[spell_id].SetActive(true);

                //어스퀘이크 카메라 흔들기
                if (spell_id == 14)
                {
                    StartCoroutine("camera_shake");

                }

                //피격당하는 원소의 색상으로 점멸 효과
                if (spell_id - 3 < 0 || spell_id == 19)
                {
                    Player.GetComponent<SpriteRenderer>().color = colors[0];
                }
                else
                {
                    Player.GetComponent<SpriteRenderer>().color = colors[((spell_id - 3) / 4) + 1];
                }
                SFX_On(1,spell_id);
            }
        }
        //공격 기술이 아닐때
        else
        {
            //회복 및 방어막
            if (player_turn)
            {
                P_hit_Effects[spell_id].SetActive(true);
                SFX_On(1, spell_id);
            }
            else
            {
                M_hit_Effects[spell_id].SetActive(true);
                SFX_On(1, spell_id);
            }
        }

        //데미지 계산
        if (player_turn) //플레이어 턴
        {

            if (BattleManager.instance.isAttack[spell_id] == 1)
            {
                //적 체력 감소
                monster_Hit(spell_id, Player_Atk_damage_calcul(BattleManager.instance.number[spell_id]));

                //물방울이 존재 할 때
                if (Monster_state.GetComponent<StateManagement>().counts[3] != 0)
                {
                    player_Heal(4, 3);
                    Monster_state.GetComponent<StateManagement>().counts[3]--;
                }

                //난기류 스택이 존재 할 때
                if (Player_state.GetComponent<StateManagement>().counts[8] != 0)
                {
                    monster_Hit(16, 5);
                    Player_state.GetComponent<StateManagement>().counts[8]--;
                }

                //파이어 리플랙션 스택이 존재 할 때
                if (Monster_state.GetComponent<StateManagement>().counts[5] != 0)
                {
                    int dam = (int)((float)BattleManager.instance.number[spell_id] * 0.75f);
                    player_Hit(7, dam);
                    M_hit_Effects[8].SetActive(true);
                    Monster_state.GetComponent<StateManagement>().counts[5]--;
                }
            }
            else
            {
                //플레이어 체력 회복
                player_Heal(spell_id, BattleManager.instance.number[spell_id]);
            }

        }
        else //적 턴
        {

            if (BattleManager.instance.isAttack[spell_id] == 1)
            {
                if (spell_id == 19)
                {
                    player_Hit(spell_id, nomal_Dem);

                    //파이어 리플랙션 스택이 존재 할 때
                    if (Player_state.GetComponent<StateManagement>().counts[5] != 0)
                    {
                        int dam = (int)((float)nomal_Dem * 0.75f);
                        monster_Hit(7, dam);
                        P_hit_Effects[8].SetActive(true);
                        
                        Player_state.GetComponent<StateManagement>().counts[5]--;
                    }
                }
                else
                {
                    player_Hit(spell_id, BattleManager.instance.number[spell_id]);

                    //파이어 리플랙션 스택이 존재 할 때
                    if (Player_state.GetComponent<StateManagement>().counts[5] != 0)
                    {
                        int dam = (int)((float)BattleManager.instance.number[spell_id] * 0.75f);
                        P_hit_Effects[8].SetActive(true);
                        monster_Hit(7, dam);
                        Player_state.GetComponent<StateManagement>().counts[5]--;
                    }
                }

                //물방울이 존재 할 때
                if (Player_state.GetComponent<StateManagement>().counts[3] != 0)
                {
                    monster_Heal(4, 3);
                    Player_state.GetComponent<StateManagement>().counts[3]--;
                }

                //난기류 스택이 존재 할 때
                if (Monster_state.GetComponent<StateManagement>().counts[8] != 0)
                {
                    player_Hit(16, 5);
                    Monster_state.GetComponent<StateManagement>().counts[8]--;
                }
            }
            else
            {
                //몬스터 체력 회복
                monster_Heal(spell_id, BattleManager.instance.number[spell_id]);
            }


        }

        yield return new WaitForSeconds(0.5f);

        spin_spell_field.SetActive(false);

        if (!player_turn)
        {
            //HP에 따른 운명 발동
            Heal_fortune();
        }
        

        //추가 행동
        Additional(spell_id);

        if (player_turn)
        {
            //운명으로 인한 추가 발동
            if (fortune_Add)
            {
                fortune_Additional();
                fortune_Add = false;
            }

            if (fortune_First || fortune_Second)
            {
                if (fortune_Second)
                {
                    StopCoroutine(coroutine);
                    isActive = true;
                    Debug.Log("10% 발동!!");
                    fortune_Second = false;
                }
                else if (fortune_First)
                {
                    StopCoroutine(coroutine);
                    isActive = true;
                    Debug.Log("30% 발동!!");
                    fortune_First = false;
                }
            }
            else
            {
                fortune_Add = true;
            }

            if(monster_current_hp == 0)
            {
                BattleManager.instance.phase = Phase.End;
            }
        }


        yield return new WaitForSeconds(0.2f);

        //배틀 페이즈 종료
        if (player_turn)
        {
            Monster.GetComponent<SpriteRenderer>().color = new Color(1,1,1);
            Book.SetActive(true);
        }
        else
        {
            yield return new WaitForSeconds(0.3f);
            Player.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
            BattleManager.instance.phase = Phase.End;
            BattleManager.instance.monster_done = true;
        }

        StopCoroutine(coroutine);
        
    }

    IEnumerator mini_fire(bool player_turn, int spell_id)
    {
        Book.SetActive(false);
        if (player_turn)
        {
            //효과
            P_hit_Effects[spell_id].SetActive(true);
            //점멸
            Player.GetComponent<SpriteRenderer>().color = colors[((spell_id - 3) / 4) + 1];

            player_Damage_txt(spell_id,3);

            SFX_On(1, spell_id);

            player_current_hp -= 3;
            //오버 히트 방지
            if (player_current_hp < 0)
            {
                player_current_hp = 0;
            }
            yield return new WaitForSeconds(0.3f);

            //데미지 계산 반영
            Player_HP.value = (float)player_current_hp / (float)player_max_hp;
            Player_HP_Text.text = $"{player_current_hp} / {player_max_hp}";

            //색 복구
            Player.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);

            //HP에 따른 운명 발동
            Heal_fortune();
            //적의 HP가 0이면 전투 종료
            check_HP();
            yield return new WaitForSeconds(0.3f);
            Book.SetActive(true);
            
        }
        else
        {
            //효과
            M_hit_Effects[spell_id].SetActive(true);
            //점멸
            Monster.GetComponent<SpriteRenderer>().color = colors[((spell_id - 3) / 4) + 1];

            monster_Damage_txt(spell_id,3);

            SFX_On(1, spell_id);

            monster_current_hp -= 3;
            //오버 히트 방지
            if (monster_current_hp < 0)
            {
                monster_current_hp = 0;
            }
            yield return new WaitForSeconds(0.3f);

            //데미지 계산 반영
            Monster_HP.value = (float)monster_current_hp / (float)monster_max_hp;
            Monster_HP_Text.text = $"{monster_current_hp} / {monster_max_hp}";

            //색 복구
            Monster.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);

            yield return new WaitForSeconds(0.3f);

            //적의 HP가 0이면 전투 종료
            check_HP();

        }
        
    }

    IEnumerator spell_16()
    {
        yield return new WaitForSeconds(0.2f);
        StopCoroutine(spell_16());
    }

    IEnumerator Sturn()
    {
        if (BattleManager.instance.player_turn)
        {
            //기절 텍스트 띄우기
            for (int i = 0; i < player_Damage_Text.Length; i++)
            {
                if (!player_Damage_Text[i].gameObject.activeSelf)
                {
                    player_Damage_Text[i].text = "기절!";
                    player_Damage_Text[i].gameObject.SetActive(true);
                    break;
                }
            }
        }
        else
        {
            //기절 텍스트 띄우기
            for (int i = 0; i < monster_Damage_Text.Length; i++)
            {
                if (!monster_Damage_Text[i].gameObject.activeSelf)
                {
                    monster_Damage_Text[i].text = "기절!";
                    monster_Damage_Text[i].gameObject.SetActive(true);
                    break;
                }
            }
            
        }
        BattleManager.instance.phase = Phase.End;

        yield return new WaitForSeconds(0.7f);

        if (!BattleManager.instance.player_turn)
        {
            BattleManager.instance.monster_done = true;
        }


        StopCoroutine("Sturn");
    }

    public void SFX_On(int subject_id, int index) 
    {
        //subject_id 0은 UI_SFX, 1은 hit_SFX
        switch (subject_id)
        {
            case 0:
                sfx_Manager.clip = UI_SFX[index];
                sfx_Manager.Play();
                break; 
            case 1:
                sfx_Manager.clip = hit_SFX[index];
                sfx_Manager.Play();
                break;
            case 2:
                sfx_Manager.clip = attack_SFX[index];
                sfx_Manager.Play();
                break;
            default: 
                break;
        }
    }
    private void Additional(int spell_id)
    {
        switch(spell_id)
        {
            case 1:
                if (player_turn)
                {
                    Player_state.GetComponent<StateManagement>().counts[4] += 5;
                    Player_state.transform.GetChild(4).gameObject.SetActive(true);
                    
                }
                else
                {
                    Monster_state.GetComponent<StateManagement>().counts[4] += 5;
                    Monster_state.transform.GetChild(4).gameObject.SetActive(true);
                }
                break;

            case 3:
                if (player_turn)
                {
                    Player_state.GetComponent<StateManagement>().counts[2] = 3;
                    Player_state.transform.GetChild(2).gameObject.SetActive(true);

                }
                else
                {
                    Monster_state.GetComponent<StateManagement>().counts[2] = 3;
                    Monster_state.transform.GetChild(2).gameObject.SetActive(true);
                }
                break;

            case 4:
                if (player_turn)
                {
                    Monster_state.GetComponent<StateManagement>().counts[3] += 1;
                    Monster_state.transform.GetChild(3).gameObject.SetActive(true);
                }
                else
                {
                    Player_state.GetComponent<StateManagement>().counts[3] += 1;
                    Player_state.transform.GetChild(3).gameObject.SetActive(true);
                }
                break;

            case 5:
                if (player_turn)
                {
                    Monster_state.GetComponent<StateManagement>().counts[3] += 1;
                    Monster_state.transform.GetChild(3).gameObject.SetActive(true);
                }
                else
                {
                    Player_state.GetComponent<StateManagement>().counts[3] += 1;
                    Player_state.transform.GetChild(3).gameObject.SetActive(true);
                }
                break;

            case 6:
                if (player_turn)
                {
                    Monster_state.GetComponent<StateManagement>().counts[3] += 2;
                    Monster_state.transform.GetChild(3).gameObject.SetActive(true);

                    //30% 회복
                    player_current_hp += (int)(player_max_hp * 0.3);
                    //오버 회복 방지
                    if (player_current_hp > player_max_hp)
                    {
                        player_current_hp = player_max_hp;
                    }
                }
                else
                {
                    Player_state.GetComponent<StateManagement>().counts[3] += 2;
                    Player_state.transform.GetChild(3).gameObject.SetActive(true);

                    //30% 회복
                    monster_current_hp += (int)(monster_max_hp * 0.3);
                    //오버 회복 방지
                    if (monster_current_hp > monster_max_hp)
                    {
                        monster_current_hp = monster_max_hp;
                    }
                }
                break;

            case 7:
                if (player_turn)
                {
                    Monster_state.GetComponent<StateManagement>().counts[0] += 3;
                    Monster_state.transform.GetChild(0).gameObject.SetActive(true);
                }
                else
                {
                    Player_state.GetComponent<StateManagement>().counts[0] += 3;
                    Player_state.transform.GetChild(0).gameObject.SetActive(true);
                }
                break;

            case 8:
                if (player_turn)
                {
                    Player_state.GetComponent<StateManagement>().counts[5] = 3;
                    Player_state.transform.GetChild(5).gameObject.SetActive(true);
                    
                }
                else
                {
                    Monster_state.GetComponent<StateManagement>().counts[5] = 3;
                    Monster_state.transform.GetChild(5).gameObject.SetActive(true);
                }
                break;

            case 9:
                if (player_turn)
                {
                    Monster_state.GetComponent<StateManagement>().counts[0] += 9;
                    Monster_state.transform.GetChild(0).gameObject.SetActive(true);
                }
                else
                {
                    Player_state.GetComponent<StateManagement>().counts[0] += 9;
                    Player_state.transform.GetChild(0).gameObject.SetActive(true);
                }
                break;

            case 10:
                if (player_turn)
                {
                    if (Monster_state.GetComponent<StateManagement>().counts[0] != 0)
                    {
                        monster_Hit(10, 3 * Monster_state.GetComponent<StateManagement>().counts[0]);
                        Monster_state.GetComponent<StateManagement>().counts[0] = 0;
                    }
                }
                else
                {
                    if (Player_state.GetComponent<StateManagement>().counts[0] != 0)
                    {
                        player_Hit(10, 3 * Player_state.GetComponent<StateManagement>().counts[0]);
                        Player_state.GetComponent<StateManagement>().counts[0] = 0;
                    }
                }
                break;


            case 11:
                if (player_turn)
                {
                    Player_state.GetComponent<StateManagement>().counts[7] = 3;
                    Player_state.transform.GetChild(7).gameObject.SetActive(true);
                    
                }
                else
                {
                    Monster_state.GetComponent<StateManagement>().counts[7] = 3;
                    Monster_state.transform.GetChild(7).gameObject.SetActive(true);
                }
                break;

            //어스 스트라이크(추가예정)
            case 12:
                break;

            case 13:
                if (player_turn)
                {
                    Monster_state.GetComponent<StateManagement>().counts[1] = 1;
                    Monster_state.transform.GetChild(1).gameObject.SetActive(true);
                }
                else
                {
                    Player_state.GetComponent<StateManagement>().counts[1] = 1;
                    Player_state.transform.GetChild(1).gameObject.SetActive(true);
                }
                break;

            //어스퀘이크(추가예정)
            case 14:
                break;

            case 15:
                if (player_turn)
                {
                    Player_state.GetComponent<StateManagement>().counts[6] = 3;
                    Player_state.transform.GetChild(6).gameObject.SetActive(true);
                    
                }
                else
                {
                    Monster_state.GetComponent<StateManagement>().counts[6] = 3;
                    Monster_state.transform.GetChild(6).gameObject.SetActive(true);
                }
                break;

            case 16:
                if (player_turn)
                {
                    Player_state.GetComponent<StateManagement>().counts[8] = 3;
                    Player_state.transform.GetChild(8).gameObject.SetActive(true);

                }
                else
                {
                    Monster_state.GetComponent<StateManagement>().counts[8] = 3;
                    Monster_state.transform.GetChild(8).gameObject.SetActive(true);
                }
                break;

            //바람칼날
            case 17:
                int ran = Random.Range(1, 11);
                if(ran > _percent && count < 4)
                {
                    count++;
                    _percent += 2;
                    StopCoroutine(coroutine);
                    isActive = true;
                }
                else
                {
                    count = 0;
                    _percent = 0;
                }
                break;
            
            //용오름
            case 18:
                if (count < 4)
                {
                    count++;
                    StopCoroutine(coroutine);
                    isActive = true;
                }
                else
                {
                    count = 0;
                    if (player_turn)
                    {
                        M_hit_Effects[spell_id].SetActive(false);
                        
                    }
                    else
                    {
                        P_hit_Effects[spell_id].SetActive(false);
                    }
                }
                break;

            default: 
                break;
        }
    }

    public void StateAct(int state_index)
    {
        switch(state_index)
        {
            case 0:
                StartCoroutine(mini_fire(BattleManager.instance.player_turn,9));
                
                break;
            case 1:
                StartCoroutine("Sturn");
                break;
            case 2:
                if (BattleManager.instance.player_turn)
                {
                    P_hit_Effects[3].SetActive(true);
                    player_Heal(2, 7);
                    SFX_On(1, 3);
                }
                else
                {
                    M_hit_Effects[3].SetActive(true);
                    monster_Heal(2, 7);
                    SFX_On(1, 3);
                }
                break;
            default: 
                break;
        }
    }

    private void spell_field(int spell_id)
    {
        switch(spell_id)
        {
            case 0:
                spin_spell_field.GetComponent<SpriteRenderer>().color = colors[0];
                SFX_On(2, 0);
                spin_spell_field.SetActive(true);
                break;
            case 7:
                spin_spell_field.GetComponent<SpriteRenderer>().color = colors[2];
                SFX_On(2, 1);
                spin_spell_field.SetActive(true);
                break;
            case 13:
                spin_spell_field.GetComponent<SpriteRenderer>().color = colors[3];
                SFX_On(2, 2);
                spin_spell_field.SetActive(true);
                break;
            case 17:
                spin_spell_field.GetComponent<SpriteRenderer>().color = colors[4];
                SFX_On(2, 3);
                spin_spell_field.SetActive(true);
                break;
            default:
                break;
        }
    }

    private void monster_spell_field(int spell_id)
    {
        switch (spell_id)
        {
            case 0:
                break;
            case 7:
                break;
            case 13:
                break;
            case 17:
                break;
            case 19:
                break;
            default:
                skill_Effects[spell_id].SetActive(false);
                break;
        }
    }

    private void player_Heal(int spell_id, int heal_point)
    {
        if (BattleManager.instance.isAttack[spell_id] != 1
            && BattleManager.instance.isHeal[spell_id] != 1)
        {
            heal_point = 0;
        }

        player_current_hp += heal_point;
        

        //오버 회복 방지
        if (player_current_hp > player_max_hp)
        {
            heal_point = (heal_point + player_max_hp - player_current_hp);
            player_current_hp = player_max_hp;
        }
        player_Heal_txt(spell_id, heal_point);
        Damage_update();
    }

    private void monster_Heal(int spell_id, int heal_point)
    {
        if (BattleManager.instance.isAttack[spell_id] != 1
            && BattleManager.instance.isHeal[spell_id] != 1)
        {
            heal_point = 0;
        }

        monster_current_hp += heal_point;
        

        //오버 회복 방지
        if (monster_current_hp > monster_max_hp)
        {
            heal_point = (heal_point + monster_max_hp - monster_current_hp);
            monster_current_hp = monster_max_hp;
        }
        monster_Heal_txt(spell_id, heal_point);
        Damage_update();
    }

    private void player_Hit(int spell_id, int atk_point)
    {
        int Dam = Player_Hit_damage_calcul(atk_point);
        player_current_hp -= Dam;
        //오버 히트 방지
        if (player_current_hp < 0)
        {
            player_current_hp = 0;
        }
        player_Damage_txt(spell_id, Dam);
        Damage_update();
    }

    private void monster_Hit(int spell_id, int atk_point)
    {
        int Dam = Monster_Hit_damage_calcul(atk_point);
        //적 체력 감소
        monster_current_hp -= Dam;
        //오버 히트 방지
        if (monster_current_hp < 0)
        {
            monster_current_hp = 0;
        }
        monster_Damage_txt(spell_id, Dam);
        Damage_update();
    }

    private void player_Damage_txt(int spell_id, int point)
    {


        for (int i = 0; i < player_Damage_Text.Length; i++)
        {
            if (!player_Damage_Text[i].gameObject.activeSelf)
            {
                //피격당하는 원소의 색상으로 점멸 효과
                if (spell_id - 3 < 0 || spell_id == 19)
                {
                    player_Damage_Text[i].color = colors[0];
                }
                else
                {
                    player_Damage_Text[i].color = colors[((spell_id - 3) / 4) + 1];
                }

                if (P_Haste)
                {
                    player_Damage_Text[i].text = "회피";
                    P_Haste = false;
                }
                else
                {
                    //-1일때는 상태이상과 같은 고정 수치 데미지
                    if (point == -1)
                    {
                        player_Damage_Text[i].text = $"-{BattleManager.instance.number[spell_id]}";
                    }
                    else
                    {
                        player_Damage_Text[i].text = $"-{point}";
                    }
                }

                if (spell_id == 16)
                {
                    player_Damage_Text[i].gameObject.GetComponent<Demage_txt>().is_16 = true;
                    player_Damage_Text[i].gameObject.SetActive(true);
                }
                else
                {
                    player_Damage_Text[i].gameObject.SetActive(true);
                }
                
                break;
            }
        }
    }

    private void player_Heal_txt(int spell_id, int point)
    {
        if (BattleManager.instance.isAttack[spell_id] != 1
            && BattleManager.instance.isHeal[spell_id] != 1)
        {
            return;
        }

        for (int i = 0; i < player_Damage_Text.Length; i++)
        {
            if (!player_Damage_Text[i].gameObject.activeSelf)
            {
                //피격당하는 원소의 색상으로 점멸 효과
                if (spell_id - 3 < 0 || spell_id == 19)
                {
                    player_Damage_Text[i].color = colors[0];
                }
                else
                {
                    player_Damage_Text[i].color = colors[((spell_id - 3) / 4) + 1];
                }

                //-1일때는 상태이상과 같은 고정 수치 데미지
                if (point == -1)
                {
                    player_Damage_Text[i].text = $"+{BattleManager.instance.number[spell_id]}";
                }
                else
                {
                    player_Damage_Text[i].text = $"+{point}";
                }

                player_Damage_Text[i].gameObject.SetActive(true);
                break;
            }
        }
    }

    private void monster_Damage_txt(int spell_id, int point)
    {
        

        for (int i = 0; i < monster_Damage_Text.Length; i++)
        {
            if (!monster_Damage_Text[i].gameObject.activeSelf)
            {
                //피격당하는 원소의 색상으로 점멸 효과
                if (spell_id - 3 < 0)
                {
                    monster_Damage_Text[i].color = colors[0];
                }
                else
                {
                    monster_Damage_Text[i].color = colors[((spell_id - 3) / 4) + 1];
                }

                if (M_Haste)
                {
                    monster_Damage_Text[i].text = "회피";
                    M_Haste = false;
                }
                else
                {
                    //-1일때는 상태이상과 같은 고정 수치 데미지
                    if (point == -1)
                    {
                        monster_Damage_Text[i].text = $"-{BattleManager.instance.number[spell_id]}";
                    }
                    else
                    {
                        monster_Damage_Text[i].text = $"-{point}";
                    }
                }

                if (spell_id == 16)
                {
                    monster_Damage_Text[i].gameObject.GetComponent<Demage_txt>().is_16 = true;
                    monster_Damage_Text[i].gameObject.SetActive(true);
                }
                else
                {
                    monster_Damage_Text[i].gameObject.SetActive(true);
                }
                
                break;
            }
        }
    }

    private void monster_Heal_txt(int spell_id, int point)
    {
        if (BattleManager.instance.isAttack[spell_id] != 1
            && BattleManager.instance.isHeal[spell_id] != 1)
        {
            return;
        }

        for (int i = 0; i < monster_Damage_Text.Length; i++)
        {
            if (!monster_Damage_Text[i].gameObject.activeSelf)
            {
                //피격당하는 원소의 색상으로 점멸 효과
                if (spell_id - 3 < 0 || spell_id == 19)
                {
                    monster_Damage_Text[i].color = colors[0];
                }
                else
                {
                    monster_Damage_Text[i].color = colors[((spell_id - 3) / 4) + 1];
                }

                //-1일때는 상태이상과 같은 고정 수치 데미지
                if (point == -1)
                {
                    monster_Damage_Text[i].text = $"+{BattleManager.instance.number[spell_id]}";
                }
                else
                {
                    monster_Damage_Text[i].text = $"+{point}";
                }

                monster_Damage_Text[i].gameObject.SetActive(true);
                break;
            }
        }
    }

    private int Player_Hit_damage_calcul(int Hit_damage)
    {
        int damage = Hit_damage;

        if (damage == 0)
        {
            return damage;
        }

        //헤이스트가 존재 할 때
        if (Player_state.GetComponent<StateManagement>().counts[6] != 0)
        {
            damage = 0;
            Player_state.GetComponent<StateManagement>().counts[6]--;
            P_Haste = true;
            return damage;
        }
        //스톤스킨이 존재 할 때
        else if (Player_state.GetComponent<StateManagement>().counts[7] != 0)
        {
            damage = damage / 2;
            Player_state.GetComponent<StateManagement>().counts[7]--;
        }

        //방어도가 존재 할 때
        if (Player_state.GetComponent<StateManagement>().counts[4] != 0)
        {
            //어스스트라이크나 어스퀘이크일때
            if(spell_id == 12 || spell_id == 14)
            {
                Player_state.GetComponent<StateManagement>().counts[4] = 0;
            }
            else
            {
                if ((damage - Player_state.GetComponent<StateManagement>().counts[4]) < 0)
                {
                    Player_state.GetComponent<StateManagement>().counts[4] -= damage;
                    damage = 0;

                }
                else
                {
                    damage = damage - Player_state.GetComponent<StateManagement>().counts[4];
                    Player_state.GetComponent<StateManagement>().counts[4] = 0;
                }
            }
            
        }

        //운명 계산
        for(int i = 0; i < BattleManager.instance.F_list.Length; i++)
        {
            switch (BattleManager.instance.F_list[i])
            {
                case 12:
                    if(monster_current_hp > player_current_hp && F_counts[12] < 3)
                    {
                        damage = damage / 2;
                        F_counts[12]++;
                    }
                    break;
                case 14:
                    if (F_counts[14] < 2)
                    {
                        if(damage - 10 < 0)
                        {
                            damage = 0;
                        }
                        else
                        {
                            damage = damage - 10;
                        }
                        
                        F_counts[14]++;
                    }
                    break;
                case 16:
                    if (F_counts[16] < 2)
                    {
                        if (damage - 5 < 0)
                        {
                            damage = 0;
                        }
                        else
                        {
                            damage = damage - 5;
                        }

                        F_counts[16]++;
                    }
                    break;
                case 17:
                    if (F_counts[17] < 1)
                    {
                        if (damage > 10)
                        {
                            damage = 10;
                        }
                        F_counts[17]++;
                    }
                    break;
                default: 
                    break;
            }
        }

        return damage;

    }

    private int Player_Atk_damage_calcul(int Hit_damage)
    {
        int damage = Hit_damage;

        if (damage == 0)
        {
            return damage;
        }

        //운명 계산
        for (int i = 0; i < BattleManager.instance.F_list.Length; i++)
        {
            switch (BattleManager.instance.F_list[i])
            {
                case 0:
                    damage += 10;
                    break;
                case 1:
                    float ran = Random.Range(0f, 1f);
                    if (ran < 0.5f)
                    {
                        damage = damage + 10;
                    }
                    break;
                case 3:
                    float ran1 = Random.Range(0f, 1f);
                    if (ran1 < 0.3f)
                    {
                        damage = damage + 5;
                    }
                    break;
                case 5:
                    damage += 2;
                    break;
                default:
                    break;
            }
        }

        return damage;

    }

    private int Monster_Hit_damage_calcul(int Hit_damage)
    {
        int damage = Hit_damage;

        if(damage == 0)
        {
            return damage;
        }

        //헤이스트가 존재 할 때
        if (Monster_state.GetComponent<StateManagement>().counts[6] != 0)
        {
            damage = 0;
            Monster_state.GetComponent<StateManagement>().counts[6]--;
            M_Haste = true;
            return damage;
        }
        //스톤스킨이 존재 할 때
        else if (Monster_state.GetComponent<StateManagement>().counts[7] != 0)
        {
            damage = damage / 2;
            Monster_state.GetComponent<StateManagement>().counts[7]--;
        }
        
        //방어도가 존재 할 때
        if (Monster_state.GetComponent<StateManagement>().counts[4] != 0)
        {
            if (spell_id == 12 || spell_id == 14)
            {
                Monster_state.GetComponent<StateManagement>().counts[4] = 0;
            }
            else
            {
                if ((damage - Monster_state.GetComponent<StateManagement>().counts[4]) < 0)
                {
                    Monster_state.GetComponent<StateManagement>().counts[4] -= damage;
                    damage = 0;
                }
                else
                {
                    damage = damage - Monster_state.GetComponent<StateManagement>().counts[4];
                    Monster_state.GetComponent<StateManagement>().counts[4] = 0;
                }
            }
            
        }

        return damage;

    }

    private void Damage_update()
    {
        //데미지 계산 반영
        Monster_HP.value = (float)monster_current_hp / (float)monster_max_hp;
        Monster_HP_Text.text = $"{monster_current_hp} / {monster_max_hp}";
        Player_HP.value = (float)player_current_hp / (float)player_max_hp;
        Player_HP_Text.text = $"{player_current_hp} / {player_max_hp}";
    }

    private void Heal_fortune()
    {
        //운명 계산
        for (int i = 0; i < BattleManager.instance.F_list.Length; i++)
        {
            switch (BattleManager.instance.F_list[i])
            {
                case 6:
                    if (player_current_hp == 0 && F_counts[6] < 1)
                    {
                        player_Heal(2, 40);
                        F_counts[6]++;
                        Damage_update();
                    }
                    break;
                case 7:
                    if (player_current_hp < 10 && F_counts[7] < 1)
                    {
                        player_Heal(2, 30);
                        F_counts[7]++;
                        Damage_update();
                    }
                    break;
                case 9:
                    if (player_current_hp < 10 && F_counts[9] < 1)
                    {
                        player_Heal(2, 15);
                        F_counts[9]++;
                        Damage_update();
                    }
                    break;
                default:
                    break;
            }
        }
    }

    private void fortune_Additional()
    {
        //운명 계산
        for (int i = 0; i < BattleManager.instance.F_list.Length; i++)
        {
            switch (BattleManager.instance.F_list[i])
            {
                case 2:
                    float ran = Random.Range(0f, 1f);
                    if (ran < 0.3f)
                    {
                        fortune_First = true;
                    }
                    else
                    {
                        Debug.Log("30% 실패!!");
                    }
                    break;
                case 4:
                    float ran1 = Random.Range(0f, 1f);
                    if (ran1 < 0.1f)
                    {
                        fortune_Second = true;
                    }
                    else
                    {
                        Debug.Log("10% 실패!!");
                    }
                    break;
                default:
                    break;
            }
        }
    }

    public void fortune_Action(int fortune_num)
    {
        switch (fortune_num)
        {
            case 7:
                if (player_current_hp < 10 && F_counts[7] < 1)
                {
                    player_Heal(2, 30);
                    F_counts[7]++;
                    Damage_update();
                }
                break;
            case 9:
                if (player_current_hp < 10 && F_counts[9] < 1)
                {
                    player_Heal(2, 15);
                    F_counts[9]++;
                    Damage_update();
                }
                break;
            case 11:
                player_Heal(2, 10);
                break;
            case 13:
                Player_state.GetComponent<StateManagement>().counts[4] += 20;
                Player_state.transform.GetChild(4).gameObject.SetActive(true);
                break;
            case 15:
                Player_state.GetComponent<StateManagement>().counts[4] += 10;
                Player_state.transform.GetChild(4).gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void fortune_Heal(int fortune_num)
    {
        switch (fortune_num)
        {
            case 8:
                if (F_counts[8] < 4)
                {
                    player_Heal(2, 10);
                    F_counts[8]++;
                }
                break;
            case 10:
                if (F_counts[10] < 4)
                {
                    player_Heal(2, 5);
                    F_counts[10]++;
                }
                break;
            default:
                break;
        }
    }

    public void check_HP()
    {

        if (player_current_hp == 0)
        {
            StopCoroutine(coroutine);
            winlose_panel.GetComponent<winlose_panel>().is_Win = false;
            winlose_panel.SetActive(true);
        }
        else if (monster_current_hp == 0)
        {
            StopCoroutine(coroutine);
            winlose_panel.GetComponent<winlose_panel>().is_Win = true;
            winlose_panel.SetActive(true);
        }
        else
        {
            //End페이즈에서 HP체크가 발동했을 때만 실행
            if(BattleManager.instance.phase == Phase.End)
            {
                BattleManager.instance.phase = Phase.StandBy;
            }
            
        }
    }

    IEnumerator camera_shake()
    {
        float F_time = 0.4f;
        float time = 0f;
        while (time < 1)
        {
            time += Time.deltaTime / F_time;

            //반경 5f 안의 랜덤 위치 지정
            camera.transform.position = Random.insideUnitSphere * 0.4f;

            //높이도 같이 변경되니까 높이 고정하기, y값 초기화하기
            camera.transform.position = new Vector3(camera.transform.position.x, 0f,-10f);
            yield return null;
        }
        camera.transform.position = new Vector3(0f, 0f,-10f);
        StopCoroutine("camera_shake");
    }
}
