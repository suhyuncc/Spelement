using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastStageStart : MonoBehaviour
{
    [SerializeField]
    private GameObject KingStage;

    private void Update()
    {
        if (this.gameObject.GetComponent<StageButton>().isCleared)
        {
            KingStage.SetActive(true);
        }
    }
}
