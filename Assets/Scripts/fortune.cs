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
    private Image[] images;
    [SerializeField]
    private Text[] names;
    [SerializeField]
    private Text[] discriptions;

    private string[] Name;
    private string[] discription;

    public int[] test = new int[18];

    private void OnEnable()
    {
        set_Fortune();
    }

    // Start is called before the first frame update
    void Awake()
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

            Name[i - 1] = data[3];
            discription[i - 1] = data[4];
           
        }

    }

    private void set_Fortune()
    {
        int ran1 = Random.Range(0, 18);
        int ran2 = Random.Range(0, 18);
        int ran3 = Random.Range(0, 18);

        if(ran1 != ran2 &&  ran3 != ran1 && ran2 != ran3) {

            images[0].sprite = fortune_sprites[ran1 / 6];
            images[1].sprite = fortune_sprites[ran2 / 6];
            images[2].sprite = fortune_sprites[ran3 / 6];

            names[0].text = Name[ran1];
            names[1].text = Name[ran2];
            names[2].text = Name[ran3];

            discriptions[0].text = discription[ran1];
            discriptions[1].text = discription[ran2];
            discriptions[2].text = discription[ran3];

            test[ran1]++;
            test[ran2]++;
            test[ran3]++;
        }
        else
        {
            set_Fortune();
            return;
        }
    }
}
