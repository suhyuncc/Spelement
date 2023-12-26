using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
//gh
public class MapManagement : MonoBehaviour
{
    [SerializeField]
    private GameObject[] stages;//�밡�ٷ� stage ������� ����
    [SerializeField]
    private GameObject mapPanel;

    private SaveData1 saveData = new SaveData1();

    private string SAVE_DATA_DIRECTORY;  // ������ ���� ���
    private string SAVE_FILENAME = "/SaveFile.txt"; // ���� �̸�

    void Start()
    {
        SAVE_DATA_DIRECTORY = Application.dataPath + "/Save/";

        if (!Directory.Exists(SAVE_DATA_DIRECTORY)) // �ش� ��ΰ� �������� �ʴ´ٸ�
            Directory.CreateDirectory(SAVE_DATA_DIRECTORY); // ���� ����(��� ����)

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
        int count = 0;//��ü������ �ݺ��� �ȿ��� ������ ���� ����

        //stageNo�� 0�̸� Ŭ����� ���������� �������� �ʴ� �������� �Լ�����
        if (stageNo == 0)
        {
            return;
        }

        for (int i = 0; i < stages.Length; i++)
        {
            //stageNo�� ���� ���������� Ŭ���� ó��
            if (count < stageNo)
            {
                stages[i].GetComponent<StageButton>().StageCleared(); // GameManager���� �޾ƿ� serialNumber�� clear�� stage�� ã�� stage Ŭ���� ����
                count++;
            }
        } 
    }

    public void SaveData()
    {
        saveData.saveStage = GameManager.instance.memorizeClearedStage;
        saveData.saveisSaved = true;

        // ���� ����Ʈ ����
        for (int i = 0; i < GameManager.instance.spell_list.Length; i++)
        {
            saveData.saveList.Add(GameManager.instance.spell_list[i]);
        }

        // ���� ��ü ����
        string json = JsonUtility.ToJson(saveData); // ���̽�ȭ

        File.WriteAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME, json);
    }
}
