using Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.XR;
using UnityEngine;
using UnityEngine.SceneManagement;

//gh
public class GameManager : Singleton<GameManager>
{
    //start������ �����ؾ� �ǹǷ� ���� start�� ����
    public state currentState = state.start;//stateManagement script���� �ۼ��� variable type ��¥�� GameManager���� �����ϹǷ� �ǵ���� �ʿ� ����
    public int currentStageSerialNumber = 0;//�̰͵� StageButton���� �����ðŰ� �밡�ٷ� ������������ �����ҰŶ� �ǵ���� �ʿ� ����
    //�ø��� �ѹ�(���� �ø��� �ѹ�)�� 1���� ���۵�
    public bool currentStageCleared = false;//battle �¸��ϸ� true�� �ٱ���� �ƴ� false ���·� scene �ٱ����

    public GameObject MapManager;//MapManager�� ã�� GameObject
    public GameObject DialogueManager; //DialogueManager�� ã�� GameObject
    public GameObject LoadingManager;
    public GameObject SpellManager;

    public int[] spell_list;

    private string sceneName;
    private string eventName;

    [SerializeField]
    private bool isButtonClickedInIdle = false;
    public int memorizeClearedStage;

    [SerializeField]
    private int[] spellList = new int[12] {1,2,3,0,0,0,0,0,0,0,0,0}; //���߿� ù 3 spell id�� �°� ������ ��
    
    //for sound
    [SerializeField]
    private float masterVolume = 100f;
    [SerializeField]
    private float backgroundVolume = 100f;
    [SerializeField]
    private float effectVolume = 100f;
    [SerializeField]
    private bool masterToggle = false;
    [SerializeField]
    private bool backgroundToggle = false;
    [SerializeField]
    private bool effectToggle = false;
    [SerializeField]
    private GameObject option;

    private void Start()
    {
        eventName = null;
    }
    void OnEnable()//��������
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)//����ٰ� ���� ��
    {
        Scene _scene = SceneManager.GetActiveScene();
        if (currentState == state.idle && _scene.name == "Dialogue_Scene") //Idle scene�� �������� ��
        {
            DialogueManager = GameObject.Find("DialogueSystem");
            MapManager = GameObject.Find("MapManager");
            option = GameObject.Find("OptionControl").GetComponent<OptionControl>().GetOptionPanel();
            if (option != null)
            {
                option.SetActive(true);
                option.GetComponent<AudioSlider>().OnSceneChangedSettingAudio(this.gameObject,masterVolume, backgroundVolume, effectVolume, masterToggle, backgroundToggle, effectToggle);
                option.SetActive(false);
            }
            //�Ź� Idle scene�� �������� �� ���൵�� ���� �� ����
            MapManager.GetComponent<MapManagement>().StageClear(memorizeClearedStage);
        }
        else if (currentState == state.dialogue && _scene.name == "Dialogue_Scene") // Idle scene but ��ȭ�� ����Ǿ�� �� ��
        {
            DialogueManager = GameObject.Find("DialogueSystem");
            MapManager = GameObject.Find("MapManager");
            option = GameObject.Find("OptionControl").GetComponent<OptionControl>().GetOptionPanel();
            option.SetActive(true);
            option.GetComponent<AudioSlider>().OnSceneChangedSettingAudio(this.gameObject,masterVolume, backgroundVolume, effectVolume, masterToggle, backgroundToggle, effectToggle);
            option.SetActive(false);
            MapManager.GetComponent<MapManagement>().SceneChanged();
            DialogueManager.GetComponent<Dialogue_Manage>().GetEventName(eventName);
            eventName = null;
        }
        else if (_scene.name == "Loading") // Loading scene�� �������� ��
        {
            LoadingManager = GameObject.Find("LoadManager");
            if (LoadingManager != null)
            {
                LoadingManager.GetComponent<LoadingScene>().GetSceneName(sceneName);
                sceneName = null;
            }
        }
        else if (currentState == state.spell_setting && _scene.name == "Spell_Custom_Scene") // Spellsettingscene�� �������� ��
        {
            SpellManager = GameObject.Find("SpellCustom_Manager");
            option = GameObject.Find("OptionControl").GetComponent<OptionControl>().GetOptionPanel();
            option.SetActive(true);
            option.GetComponent<AudioSlider>().OnSceneChangedSettingAudio(this.gameObject,masterVolume, backgroundVolume, effectVolume, masterToggle, backgroundToggle, effectToggle);
            option.SetActive(false);
        }
        else if (_scene.name == "BattleScene")
        {
            option = GameObject.Find("OptionControl").GetComponent<OptionControl>().GetOptionPanel();
            option.SetActive(true);
            option.GetComponent<AudioSlider>().OnSceneChangedSettingAudio(this.gameObject,masterVolume, backgroundVolume, effectVolume, masterToggle, backgroundToggle, effectToggle);
            option.SetActive(false);
        }
        else if(_scene.name == "Start Scene")
        {
            option = GameObject.Find("OptionControl").GetComponent<OptionControl>().GetOptionPanel();
            option.SetActive(true);
            option.GetComponent<AudioSlider>().OnSceneChangedSettingAudio(this.gameObject,masterVolume, backgroundVolume, effectVolume, masterToggle, backgroundToggle, effectToggle);
            option.SetActive(false);
        }
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }//������ �� ����� �� ���� Update ���� ���� ���� ��(start�� �ѹ��� ����ǹǷ�)�͵� �����Ű�� �Լ���

    //startbutton(Stage)���� serialNumber �޾ƿ��� battle������ ��ȯ
    public void StartBattle(int serialNo) 
    {
        currentStageSerialNumber= serialNo;
        currentState = state.battle;
        sceneName = "BattleScene";
        SceneManager.LoadScene("Loading");
        Debug.Log("Start Battle!");
    }

    public void SaveandBack()
    {
        //������ �߰�

        currentState = state.start;
        sceneName = "Start Scene";
        SceneManager.LoadScene("Loading");
    }

    public void SetEventName(string _eventName)
    {
        eventName = _eventName;
    }

    private Scene __scene;
    private void Update()
    {
        __scene = SceneManager.GetActiveScene();

        //dialogue �� -> spellsetting ��
        if (currentState == state.idle && isButtonClickedInIdle == true && __scene.name == "Dialogue_Scene")
        {
            isButtonClickedInIdle = false;
            currentState = state.spell_setting;
            sceneName = "Spell_Custom_Scene";
            SceneManager.LoadScene("Loading");
        }

        //spellsetting �� -> dialogue ��
        if (currentState == state.spell_setting && isButtonClickedInIdle == true && __scene.name == "Spell_Custom_Scene") 
        {
            isButtonClickedInIdle = false;
            currentState = state.idle;
            sceneName = "Dialogue_Scene";
            SceneManager.LoadScene("Loading");
        }

        //start �� -> dialogue ��
        if (currentState == state.start && isButtonClickedInIdle == true && __scene.name == "Start Scene")
        {
            isButtonClickedInIdle = false;
            currentState = state.idle;
            sceneName = "Dialogue_Scene";
            SceneManager.LoadScene("Loading");
        }

        //battle �� -> dialogue ��
        if (currentState == state.battle && isButtonClickedInIdle == true && __scene.name == "BattleScene")
        {
            isButtonClickedInIdle = false;
            sceneName = "Dialogue_Scene";
            SceneManager.LoadScene("Loading");

            if (currentStageCleared)//Ŭ����� ���ں���
            {
                memorizeClearedStage = currentStageSerialNumber;
                
                currentStageCleared = false;//�������� ���õ��� ���� ���·�
            }
            else
            {
                eventName = null;
            }

            if (eventName == null)
                currentState = state.idle;
            else
                currentState = state.dialogue;

            Debug.Log(eventName);
            currentStageSerialNumber = 0;
        }
    }

    public void IdleSceneChange() // spell setting < - > idle �� scene ��ȯ�� ���� ���
    {
        isButtonClickedInIdle = true;
    }

    public void SetSpellBook(int[] _spellset) // spell setting scene���� ������ ������ �� ���
    {                                         
        spellList = _spellset;
    }
    public void SetSpellBook(int setCount, int spellId)// �ʿ��ϴٸ� ���� ������ ��ȯ�Ͽ��� ��� ����.
    {
        spellList[setCount] = spellId;
    }

    public int[] GetSpellSet() // spell setting or battle scene���� ������ spell ��ġ ������ ������ �� ���
    {
        return spellList;
    }

    public void Addspell(int stage_id)
    {
        int[] temp_list = new int[spell_list.Length + 1];
        int spell_id;

        spell_list.CopyTo(temp_list, 0);
        //�������� ������ ���� Ŭ����� ���� �ڵ� �߰�
        switch (stage_id)
        {
            case 0:
                spell_id = 15;
                temp_list.SetValue(spell_id, spell_list.Length);
                break;
            case 1:
                spell_id = 16;
                temp_list.SetValue(spell_id, spell_list.Length);
                break;
            case 2:
                spell_id = 17;
                temp_list.SetValue(spell_id, spell_list.Length);
                break;
            case 3:
                spell_id = 11;
                temp_list.SetValue(spell_id, spell_list.Length);
                break;
            case 4:
                spell_id = 12;
                temp_list.SetValue(spell_id, spell_list.Length);
                break;
            case 5:
                spell_id = 13;
                temp_list.SetValue(spell_id, spell_list.Length);
                break;
            case 6:
                spell_id = 3;
                temp_list.SetValue(spell_id, spell_list.Length);
                break;
            case 7:
                spell_id = 4;
                temp_list.SetValue(spell_id, spell_list.Length);
                break;
            case 8:
                spell_id = 5;
                temp_list.SetValue(spell_id, spell_list.Length);
                break;
            default: 
                break;
        }   
        spell_list = temp_list;
    }

    public void IsStageFailed() // �������� ������ �������� �� ȣ����� �Լ�
    {
        currentStageSerialNumber /= 3; // ex> 5���������� 1�� ��ȯ
        currentStageSerialNumber *= 3; // ex> ��������ȯ�� 1�� 3���� ��ȯ --> 3stage���� Ŭ���� �� ������ ó��
        eventName = null; // ������ ���������� ��ȭ�� �־��� �� �����Ƿ�
        currentStageCleared = true; // scene�� Idle Scene���� ��ȯ�� �� �ֵ���
    }
    public int GetStageNumber()
    {
        return memorizeClearedStage/3; //���ȭ�� ��� �� �� ��
    }
    public void SetVolM(float vol, bool tog)
    {
        masterVolume = vol;
        masterToggle= tog;
    }
    public void SetVolB(float vol, bool tog)
    {
        backgroundVolume = vol;
        backgroundToggle = tog;
    }
    public void SetVolE(float vol, bool tog)
    {
        effectVolume = vol;
        effectToggle = tog;
    }
}
