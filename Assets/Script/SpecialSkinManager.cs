﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpecialSkinManager : MonoBehaviour
{
    public bool[] challenge;
    public Sprite[] challengeImage;
    public string[] challengeInfo;

    [SerializeField] private GameObject successPanel;
    bool successPanelActive;

    [SerializeField] private Animator animator;

    public List<int> achiveCode;

    public void ChallengeClear(int index)
    {
        if (challenge[index]) return;

        achiveCode.Add(index);

        if(!successPanelActive)
        {
            Challenge();
            successPanelActive = true;
        }
    }

    public void Challenge()
    {
        if (achiveCode.Count > 0)
        {
            challenge[achiveCode[0]] = true;
            successPanel.transform.GetChild(0).GetComponent<Image>().sprite = challengeImage[achiveCode[0]];
            successPanel.transform.GetChild(2).GetComponent<Text>().text = challengeInfo[achiveCode[0]];

            animator.SetTrigger("Active");

            achiveCode.Remove(achiveCode[0]);

            StartCoroutine(Delay());
        }
        else
            successPanelActive = false;
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(4);
        Challenge();
    }
}
