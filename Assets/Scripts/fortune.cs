using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class fortune : MonoBehaviour
{
    [SerializeField]
    private TextAsset fortune_info = null;
    [SerializeField]
    private Sprite[] fortune_sprites;
    [SerializeField]
    private SpriteRenderer[] sprites;
    [SerializeField]
    private Text[] names;
    [SerializeField]
    private Text[] discriptions;

    private string[] Name;
    private string[] discription;

    private void OnEnable()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        Set_Fortune_Data();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Set_Fortune_Data()
    {
        string csvText = fortune_info.text.Substring(0, fortune_info.text.Length - 1); //get csv file as string type, except last line(empty)
        string[] row = csvText.Split(new char[] { '\n' }); //split by enter sign

        Name = new string[row.Length - 1];
        discription = new string[row.Length - 1];

        for (int i = 1; i < row.Length; i++)
        {
            string[] data = row[i].Split(new char[] { ',' }); //split by (,)

            Name[i - 1] = data[2];
            discription[i - 1] = data[3];
           
        }

    }
}
