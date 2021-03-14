using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinLock : MonoBehaviour
{
    GameManager gm;
    [SerializeField] int unLockLv;
    [SerializeField] int kind;

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
        if(kind == 0)
        {
            if (gm.playerLv >= unLockLv)
                transform.GetChild(1).gameObject.SetActive(false);
            else
                transform.GetChild(1).gameObject.SetActive(true);
        }
        if(kind == 1)
        {
            if (gm.playerLv >= unLockLv)
                transform.GetChild(1).gameObject.SetActive(false);
            else
                transform.GetChild(1).gameObject.SetActive(true);
        }
    }
}
