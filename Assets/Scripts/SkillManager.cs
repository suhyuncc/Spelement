using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;

    [SerializeField]
    private Camera camera;
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
    private GameObject[] skill_Effects;
    [SerializeField]
    private GameObject[] P_hit_Effects;
    [SerializeField]
    private GameObject[] M_hit_Effects;
    [SerializeField]
    private AudioClip[] hit_SFX;
    [SerializeField]
    private GameObject Book;
    [SerializeField]
    private Text Demage_Text;

    private Color[] colors = {new Color(0.75f,0.75f,0.75f), new Color(0.75f, 0.75f, 1f),
        new Color(1, 0.75f, 0.75f), new Color(1f, 0.875f, 0.75f), new Color(0.875f, 1f, 0.75f) };

    private AudioSource sfx;

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

    private int count;
    private int _percent;

    private Coroutine coroutine;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        sfx = this.GetComponent<AudioSource>();
        _percent = 0;
    }

    private void Start()
    {
        //플레이어, 몬스터 체력 받아오기
        player_max_hp = BattleManager.instance.player_HP[0];
        monster_max_hp = BattleManager.instance.monster_HP[0];
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
                skill_Effects[spell_id].transform.eulerAngles += new Vector3(0,0,0);
                skill_Effects[spell_id].GetComponent<Attack>().speed *= 1;
                spell_field(spell_id);
            }
            else
            {
                //몬스터가 플레이어에게
                skill_Effects[spell_id].transform.position = Monster_apos.transform.position;
                skill_Effects[spell_id].transform.rotation = Quaternion.Euler(0, 0, 180);
                skill_Effects[spell_id].GetComponent<Attack>().speed *= -1;
            }
            skill_Effects[spell_id].SetActive(true);

            yield return new WaitForSeconds(0.5f);

            //피격
            if (player_turn)
            {
                //몬스터에게
                M_hit_Effects[spell_id].SetActive(true);
                if(spell_id == 14)
                {
                    StartCoroutine("camera_shake");
                    
                }

                //피격당하는 원소의 색상으로 점멸 효과
                if (spell_id - 3 < 0)
                {
                    Monster.GetComponent<SpriteRenderer>().color = colors[0];
                    Demage_Text.color = colors[0];
                }
                else
                {
                    Monster.GetComponent<SpriteRenderer>().color = colors[((spell_id - 3) / 4) + 1];
                    Demage_Text.color = colors[((spell_id - 3) / 4) + 1];
                }
                Demage_Text.text = $"{BattleManager.instance.number[spell_id]}";
                Demage_Text.gameObject.SetActive(true);

                sfx.clip = hit_SFX[0];
                sfx.Play();
            }
            else
            {
                //플레이어에게
                P_hit_Effects[spell_id].SetActive(true);

                //피격당하는 원소의 색상으로 점멸 효과
                if (spell_id - 3 < 0 || spell_id == 19)
                {
                    Player.GetComponent<SpriteRenderer>().color = colors[0];
                }
                else
                {
                    Player.GetComponent<SpriteRenderer>().color = colors[((spell_id - 3) / 4) + 1];
                }

            }
        }
        //공격 기술이 아닐때
        else
        {
            //회복 및 방어막
            if (player_turn)
            {
                P_hit_Effects[spell_id].SetActive(true);
            }
            else
            {
                M_hit_Effects[spell_id].SetActive(true);
            }
        }
        
        yield return new WaitForSeconds(0.3f);

        spin_spell_field.SetActive(false);
        //데미지 계산
        if (player_turn)
        {
            if (BattleManager.instance.isAttack[spell_id] == 1)
            {
                //적 체력 감소
                monster_current_hp -= BattleManager.instance.number[spell_id];
            }
            else
            {
                player_current_hp += BattleManager.instance.number[spell_id];
                //오버 회복 방지
                if (player_current_hp > player_max_hp)
                {
                    player_current_hp = player_max_hp;
                }
            }
                
        }
        else
        {
            if (BattleManager.instance.isAttack[spell_id] == 1)
            {
                if (spell_id == 19)
                {
                    player_current_hp -= nomal_Dem;
                }
                else
                {
                    player_current_hp -= BattleManager.instance.number[spell_id];
                }
            }
            else
            {
                monster_current_hp += BattleManager.instance.number[spell_id];
                //오버 회복 방지
                if (monster_current_hp > monster_max_hp)
                {
                    monster_current_hp = monster_max_hp;
                }
            }
            
            
        }

        //추가 행동
        Additional(spell_id);

        //데미지 계산 반영
        Monster_HP.value = (float)monster_current_hp / (float)monster_max_hp;
        Monster_HP_Text.text = $"{monster_current_hp} / {monster_max_hp}";
        Player_HP.value = (float)player_current_hp / (float)player_max_hp;
        Player_HP_Text.text = $"{player_current_hp} / {player_max_hp}";

        

        //배틀 페이즈 종료
        if (player_turn)
        {
            Monster.GetComponent<SpriteRenderer>().color = new Color(1,1,1);
            Book.SetActive(true);
        }
        else
        {
            Player.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
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
            Demage_Text.color = colors[((spell_id - 3) / 4) + 1];

            Demage_Text.text = $"{BattleManager.instance.number[spell_id]}";
            Demage_Text.gameObject.SetActive(true);

            sfx.clip = hit_SFX[0];
            sfx.Play();

            player_current_hp -= 3;
            yield return new WaitForSeconds(0.3f);

            //데미지 계산 반영
            Player_HP.value = (float)player_current_hp / (float)player_max_hp;
            Player_HP_Text.text = $"{player_current_hp} / {player_max_hp}";

            //색 복구
            Player.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);

            yield return new WaitForSeconds(0.3f);
            Book.SetActive(true);
            
        }
        else
        {
            //효과
            M_hit_Effects[spell_id].SetActive(true);
            //점멸
            Monster.GetComponent<SpriteRenderer>().color = colors[((spell_id - 3) / 4) + 1];
            Demage_Text.color = colors[((spell_id - 3) / 4) + 1];

            Demage_Text.text = $"{BattleManager.instance.number[spell_id]}";
            Demage_Text.gameObject.SetActive(true);

            sfx.clip = hit_SFX[0];
            sfx.Play();

            monster_current_hp -= 3;
            yield return new WaitForSeconds(0.3f);

            //데미지 계산 반영
            Monster_HP.value = (float)monster_current_hp / (float)monster_max_hp;
            Monster_HP_Text.text = $"{monster_current_hp} / {monster_max_hp}";

            //색 복구
            Monster.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);

            yield return new WaitForSeconds(0.3f);

        }
        
    }

    private void Attack(int spell_id)
    {

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
                    Player_state.GetComponent<StateManagement>().counts[2] = 5;
                    Player_state.transform.GetChild(2).gameObject.SetActive(true);

                }
                else
                {
                    Monster_state.GetComponent<StateManagement>().counts[2] = 5;
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

            //불기둥(추가 예정)


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
                spin_spell_field.SetActive(true);
                break;
            case 7:
                spin_spell_field.GetComponent<SpriteRenderer>().color = colors[2];
                spin_spell_field.SetActive(true);
                break;
            case 13:
                spin_spell_field.GetComponent<SpriteRenderer>().color = colors[3];
                spin_spell_field.SetActive(true);
                break;
            case 17:
                spin_spell_field.GetComponent<SpriteRenderer>().color = colors[4];
                spin_spell_field.SetActive(true);
                break;
            default:
                break;
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
