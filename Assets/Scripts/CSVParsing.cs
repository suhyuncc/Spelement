using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSVParsing : MonoBehaviour
{
    [SerializeField]
    private TextAsset csvFile = null;

    public string[] Name;
    public int[] Null;
    public int[] Air;
    public int[] Earth;
    public int[] Water;
    public int[] Fire;
    public int[] number;
    public string[] discription;


    public void SetData()
    {
        string csvText = csvFile.text.Substring(0, csvFile.text.Length - 1); //get csv file as string type, except last line(empty)
        string[] row = csvText.Split(new char[] { '\n' }); //split by enter sign

        Name = new string[row.Length -1];
        Null = new int[row.Length -1];
        Air = new int[row.Length - 1];
        Earth = new int[row.Length - 1];
        Water = new int[row.Length - 1];
        Fire = new int[row.Length - 1];
        number = new int[row.Length - 1];
        discription = new string[row.Length - 1];

        for (int i = 1; i < row.Length; i++) {
            string[] data = row[i].Split(new char[] { ',' }); //split by (,)

            Name[i-1] = data[0];
            Null[i - 1] = int.Parse(data[1]);
            Air[i - 1] = int.Parse(data[2]);
            Earth[i - 1] = int.Parse(data[3]);
            Water[i - 1] = int.Parse(data[4]);
            Fire[i - 1] = int.Parse(data[5]);
            number[i - 1] = int.Parse(data[6]);
            discription[i - 1] = data[7];
        }

    }

    private void Awake()
    { 
        SetData();
    }
}
