using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue_Manage : MonoBehaviour
{
    public string eventName; // eventName ���� ���� ��
    [SerializeField]
    private TMP_Text nameText; //ȭ��
    [SerializeField]
    private TMP_Text contextText; //��ȭ
    [SerializeField]
    private GameObject endTriangle; //������ �����Ÿ��� �ﰢ��

    private bool isDialogue = false; // recieve event
    private bool currentDialogue = false; // is current dialogue working

    private DialogueData[] dialogueData; //��ȭ ������
    [SerializeField]
    private GameObject dialoguePanel; //��ȭpanel
    [SerializeField]
    private GameObject dialogueImagePanel; //Image���� ����Ǿ��ִ� �г�
    public void GetEventName(string _eventName) //eventName ���ɹ޴� �Լ�
    {
        eventName = _eventName;
        isDialogue = true;
        dialoguePanel.SetActive(true);
    }
    
    private int dataIndex = 0;
    private int contextIndex = 0;

    private int previousImage = 0;

    private bool currentTypeEnd = false;
    private string toType = null;

    private IEnumerator typingText;
    private void Update()
    {
        if(isDialogue)
        {
            isDialogue = false;
            currentDialogue= true;
            dialogueData = CSVParsingD.GetDialogue(eventName); // ȭ�� Ÿ��, ȭ�� �̸�, ��縦 ���ϴ� �̺�Ʈ�� �ִ� ������ ������
            endTriangle.SetActive(false);
            nameText.text = dialogueData[0].name;
            if (dialogueImagePanel.transform.GetChild(dialogueData[0].speakerType).gameObject.activeSelf == false)
            {
                Debug.Log(dialogueData[0].speakerType);
                previousImage = dialogueData[0].speakerType;
                dialogueImagePanel.transform.GetChild(previousImage).gameObject.SetActive(true);
            }
            dataIndex = 0;
            contextIndex = 0;
            currentTypeEnd = false;
            typingText = TypingMotion();
            toType = dialogueData[dataIndex].dialogue_Context[contextIndex];
            StartCoroutine(typingText);
        }//EventName�� �޾ƿ��� ��

        if(currentDialogue)
        {
            if(Input.GetMouseButtonDown(0))
            {
                if (currentTypeEnd)
                {
                    if (contextIndex < dialogueData[dataIndex].dialogue_Context.Length - 1)
                    {
                        contextIndex++;
                        currentTypeEnd= false;
                        typingText = TypingMotion();
                        endTriangle.SetActive(false);
                        toType = dialogueData[dataIndex].dialogue_Context[contextIndex];
                        StartCoroutine(typingText);
                    }
                    else if (contextIndex >= dialogueData[dataIndex].dialogue_Context.Length - 1)
                    {
                        contextIndex = 0;
                        dataIndex++;
                        if (dataIndex < dialogueData.Length)
                        {
                            nameText.text = dialogueData[dataIndex].name;
                            currentTypeEnd = false;
                            typingText = TypingMotion();
                            endTriangle.SetActive(false);
                            toType = dialogueData[dataIndex].dialogue_Context[contextIndex];

                            if (dialogueImagePanel.transform.GetChild(dialogueData[dataIndex].speakerType).gameObject.activeSelf == false)
                            {
                                Debug.Log(dialogueData[dataIndex].speakerType);
                                GameObject _prevImage = dialogueImagePanel.transform.GetChild(previousImage).gameObject;
                                Color _prevColor = _prevImage.GetComponent<Image>().color;
                                _prevColor.a = 0.3f;
                                _prevImage.GetComponent<Image>().color = _prevColor; //������� ���İ� �ٱ��ְ�---------
                                previousImage = dialogueData[dataIndex].speakerType;
                                dialogueImagePanel.transform.GetChild(previousImage).gameObject.SetActive(true);
                            }
                            else
                            {
                                GameObject _prevImage = dialogueImagePanel.transform.GetChild(previousImage).gameObject;
                                Color _prevColor = _prevImage.GetComponent<Image>().color;
                                _prevColor.a = 0.3f;
                                _prevImage.GetComponent<Image>().color = _prevColor;
                                previousImage = dialogueData[dataIndex].speakerType;
                                _prevImage = dialogueImagePanel.transform.GetChild(previousImage).gameObject;
                                _prevColor = _prevImage.GetComponent<Image>().color;
                                _prevColor.a = 1.0f;
                                _prevImage.GetComponent<Image>().color = _prevColor;
                            }

                            StartCoroutine(typingText); ;
                        }
                        else
                        {
                            currentDialogue = false;
                            GameObject gm = GameObject.Find("GameManager");
                            gm.GetComponent<GameManager>().currentState = state.idle;
                            gm.GetComponent<GameManager>().MapManager.GetComponent<MapManagement>().ReturnScene();
                            dialoguePanel.SetActive(false); //Dialogue UI
                        }
                    }
                }
                else
                {
                    StopCoroutine(typingText);
                    contextText.text = toType;
                    currentTypeEnd = true;
                    endTriangle.SetActive(true);
                }
            }
        }
    }
    IEnumerator TypingMotion()
    {
        contextText.text = null;
        for(int i = 0; i < toType.Length; i++)
        {
            contextText.text+= toType[i];
            yield return new WaitForSeconds(0.2f);
        }
        currentTypeEnd = true;
        endTriangle.SetActive(true);
        yield break;
    }
}
