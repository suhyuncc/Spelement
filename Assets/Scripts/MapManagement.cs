using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//gh
public class MapManagement : MonoBehaviour
{
    [SerializeField]
    private GameObject[] stages;//노가다로 stage 집어넣을 것임
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
                stages[i].GetComponent<StageButton>().StageCleared(); // GameManager에서 받아온 serialNumber로 clear한 stage를 찾아 stage 클리어 판정
            }
        } 
    }
}
