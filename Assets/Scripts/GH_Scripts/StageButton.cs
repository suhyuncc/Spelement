using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
//gh
public class StageButton : MonoBehaviour
{
    public bool isCleared = false; // ���� ���������� Ŭ����� �������� üũ�ϴ� ����
    public bool isChecked = false; // Ŭ���� �� ���������� üũ�Ǿ����� üũ�ϴ� ����
    public int stageSerialNumber = 1; // �� ���������� �ø��� �ѹ��� �����ϴ� ����
    [SerializeField]
    private GameObject previousStage; //������ stage�� �����ϴ� �Լ�
    [SerializeField]
    private bool next_Dialogue;
    [SerializeField]
    private bool prev_Dialogue;

    public string dialogueEventName = null;
    
    private GameObject GM; //GameManager�� �Ҵ�޴� ����
    private void Update()
    {
        if (isChecked == false && isCleared == true) //���������� Ŭ���� �Ǿ����� üũ�� ���� �ʾ��� ��
        {
            isChecked = true;//üũ true
            this.transform.GetChild(0).gameObject.SetActive(true);//�̹��� Ȱ��ȭ
        }else if (previousStage != null 
            && previousStage.GetComponent<StageButton>().isCleared == true && isCleared == false)
        {
            this.transform.GetChild(1).gameObject.SetActive(true);
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
            GM = GameObject.Find("GameManager"); //GameManager�� ã�Ƽ�
            if (next_Dialogue) // ���� ���� ��ȭ�� ����� ��
            {
                GM.GetComponent<GameManager>().SetEventName(dialogueEventName);
                GM.GetComponent<GameManager>().StartBattle(stageSerialNumber); //StartBattle!
            }
            else if(prev_Dialogue) // ���� ���� ��ȭ�� ������ �� ��
            {
                GM.GetComponent<GameManager>().DialogueManager.GetComponent<Dialogue_Manage>().ItIsPreviousDialogue(stageSerialNumber);
                GM.GetComponent<GameManager>().MapManager.GetComponent<MapManagement>().SceneChanged();
                GM.GetComponent<GameManager>().DialogueManager.GetComponent<Dialogue_Manage>().GetEventName(dialogueEventName);
            }
            else
            {
                GM.GetComponent<GameManager>().SetEventName(null);
                GM.GetComponent<GameManager>().StartBattle(stageSerialNumber); //StartBattle!
            }
        }
    }
}
