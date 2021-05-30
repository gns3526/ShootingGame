using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JobChangeScript : MonoBehaviour
{
    [SerializeField] GameManager gm;

    [SerializeField] SpriteRenderer cardSpriteRender;
    [SerializeField] Sprite[] jobSprites;

    [SerializeField] Text jobNameText;
    [SerializeField] Text jobInfoText;

    [SerializeField] string[] jobName;
    [SerializeField] string[] jobInfo;

    int jobIndex;
    private void OnEnable()
    {
        jobIndex = gm.jm.jobCode;

        cardSpriteRender.sprite = jobSprites[jobIndex];
        jobNameText.text = jobName[jobIndex];
        jobInfoText.text = jobInfo[jobIndex];
    }

    void JobChange(int index)
    {
        jobIndex += index;

        if (jobIndex == 5) jobIndex = 0;
        else if (jobIndex == -1) jobIndex = 4;

        cardSpriteRender.sprite = jobSprites[jobIndex];
        jobNameText.text = jobName[jobIndex];
        jobInfoText.text = jobInfo[jobIndex];
    }

    void JobSelect()
    {
        gm.jm.jobCode = jobIndex;
    }
}
