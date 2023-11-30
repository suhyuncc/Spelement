using UnityEngine;
//gh
[System.Serializable]
public struct DialogueData
{
    public string name; //화자
    public string[] dialogue_Context; // 대화 내용
    public int speakerType; // 현재 대화중인 대상 ... 현재 딱히 필요없어보임 아마, 오브젝트 
}
public class Dialogue : MonoBehaviour
{
    [SerializeField] string eventName; //현재 진행중인 대화 종류

    [SerializeField] string[] dialogue_Data;
}
