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
    private GameObject Monster;
    [SerializeField]
    private Slider Monster_HP;
    [SerializeField]
    private Text Monster_HP_Text;
    [SerializeField]
    private GameObject Monster_state;

    [SerializeField]
    private GameObject[] skill_Effects;
    [SerializeField]
    private GameObject[] hit_Effects;
    [SerializeField]
    private GameObject Book;

    private Color[] colors = {new Color(0.75f,0.75f,0.75f), new Color(0.75f, 0.75f, 1f),
        new Color(1, 0.75f, 0.75f), new Color(1f, 0.875f, 0.75f), new Color(0.875f, 1f, 0.75f) };

    public bool isActive;
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
        //투사체 날리기
        skill_Effects[0].SetActive(true);
        yield return new WaitForSeconds(0.5f);

        //피격 및 데미지 계산
        hit_Effects[0].SetActive(true);

        //피격당하는 원소의 색상으로 점멸 효과
        if (player_turn)
        {
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
            if (spell_id - 3 < 0)
            {
                Player.GetComponent<SpriteRenderer>().color = colors[0];
            }
            else
            {
                Player.GetComponent<SpriteRenderer>().color = colors[((spell_id - 3) / 4) + 1];
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
            BattleManager.instance.phase = Phase.End;
        }
        
    }
}
