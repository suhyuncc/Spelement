using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.SceneManagement;
//gh
public class StageButton : MonoBehaviour
{
    public bool isCleared = false; // ���� ���������� Ŭ����� �������� üũ�ϴ� ����
    public bool isChecked = false; // Ŭ���� �� ���������� üũ�Ǿ����� üũ�ϴ� ����
    public int stageSerialNumber = 1; // �� ���������� �ø��� �ѹ��� �����ϴ� ����
    [SerializeField]
    private GameObject previousStage; //������ stage�� �����ϴ� �Լ�
    
    private GameObject GM; //GameManager�� �Ҵ�޴� ����
    private void Update()
    {
        if(isChecked == false && isCleared == true ) //���������� Ŭ���� �Ǿ����� üũ�� ���� �ʾ��� ��
        {
            isChecked = true;//üũ true
           this.transform.GetChild(0).gameObject.SetActive(true);//�̹��� Ȱ��ȭ
        }
    }


    public void StageCleared() //MapManagement���� ȣ��Ǵ� �Լ�
    {
        isCleared = true;// Ŭ����
        if(previousStage!= null) // �� ���������� �����Ѵٸ�
        {
            previousStage.GetComponent<StageButton>().StageCleared(); //�� ���������� �Լ� ���
        }
    }
     public void StageStart() //��ư Ŭ���� ȣ��Ǵ� �Լ�
    {
        if (isCleared == false) // isCleared(Ŭ���� ����)�� false�� ������ ����� ��
        {
            if (previousStage.GetComponent<StageButton>().isCleared == true) // ���� ���������� Ŭ���� �Ǿ��� ������ ����� ��
            {
                GM = GameObject.Find("GameManager"); //GameManager�� ã�Ƽ�
                GM.GetComponent<GameManager>().StartBattle(stageSerialNumber); //StartBattle!
            }
        }
    }
}
