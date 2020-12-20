using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] Animator ani;

    private void OnEnable()
    {
        Invoke("Disable", 2f);
    }
    void Disable()
    {
        gameObject.SetActive(false);
    }

    public void StartExplosion(string targetName)
    {
        ani.SetTrigger("Active");

        switch (targetName)
        {
            case "Player":
                transform.localScale = Vector3.one * 1;
                break;
            case "S":
                transform.localScale = Vector3.one * 0.7f;
                break;
            case "M":
                transform.localScale = Vector3.one * 1;
                break;
            case "L":
                transform.localScale = Vector3.one * 2;
                break;
            case "Boss0":
                transform.localScale = Vector3.one * 3;
                break;
        }
    }
}
