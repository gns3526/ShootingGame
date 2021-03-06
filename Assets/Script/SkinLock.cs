﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinLock : MonoBehaviour
{
    GameManager gm;
    ChallengeManager challengeManager;
    [SerializeField] int unLockLv;

    [Header("Challenge")]
    [SerializeField] bool isChallenge;
    [SerializeField] int challengeNum;
    

    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
        challengeManager = FindObjectOfType<ChallengeManager>();
    }

    private void OnEnable()
    {
        UnLock();
    }



    public void UnLock()
    {
        if (isChallenge) //challenge
        {
            if (challengeManager.challenge[challengeNum] == true)
                transform.GetChild(1).gameObject.SetActive(false);
            else
                transform.GetChild(1).gameObject.SetActive(true);
        }
        else //Level
        {
            if (gm.playerLv >= unLockLv)
                transform.GetChild(1).gameObject.SetActive(false);
            else
                transform.GetChild(1).gameObject.SetActive(true);
        }
    }
}
