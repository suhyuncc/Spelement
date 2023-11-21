using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.SceneManagement;
//gh
public class StageButton : MonoBehaviour
{
    public bool isCleared = false; // 현재 스테이지가 클리어된 상태인지 체크하는 변수
    public bool isChecked = false; // 클리어 된 스테이지가 체크되었는지 체크하는 변수
    public int stageSerialNumber = 1; // 각 스테이지의 시리얼 넘버를 저장하는 변수
    [SerializeField]
    private GameObject previousStage; //이전의 stage를 저장하는 함수
    
    private GameObject GM; //GameManager를 할당받는 변수
    private void Update()
    {
        if(isChecked == false && isCleared == true ) //스테이지가 클리어 되었지만 체크는 되지 않았을 때
        {
            isChecked = true;//체크 true
           this.transform.GetChild(0).gameObject.SetActive(true);//이미지 활성화
        }
    }


    public void StageCleared() //MapManagement에서 호출되는 함수
    {
        isCleared = true;// 클리어
        if(previousStage!= null) // 전 스테이지가 존재한다면
        {
            previousStage.GetComponent<StageButton>().StageCleared(); //전 스테이지의 함수 재귀
        }
    }
     public void StageStart() //버튼 클릭시 호출되는 함수
    {
        if (isCleared == false) // isCleared(클리어 여부)가 false일 때에만 실행될 것
        {
            if (previousStage.GetComponent<StageButton>().isCleared == true) // 직전 스테이지가 클리어 되었을 때에만 실행될 것
            {
                GM = GameObject.Find("GameManager"); //GameManager를 찾아서
                GM.GetComponent<GameManager>().StartBattle(stageSerialNumber); //StartBattle!
            }
        }
    }
}
