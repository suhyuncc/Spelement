using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
//gh
public class MapManagement : MonoBehaviour
{
    [SerializeField]
    private GameObject[] stages;//노가다로 stage 집어넣을 것임
    [SerializeField]
    private GameObject mapPanel;

    private SaveData1 saveData = new SaveData1();

    private string SAVE_DATA_DIRECTORY;  // 저장할 폴더 경로
    private string SAVE_FILENAME = "/SaveFile.txt"; // 파일 이름

    void Start()
    {
        SAVE_DATA_DIRECTORY = Application.dataPath + "/Save/";

        if (!Directory.Exists(SAVE_DATA_DIRECTORY)) // 해당 경로가 존재하지 않는다면
            Directory.CreateDirectory(SAVE_DATA_DIRECTORY); // 폴더 생성(경로 생성)

    }

    public void GostartScene()
    {
        SaveData();

        GameObject gm = GameObject.Find("GameManager");

        if (gm != null)
        {
            Time.timeScale = 1;
            gm.GetComponent<GameManager>().SaveandBack();
        }
    }

    public void SceneChanged()
    {
        mapPanel.SetActive(false);
    }
    public void ReturnScene()
    {
        mapPanel.SetActive(true);
    }
    public void StageClear(int stageNo)
    {
        int count = 0;//자체적으로 반복문 안에서 갯수를 새는 변수

        //stageNo가 0이면 클리어된 스테이지가 존재하지 않는 것임으로 함수종료
        if (stageNo == 0)
        {
            return;
        }

        for (int i = 0; i < stages.Length; i++)
        {
            //stageNo에 따라 스테이지를 클리어 처리
            if (count < stageNo)
            {
                stages[i].GetComponent<StageButton>().StageCleared(); // GameManager에서 받아온 serialNumber로 clear한 stage를 찾아 stage 클리어 판정
                count++;
            }
        } 
    }

    public void SaveData()
    {
        saveData.saveStage = GameManager.instance.memorizeClearedStage;
        saveData.saveisSaved = true;

        // 스펠 리스트 저장
        for (int i = 0; i < GameManager.instance.spell_list.Length; i++)
        {
            saveData.saveList.Add(GameManager.instance.spell_list[i]);
        }

        // 최종 전체 저장
        string json = JsonUtility.ToJson(saveData); // 제이슨화

        File.WriteAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME, json);
    }
}
