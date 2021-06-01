using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JobChangeScript : MonoBehaviour
{
    [SerializeField] GameManager gm;

    [SerializeField] Image cardImage;
    [SerializeField] Sprite[] jobSprites;

    [SerializeField] Text jobNameText;
    [SerializeField] Text jobInfoText;
    [SerializeField] Text skillInfoText;

    [SerializeField] string[] jobName;
    [SerializeField] string[] jobInfo;
    [SerializeField] string[] skillInfo;

    int jobIndex;
    private void OnEnable()
    {
        jobIndex = gm.jm.jobCode;

        cardImage.sprite = jobSprites[jobIndex];
        jobNameText.text = jobName[jobIndex];
        jobInfoText.text = jobInfo[jobIndex];
        skillInfoText.text = skillInfo[jobIndex];
    }

    public void JobChange(int index)
    {
        jobIndex += index;

        if (jobIndex == 5) jobIndex = 0;
        else if (jobIndex == -1) jobIndex = 4;

        cardImage.sprite = jobSprites[jobIndex];
        jobNameText.text = jobName[jobIndex];
        jobInfoText.text = jobInfo[jobIndex];
        skillInfoText.text = skillInfo[jobIndex];
    }

    public void JobSelect()
    {
        gm.jm.jobCode = jobIndex;
    }
}
