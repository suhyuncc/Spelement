using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class startsceneManager : MonoBehaviour
{
    public GameObject WarningPanel;
    public Button contine;
    public SaveData Save_data;

    private SaveData1 saveData = new SaveData1();

    private string SAVE_DATA_DIRECTORY;  // 저장할 폴더 경로
    private string SAVE_FILENAME = "/SaveFile.txt"; // 파일 이름

    private GameObject gm;

    private void Start()
    {
        gm = GameObject.Find("GameManager");
        SAVE_DATA_DIRECTORY = Application.dataPath + "/Save/";

        if (File.Exists(SAVE_DATA_DIRECTORY + SAVE_FILENAME))
        {
            // 전체 읽어오기
            string loadJson = File.ReadAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME);
            saveData = JsonUtility.FromJson<SaveData1>(loadJson);

            Save_data.isSaved = saveData.saveisSaved;
        }
    }

    private void Update()
    {
        if (!Save_data.isSaved)
        {
            contine.interactable = false;
        }
        else
        {
            contine.interactable = true;
        }
    }

    public void Startbtn()
    {

        if (gm != null)
        {
            gm.GetComponent<GameManager>().IdleSceneChange();
        }
    }

    public void PlayMainScene()
    {
        if (!Save_data.isSaved)
        {
            NewStart();
        }
        else
        {
            WarningPanel.SetActive(true);
        }

    }

    public void ContinueScene()
    {
        if (Save_data.isSaved)
        {
            //이어서 하기
            LoadData();
            Startbtn();
        }

    }

    public void NewStart()
    {
        GameManager.instance.memorizeClearedStage = 0;
        Save_data.isSaved = true;
        gm.GetComponent<GameManager>().spell_list = new int[3] { 0, 1, 2 };
        gm.GetComponent<GameManager>().FirstGameStart();
        Startbtn();
    }

    public void FinBtn()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void LoadData()
    {
        if (File.Exists(SAVE_DATA_DIRECTORY + SAVE_FILENAME))
        {
            // 전체 읽어오기
            string loadJson = File.ReadAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME);
            saveData = JsonUtility.FromJson<SaveData1>(loadJson);

            GameManager.instance.memorizeClearedStage = saveData.saveStage;
            Save_data.isSaved = saveData.saveisSaved;

            GameManager.instance.spell_list = new int[saveData.saveList.Count];
            // spell 로드
            for (int i = 0; i < saveData.saveList.Count; i++)
            {
                GameManager.instance.spell_list[i] = saveData.saveList[i];
            }
                
                //GameManager.instance.spell_list[i] = saveData.saveList[i];
        }
        else
            Debug.Log("세이브 파일이 없습니다.");
    }
}
