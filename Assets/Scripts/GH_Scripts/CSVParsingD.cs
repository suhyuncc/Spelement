using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//gh
public class CSVParsingD : MonoBehaviour
{
    [SerializeField]
    private TextAsset csvFile = null;
    private static Dictionary<string, DialogueData[]> dialogueDict = new Dictionary<string, DialogueData[]>();
    static bool isFirstOn = true;
    public static DialogueData[] GetDialogue(string eventName)
    {
        return dialogueDict[eventName];
    }

    public void SetDict()
    {
        string csvText = csvFile.text.Substring(0, csvFile.text.Length - 1); //get csv file as string type, except last line(empty)
        string[] row = csvText.Split(new char[] { '\n' }); //split by enter sign

        for(int i = 1; i < row.Length; i++)
        {
            string[] data = row[i].Split(new char[] { ',' }); //split by (,)

            if (data[0].Trim() == "" || data[0].Trim() == "end") continue; //no event -> continue

            List<DialogueData> dataList = new List<DialogueData>();
            string eventName = data[0];

            while (data[0].Trim() != "end")
            {
                List<string> contextList = new List<string>();

                DialogueData dialogueData;
                dialogueData.name = data[2];
                dialogueData.speakerType = Int32.Parse(data[1]);
                do
                {
                    contextList.Add(data[3].ToString());
                    if(++i < row.Length)
                    {
                        data = row[i].Split(new char[] { ',' });
                    }
                    else break;
                } while (data[1] == "" && data[0] != "end");

                dialogueData.dialogue_Context= contextList.ToArray();
                dataList.Add(dialogueData);
            }
            dialogueDict.Add(eventName, dataList.ToArray());
        }
    }

    private void Awake()
    {
        if (isFirstOn == true)
        {
            isFirstOn = false;
            SetDict();
        }
    }
}
