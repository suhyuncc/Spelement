using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fortune_Box : MonoBehaviour
{
    [SerializeField]
    private Text name;
    [SerializeField]
    private Text dis;
    [SerializeField]
    private Sprite[] sprites;
    [SerializeField]
    private Image spriteRenderer;

    private void OnEnable()
    {
        int index = BattleManager.instance.F_list[fortune.instance.Show_F_index];

        spriteRenderer.sprite = sprites[index / 6];
        name.text = fortune.instance.Name[index];
        dis.text = fortune.instance.discription[index];
    }
}
