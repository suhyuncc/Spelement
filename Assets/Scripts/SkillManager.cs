using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;

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
    private GameObject[] hit_Effects;
    [SerializeField]
    private GameObject Book;

    private Color[] colors = {new Color(0.75f,0.75f,0.75f), new Color(0.75f, 0.75f, 1f),
        new Color(1, 0.75f, 0.75f), new Color(1f, 0.875f, 0.75f), new Color(0.875f, 1f, 0.75f) };

    public bool isActive;
    public bool isAttack;
    public bool player_turn;
    public int spell_id;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        //플레이어, 몬스터 체력 받아오기
    }

    // Update is called once per frame
    void Update()
    {
        if(isActive)
        {
            StartCoroutine(Skill_Active(player_turn, spell_id));
            isActive = false;
        }
    }

    IEnumerator Skill_Active(bool player_turn, int spell_id)
    {
        if (BattleManager.instance.isAttack[spell_id] == 1)
        {
            //투사체 날리기
            if (player_turn)
            {
                //플레이어가 몬스터에게
                skill_Effects[spell_id].transform.position = Player_apos.transform.position;
                skill_Effects[spell_id].transform.rotation = Quaternion.Euler(0, 0, 0);
                skill_Effects[spell_id].GetComponent<Attack>().speed = 15;
            }
            else
            {
                //몬스터가 플레이어에게
                skill_Effects[spell_id].transform.position = Monster_apos.transform.position;
                skill_Effects[spell_id].transform.rotation = Quaternion.Euler(0, 0, 180);
                skill_Effects[spell_id].GetComponent<Attack>().speed = -15;
            }
            skill_Effects[spell_id].SetActive(true);
            yield return new WaitForSeconds(0.5f);

            //피격 및 데미지 계산
            if (player_turn)
            {
                hit_Effects[spell_id].transform.position = Monster.transform.position;
                hit_Effects[spell_id].SetActive(true);

                //피격당하는 원소의 색상으로 점멸 효과
                if (spell_id - 3 < 0)
                {
                    Monster.GetComponent<SpriteRenderer>().color = colors[0];
                }
                else
                {
                    Monster.GetComponent<SpriteRenderer>().color = colors[((spell_id - 3) / 4) + 1];
                }
            }
            else
            {
                hit_Effects[spell_id].transform.position = Player.transform.position;
                hit_Effects[spell_id].SetActive(true);

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
        else
        {
            //피격 및 데미지 계산
            if (player_turn)
            {
                hit_Effects[spell_id].transform.position = Player.transform.position;
                hit_Effects[spell_id].SetActive(true);
            }
            else
            {
                hit_Effects[spell_id].transform.position = Monster.transform.position;
                hit_Effects[spell_id].SetActive(true);
            }
        }
        

        
        yield return new WaitForSeconds(0.3f);

        //배틀 페이즈 종료
        
        if(player_turn)
        {
            Monster.GetComponent<SpriteRenderer>().color = new Color(1,1,1);
            Book.SetActive(true);
        }
        else
        {
            Player.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
            BattleManager.instance.monster_done = true;
        }

        StopCoroutine("Skill_Active");
        
    }
}
