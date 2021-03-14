using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinLock : MonoBehaviour
{
    GameManager gm;
    [SerializeField] int unLockLv;

    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
    }

    private void OnEnable()
    {
        UnLock();
    }

    public void UnLock()
    {
        if(gm.playerLv >= unLockLv)
            transform.GetChild(1).gameObject.SetActive(false);
        else
            transform.GetChild(1).gameObject.SetActive(true);
    }
}
