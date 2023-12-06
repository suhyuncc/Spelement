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
    //start씬에서 시작해야 되므로 상태 start로 지정
    public state currentState = state.start;//stateManagement script에서 작성된 variable type 어짜피 GameManager에서 관리하므로 건드려줄 필요 없음
    public int currentStageSerialNumber = 0;//이것도 StageButton에서 가져올거고 노가다로 스테이지마다 설정할거라 건드려줄 필요 없음
    //시리얼 넘버(몬스터 시리얼 넘버)는 1부터 시작됨
    public bool currentStageCleared = false;//battle 승리하면 true로 바까줘요 아님 false 상태로 scene 바까줘요

    public GameObject MapManager;//MapManager를 찾을 GameObject
    public GameObject DialogueManager; //DialogueManager를 찾을 GameObject
    public GameObject LoadingManager;
    public GameObject SpellManager;

    public int[] spell_list;

    private string sceneName;
    private string eventName;

    [SerializeField]
    private bool isButtonClickedInIdle = false;
    public int memorizeClearedStage;

    [SerializeField]
    private int[] spellList = new int[12] {1,2,3,0,0,0,0,0,0,0,0,0}; //나중에 첫 3 spell id에 맞게 수정할 것
    
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
    void OnEnable()//여서부터
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)//여기다가 넣을 것
    {
        Scene _scene = SceneManager.GetActiveScene();
        if (currentState == state.idle && _scene.name == "Dialogue_Scene") //Idle scene에 도달했을 때
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
            //매번 Idle scene에 도달했을 때 진행도에 따른 맵 세팅
            MapManager.GetComponent<MapManagement>().StageClear(memorizeClearedStage);
        }
        else if (currentState == state.dialogue && _scene.name == "Dialogue_Scene") // Idle scene but 대화가 선행되어야 할 때
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

    public void SaveandBack()
    {
        //저장기능 추가

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

        //start 씬 -> dialogue 씬
        if (currentState == state.start && isButtonClickedInIdle == true && __scene.name == "Start Scene")
        {
            isButtonClickedInIdle = false;
            currentState = state.idle;
            sceneName = "Dialogue_Scene";
            SceneManager.LoadScene("Loading");
        }

        //battle 씬 -> dialogue 씬
        if (currentState == state.battle && isButtonClickedInIdle == true && __scene.name == "BattleScene")
        {
            isButtonClickedInIdle = false;
            sceneName = "Dialogue_Scene";
            SceneManager.LoadScene("Loading");

            if (currentStageCleared)//클리어시 숫자변경
            {
                memorizeClearedStage = currentStageSerialNumber;
                
                currentStageCleared = false;//스테이지 선택되지 않은 상태로
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

    public void Addspell(int stage_id)
    {
        int[] temp_list = new int[spell_list.Length + 1];
        int spell_id;

        spell_list.CopyTo(temp_list, 0);
        //스테이지 레벨에 따른 클리어시 스펠 자동 추가
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

    public void IsStageFailed() // 스테이지 도전에 실패했을 때 호출받을 함수
    {
        currentStageSerialNumber /= 3; // ex> 5스테이지면 1로 변환
        currentStageSerialNumber *= 3; // ex> 위에서변환한 1을 3으로 변환 --> 3stage까지 클리어 한 것으로 처리
        eventName = null; // 도전한 스테이지에 대화가 있었을 수 있으므로
        currentStageCleared = true; // scene을 Idle Scene으로 변환할 수 있도록
    }
    public int GetStageNumber()
    {
        return memorizeClearedStage/3; //배경화면 띄울 때 쓸 거
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
