using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//gh
public class MapManagement : MonoBehaviour
{
    [SerializeField]
    private GameObject[] stages;//�밡�ٷ� stage ������� ����
    [SerializeField]
    private GameObject mapPanel;

    public void GostartScene()
    {
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
}
