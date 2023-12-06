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
}
