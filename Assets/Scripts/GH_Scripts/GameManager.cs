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
            //매번 Idle scene에 도달했을 때 진행도에따른 맵 세팅
            MapManager.GetComponent<MapManagement>().StageClear(memorizeClearedStage);
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
        else if (currentState == state.battle && _scene.name == "BattleScene") // BattleScene에 도달했을 때
        {
            BattleManager = GameObject.Find("BattleManager");
            SkillManager = GameObject.Find("SkillManager");
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

    //startbutton(Stage)에서 serialNumber 받아오며 battle씬으로 전환
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

        //dialogue 씬 -> spellsetting 씬
        if (currentState == state.idle && isButtonClickedInIdle == true && __scene.name == "Dialogue_Scene")
        {
            isButtonClickedInIdle = false;
            currentState = state.spell_setting;
            sceneName = "Spell_Custom_Scene";
            SceneManager.LoadScene("Loading");
        }

        //spellsetting 씬 -> dialogue 씬
        if (currentState == state.spell_setting && isButtonClickedInIdle == true && __scene.name == "Spell_Custom_Scene") 
        {
            isButtonClickedInIdle = false;
            currentState = state.idle;
            sceneName = "Dialogue_Scene";
            SceneManager.LoadScene("Loading");
        }

        //battle 씬 -> dialogue 씬
        if (currentState == state.battle && isButtonClickedInIdle == true && __scene.name == "BattleScene")
        {
            if (eventName == null)
                currentState = state.idle;
            else
                currentState = state.dialogue;

            isButtonClickedInIdle = false;
            sceneName = "Dialogue_Scene";
            SceneManager.LoadScene("Loading");

            if (currentStageCleared)//클리어시 숫자변경
            {
                memorizeClearedStage = currentStageSerialNumber;
                
                currentStageCleared = false;//스테이지 선택되지 않은 상태로
            }

            currentStageSerialNumber = 0;
        }
    }

    public void IdleSceneChange()
    {
        isButtonClickedInIdle = true;
    }
}
