using Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.XR;
using UnityEngine;
using UnityEngine.SceneManagement;

//gh
public class GameManager : Singleton<GameManager>
{
    public state currentState = state.idle;//stateManagement script���� �ۼ��� variable type ��¥�� GameManager���� �����ϹǷ� �ǵ���� �ʿ� ����
    public int currentStageSerialNumber = 0;//�̰͵� StageButton���� �����ðŰ� �밡�ٷ� ������������ �����ҰŶ� �ǵ���� �ʿ� ����
    public int previousSerialNumber = 0;//�̰� GameManager �������� currentStageSerialNumber �ϴ� �����Ϸ��� �����°Ŵϱ� �ǵ���� �ʿ� ����
    //�ø��� �ѹ�(���� �ø��� �ѹ�)�� 1���� ���۵�
    public bool currentStageCleared = false;//battle �¸��ϸ� true�� �ٱ���� �ƴ� false ���·� scene �ٱ����

    public GameObject MapManager;//MapManager�� ã�� GameObject
    public GameObject DialogueManager; //DialogueManager�� ã�� GameObject
    public GameObject LoadingManager;
    public GameObject BattleManager;
    public GameObject SkillManager;
    public GameObject SpellManager;

    public int[] spell_list;

    private string sceneName;
    private string eventName;

    [SerializeField]
    private bool isButtonClickedInIdle = false;
    [SerializeField]
    private int memorizeClearedStage;

    void OnEnable()//��������
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)//����ٰ� ���� ��
    {
        Scene _scene = SceneManager.GetActiveScene();
        if (currentState == state.idle && _scene.name == "Dialogue_Scene") //Idle scene�� �������� ��
        {
            MapManager = GameObject.Find("MapManager");
            //�Ź� Idle scene�� �������� �� ���൵������ �� ����
            MapManager.GetComponent<MapManagement>().StageClear(memorizeClearedStage);
        }
        else if (currentState == state.dialogue && _scene.name == "Dialogue_Scene") // Idle scene but ��ȭ�� ����Ǿ�� �� ��
        {
            DialogueManager = GameObject.Find("DialogueSystem");
            MapManager = GameObject.Find("MapManager");
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
        else if (currentState == state.battle && _scene.name == "BattleScene") // BattleScene�� �������� ��
        {
            BattleManager = GameObject.Find("BattleManager");
            SkillManager = GameObject.Find("SkillManager");
        }
        else if (currentState == state.spell_setting && _scene.name == "Spell_Custom_Scene") // Spellsettingscene�� �������� ��
        {
            SpellManager = GameObject.Find("SpellCustom_Manager");
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

    public void GetEventName(string _eventName)
    {
        eventName = _eventName;
    }


    Scene __scene;
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

        //battle �� -> dialogue ��
        if (currentState == state.battle && isButtonClickedInIdle == true && __scene.name == "BattleScene")
        {
            if (eventName == null)
                currentState = state.idle;
            else
                currentState = state.dialogue;

            isButtonClickedInIdle = false;
            sceneName = "Dialogue_Scene";
            SceneManager.LoadScene("Loading");

            if (currentStageCleared)//Ŭ����� ���ں���
            {
                memorizeClearedStage = currentStageSerialNumber;
                
                currentStageCleared = false;//�������� ���õ��� ���� ���·�
            }

            currentStageSerialNumber = 0;
        }
    }

    public void IdleSceneChange()
    {
        isButtonClickedInIdle = true;
    }
}
