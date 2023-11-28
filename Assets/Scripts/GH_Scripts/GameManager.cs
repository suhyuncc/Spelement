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
    public GameObject SpellManager;

    private string sceneName;
    private string eventName;

    private bool isButtonClickedInIdle = false;
    private int memorizeClearedStage;

    [SerializeField]
    private int[] spellList = new int[12] {1,2,3,0,0,0,0,0,0,0,0,0}; //���߿� ù 3 spell id�� �°� ������ ��

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
        else if (currentState == state.spell_setting && _scene.name == "Spell_Custom_Scene") // Spellsettingscene�� �������� ��
        {
            SpellManager = GameObject.Find("SpellCustom_Manager");
        }
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }//������ �� ����� �� ���� Update ���� ���� ���� ��(start�� �ѹ��� ����ǹǷ�)�͵� �����Ű�� �Լ���
    public void StartBattle(int serialNo) //startbutton(Stage)���� serialNumber �޾ƿ� �Լ�
    {
        currentStageSerialNumber= serialNo;
    }
    public void SetEventName(string _eventName)
    {
        eventName = _eventName;
    }

    private Scene __scene;
    private void Update()
    {
        __scene = SceneManager.GetActiveScene();
        if (currentState == state.battle && currentStageCleared == true) //��Ʋ���ε� ���� ����Ʈ��(clear)
        {
            if (eventName == null) // ��ȭ event�� ���� �ʾҴٸ�
                currentState = state.idle; // �׳� idle scene���� 
            else //�޾Ҵٸ�
                currentState = state.dialogue;
            previousSerialNumber = currentStageSerialNumber;
            currentStageSerialNumber = 0;
            sceneName = "Dialogue_Scene";
            SceneManager.LoadScene("Loading");
        }
        if (currentState == state.idle && currentStageCleared == true && __scene.name == "Dialogue_Scene") //��Ʋ ������ idle scene���� ���ƿ��� ���� ����� �����
        {
            if (previousSerialNumber != 0 && MapManager != null)
            {
                memorizeClearedStage = previousSerialNumber;
                MapManager.GetComponent<MapManagement>().StageClear(previousSerialNumber);
                currentStageCleared = false;//�������� ���õ��� ���� ���·�
                previousSerialNumber = 0;
            }
        }
        if (currentState == state.idle && currentStageSerialNumber != 0) //��ư�� Ŭ���Ǿ �ø��� �ѹ��� gameManager�� �ԷµǾ��� �� -> ��Ʋ ������ �Ѿ��
        {
            currentState = state.battle;
            //sceneName = "TestingBattle"; //�̰� ��Ʋ �� �̸�����
            sceneName = "BattleScene";
            SceneManager.LoadScene("Loading");
            Debug.Log("Start Battle!");
        }
        if (currentState == state.idle && isButtonClickedInIdle == true && __scene.name == "Dialogue_Scene") //spellsetting ��ư�� ������ ��
        {
            isButtonClickedInIdle = false;
            currentState = state.spell_setting;
            //���ݱ��� Ŭ���� �� �������� ���� ������ ���� ��
            previousSerialNumber = memorizeClearedStage;
            sceneName = "Spell_Custom_Scene";
            SceneManager.LoadScene("Loading");
        }
        if (currentState == state.spell_setting && isButtonClickedInIdle == true && __scene.name == "Spell_Custom_Scene") //spellsetting ������ ���� ��ư�� ������ ��
        {
            isButtonClickedInIdle = false;
            currentState = state.idle;
            currentStageCleared = true;
            sceneName = "Dialogue_Scene";
            SceneManager.LoadScene("Loading");
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

    public void IsStageFailed() // �������� ������ �������� �� ȣ����� �Լ�
    {
        currentStageSerialNumber /= 3; // ex> 5���������� 1�� ��ȯ
        currentStageSerialNumber *= 3; // ex> ��������ȯ�� 1�� 3���� ��ȯ --> 3stage���� Ŭ���� �� ������ ó��
        eventName = null; // ������ ���������� ��ȭ�� �־��� �� �����Ƿ�
        currentStageCleared = true; // scene�� Idle Scene���� ��ȯ�� �� �ֵ���
    }
}
