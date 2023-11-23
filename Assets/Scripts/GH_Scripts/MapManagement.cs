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
        for (int i = 0; i < stages.Length; i++)
        {
            if(stages[i].GetComponent<StageButton>().stageSerialNumber == stageNo)
            {
                stages[i].GetComponent<StageButton>().StageCleared(); // GameManager���� �޾ƿ� serialNumber�� clear�� stage�� ã�� stage Ŭ���� ����
            }
        } 
    }
}
