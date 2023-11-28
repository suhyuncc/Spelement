using Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.XR;
using UnityEngine;
using UnityEngine.SceneManagement;

//gh
public class GameManager : Singleton<GameManager>
{
    public state currentState = state.idle;//stateManagement script에서 작성된 variable type 어짜피 GameManager에서 관리하므로 건드려줄 필요 없음
    public int currentStageSerialNumber = 0;//이것도 StageButton에서 가져올거고 노가다로 스테이지마다 설정할거라 건드려줄 필요 없음
    public int previousSerialNumber = 0;//이건 GameManager 루프나서 currentStageSerialNumber 일단 저장하려고 가져온거니까 건드려줄 필요 없음
    //시리얼 넘버(몬스터 시리얼 넘버)는 1부터 시작됨
    public bool currentStageCleared = false;//battle 승리하면 true로 바까줘요 아님 false 상태로 scene 바까줘요

    public GameObject MapManager;//MapManager를 찾을 GameObject
    public GameObject DialogueManager; //DialogueManager를 찾을 GameObject
    public GameObject LoadingManager;
    public GameObject SpellManager;

    private string sceneName;
    private string eventName;

    private bool isButtonClickedInIdle = false;
    private int memorizeClearedStage;

    [SerializeField]
    private int[] spellList = new int[12] {1,2,3,0,0,0,0,0,0,0,0,0}; //나중에 첫 3 spell id에 맞게 수정할 것

    void OnEnable()//여서부터
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)//여기다가 넣을 것
    {
        Scene _scene = SceneManager.GetActiveScene();
        if (currentState == state.idle && _scene.name == "Dialogue_Scene") //Idle scene에 도달했을 때
        {
            MapManager = GameObject.Find("MapManager");
        }
        else if (currentState == state.dialogue && _scene.name == "Dialogue_Scene") // Idle scene but 대화가 선행되어야 할 때
        {
            DialogueManager = GameObject.Find("DialogueSystem");
            MapManager = GameObject.Find("MapManager");
            MapManager.GetComponent<MapManagement>().SceneChanged();
            DialogueManager.GetComponent<Dialogue_Manage>().GetEventName(eventName);
            eventName = null;
        }
        else if (_scene.name == "Loading") // Loading scene에 도달했을 때
        {
            LoadingManager = GameObject.Find("LoadManager");
            if (LoadingManager != null)
            {
                LoadingManager.GetComponent<LoadingScene>().GetSceneName(sceneName);
                sceneName = null;
            }
        }
        else if (currentState == state.spell_setting && _scene.name == "Spell_Custom_Scene") // Spellsettingscene에 도달했을 때
        {
            SpellManager = GameObject.Find("SpellCustom_Manager");
        }
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }//여까지 씬 변경될 때 마다 Update 실행 전에 실행 될(start는 한번만 실행되므로)것들 실행시키는 함수임
    public void StartBattle(int serialNo) //startbutton(Stage)에서 serialNumber 받아올 함수
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
        if (currentState == state.battle && currentStageCleared == true) //배틀중인데 적을 쓰러트림(clear)
        {
            if (eventName == null) // 대화 event를 받지 않았다면
                currentState = state.idle; // 그냥 idle scene으로 
            else //받았다면
                currentState = state.dialogue;
            previousSerialNumber = currentStageSerialNumber;
            currentStageSerialNumber = 0;
            sceneName = "Dialogue_Scene";
            SceneManager.LoadScene("Loading");
        }
        if (currentState == state.idle && currentStageCleared == true && __scene.name == "Dialogue_Scene") //배틀 종료후 idle scene으로 돌아오자 마자 실행될 내용들
        {
            if (previousSerialNumber != 0 && MapManager != null)
            {
                memorizeClearedStage = previousSerialNumber;
                MapManager.GetComponent<MapManagement>().StageClear(previousSerialNumber);
                currentStageCleared = false;//스테이지 선택되지 않은 상태로
                previousSerialNumber = 0;
            }
        }
        if (currentState == state.idle && currentStageSerialNumber != 0) //버튼이 클릭되어서 시리얼 넘버가 gameManager에 입력되었을 때 -> 배틀 씬으로 넘어간다
        {
            currentState = state.battle;
            //sceneName = "TestingBattle"; //이거 배틀 씬 이름으로
            sceneName = "BattleScene";
            SceneManager.LoadScene("Loading");
            Debug.Log("Start Battle!");
        }
        if (currentState == state.idle && isButtonClickedInIdle == true && __scene.name == "Dialogue_Scene") //spellsetting 버튼이 눌렸을 때
        {
            isButtonClickedInIdle = false;
            currentState = state.spell_setting;
            //지금까지 클리어 한 스테이지 정보 가지고 있을 것
            previousSerialNumber = memorizeClearedStage;
            sceneName = "Spell_Custom_Scene";
            SceneManager.LoadScene("Loading");
        }
        if (currentState == state.spell_setting && isButtonClickedInIdle == true && __scene.name == "Spell_Custom_Scene") //spellsetting 씬에서 지도 버튼이 눌렸을 때
        {
            isButtonClickedInIdle = false;
            currentState = state.idle;
            currentStageCleared = true;
            sceneName = "Dialogue_Scene";
            SceneManager.LoadScene("Loading");
        }
    }

    public void IdleSceneChange() // spell setting < - > idle 간 scene 전환을 위해 사용
    {
        isButtonClickedInIdle = true;
    }

    public void SetSpellBook(int[] _spellset) // spell setting scene에서 정보를 변경할 때 사용
    {                                         
        spellList = _spellset;
    }
    public void SetSpellBook(int setCount, int spellId)// 필요하다면 원소 각각을 변환하여도 상관 없다.
    {
        spellList[setCount] = spellId;
    }

    public int[] GetSpellSet() // spell setting or battle scene에서 현재의 spell 배치 정보를 가져올 때 사용
    {
        return spellList;
    }

    public void IsStageFailed() // 스테이지 도전에 실패했을 때 호출받을 함수
    {
        currentStageSerialNumber /= 3; // ex> 5스테이지면 1로 변환
        currentStageSerialNumber *= 3; // ex> 위에서변환한 1을 3으로 변환 --> 3stage까지 클리어 한 것으로 처리
        eventName = null; // 도전한 스테이지에 대화가 있었을 수 있으므로
        currentStageCleared = true; // scene을 Idle Scene으로 변환할 수 있도록
    }
}
