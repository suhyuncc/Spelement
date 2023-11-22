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

    private string sceneName;
    private string eventName;

    void OnEnable()//��������
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)//����ٰ� ���� ��
    {
        Scene _scene = SceneManager.GetActiveScene();
        if (currentState == state.idle && _scene.name == "Dialogue_Scene")
        {
            MapManager = GameObject.Find("MapManager");
        }
        else if (currentState == state.dialogue && _scene.name == "Dialogue_Scene")
        {
            DialogueManager = GameObject.Find("DialogueSystem");
            MapManager = GameObject.Find("MapManager");
            MapManager.GetComponent<MapManagement>().SceneChanged();
            DialogueManager.GetComponent<Dialogue_Manage>().GetEventName(eventName);
            eventName = null;
        }
        else if (_scene.name == "Loading")
        {
            LoadingManager = GameObject.Find("LoadManager");
            if(LoadingManager != null)
            {
                LoadingManager.GetComponent<LoadingScene>().GetSceneName(sceneName);
                sceneName = null;
            }
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
    public void GetEventName(string _eventName)
    {
        eventName = _eventName;
    }

    Scene __scene;
    private void Update()
    {
        __scene = SceneManager.GetActiveScene();
        if(currentState == state.battle && currentStageCleared == true) //��Ʋ���ε� ���� ����Ʈ��(clear)
        {
            if(eventName == null)
                currentState = state.idle;
            else 
                currentState = state.dialogue;
            previousSerialNumber= currentStageSerialNumber;
            currentStageSerialNumber = 0;
            sceneName = "Dialogue_Scene";
            SceneManager.LoadScene("Loading");
        }
        if (currentState == state.idle && currentStageCleared == true && __scene.name == "Dialogue_Scene") //��Ʋ ������ idle scene���� ���ƿ��� ���� ����� �����
        {
            if (previousSerialNumber != 0 && MapManager != null)
            {
                MapManager.GetComponent<MapManagement>().StageClear(previousSerialNumber);
                currentStageCleared = false;//�������� ���õ��� ���� ���·�
                previousSerialNumber = 0;
            }
        }
        if(currentState == state.idle && currentStageSerialNumber != 0) //��ư�� Ŭ���Ǿ �ø��� �ѹ��� gameManager�� �ԷµǾ��� �� -> ��Ʋ ������ �Ѿ��
        {
            currentState = state.battle;
            sceneName = "TestingBattle"; //�̰� ��Ʋ �� �̸�����
            SceneManager.LoadScene("Loading");
            Debug.Log("Start Battle!");
        }
    }
}
