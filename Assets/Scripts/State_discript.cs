using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class State_discript : MonoBehaviour
{
    [SerializeField]
    private Text name;
    [SerializeField]
    private Text dis;

    private void OnEnable()
    {
        name.text = BattleManager.instance.State_Name[BattleManager.instance.state_id];
        dis.text = BattleManager.instance.State_discription[BattleManager.instance.state_id];
    }
}
