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
    private GameObject wind;

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
        //�÷��̾�, ���� ü�� �޾ƿ���
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
        //���� ����϶�
        if (BattleManager.instance.isAttack[spell_id] == 1)
        {
            //����ü ������
            if (player_turn)
            {
                //�÷��̾ ���Ϳ���
                skill_Effects[spell_id].transform.position = Player_apos.transform.position;
                //skill_Effects[spell_id].transform.rotation = Quaternion.Euler(0, 0, 0);
                skill_Effects[spell_id].transform.eulerAngles += new Vector3(0,0,0);
                skill_Effects[spell_id].GetComponent<Attack>().speed *= 1;
                if(spell_id == 17)
                {
                    wind.transform.position = Player_apos.transform.position;
                    wind.SetActive(true);
                }
            }
            else
            {
                //���Ͱ� �÷��̾��
                skill_Effects[spell_id].transform.position = Monster_apos.transform.position;
                skill_Effects[spell_id].transform.rotation = Quaternion.Euler(0, 0, 180);
                skill_Effects[spell_id].GetComponent<Attack>().speed *= -1;
            }
            skill_Effects[spell_id].SetActive(true);

            yield return new WaitForSeconds(0.5f);

            //�ǰ�
            if (player_turn)
            {
                //���Ϳ���
                hit_Effects[spell_id].transform.position = Monster.transform.position;
                hit_Effects[spell_id].SetActive(true);

                //�ǰݴ��ϴ� ������ �������� ���� ȿ��
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
                //�÷��̾��
                hit_Effects[spell_id].transform.position = Player.transform.position;
                hit_Effects[spell_id].SetActive(true);

                //�ǰݴ��ϴ� ������ �������� ���� ȿ��
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
        //���� ����� �ƴҶ�
        else
        {
            //�ǰ�
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

        wind.SetActive(false);
        //������ ���
        if (player_turn)
        {
            if (BattleManager.instance.isAttack[spell_id] == 1)
            {
                //�� ü�� ����
                monster_current_hp -= BattleManager.instance.number[spell_id];
            }
            else
            {
                player_current_hp += BattleManager.instance.number[spell_id];
                //���� ȸ�� ����
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
                //���� ȸ�� ����
                if (monster_current_hp > monster_max_hp)
                {
                    monster_current_hp = monster_max_hp;
                }
            }
            
            
        }

        //������ ��� �ݿ�
        Monster_HP.value = (float)monster_current_hp / (float)monster_max_hp;
        Monster_HP_Text.text = $"{monster_current_hp} / {monster_max_hp}";
        Player_HP.value = (float)player_current_hp / (float)player_max_hp;
        Player_HP_Text.text = $"{player_current_hp} / {player_max_hp}";

        //�߰� �ൿ
        Additional(spell_id);

        //��Ʋ ������ ����
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
            
            //ȿ��
            hit_Effects[spell_id].transform.position = Player.transform.position;
            hit_Effects[spell_id].SetActive(true);
            //����
            Player.GetComponent<SpriteRenderer>().color = colors[((spell_id - 3) / 4) + 1];
            Demage_Text.color = colors[((spell_id - 3) / 4) + 1];

            Demage_Text.text = $"{BattleManager.instance.number[spell_id]}";
            Demage_Text.gameObject.SetActive(true);

            sfx.clip = hit_SFX[0];
            sfx.Play();

            player_current_hp -= 3;
            yield return new WaitForSeconds(0.3f);

            //������ ��� �ݿ�
            Player_HP.value = (float)player_current_hp / (float)player_max_hp;
            Player_HP_Text.text = $"{player_current_hp} / {player_max_hp}";

            //�� ����
            Player.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);

            yield return new WaitForSeconds(0.3f);
            Book.SetActive(true);
            
        }
        else
        {
            //ȿ��
            hit_Effects[spell_id].transform.position = Monster.transform.position;
            hit_Effects[spell_id].SetActive(true);
            //����
            Monster.GetComponent<SpriteRenderer>().color = colors[((spell_id - 3) / 4) + 1];
            Demage_Text.color = colors[((spell_id - 3) / 4) + 1];

            Demage_Text.text = $"{BattleManager.instance.number[spell_id]}";
            Demage_Text.gameObject.SetActive(true);

            sfx.clip = hit_SFX[0];
            sfx.Play();

            monster_current_hp -= 3;
            yield return new WaitForSeconds(0.3f);

            //������ ��� �ݿ�
            Monster_HP.value = (float)monster_current_hp / (float)monster_max_hp;
            Monster_HP_Text.text = $"{monster_current_hp} / {monster_max_hp}";

            //�� ����
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
            case 9:
                if (player_turn)
                {
                    Monster_state.GetComponent<StateManagement>().counts[0] = 9;
                    Monster_state.transform.GetChild(0).gameObject.SetActive(true);
                }
                else
                {
                    Player_state.GetComponent<StateManagement>().counts[0] = 9;
                    Player_state.transform.GetChild(0).gameObject.SetActive(true);
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
}
