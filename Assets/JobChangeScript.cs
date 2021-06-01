using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JobChangeScript : MonoBehaviour
{
    [SerializeField] GameManager gm;

    [SerializeField] GameObject selectMark;

    [SerializeField] Button selectBtn;

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

        JobChange(0);
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

        if (jobIndex == gm.jm.jobCode)
        {
            selectMark.SetActive(true);
            selectBtn.interactable = false;
        }
        else
        {
            selectMark.SetActive(false);
            selectBtn.interactable = true;
        }
        SoundManager.Play("Btn_1");
    }

    public void JobSelect()
    {
        gm.jm.jobCode = jobIndex;

        JobChange(0);
    }
}
