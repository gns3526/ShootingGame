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

    public void StartExplosion(int targetCode)
    {
        ani.SetTrigger("Active");

        switch (targetCode)
        {
            case 0:
                transform.localScale = Vector3.one * 1;
                break;
            case 1:
                transform.localScale = Vector3.one * 0.7f;
                break;
            case 2:
                transform.localScale = Vector3.one * 1;
                break;
            case 3:
                transform.localScale = Vector3.one * 2;
                break;
            case 4:
                transform.localScale = Vector3.one * 3;
                break;
        }
    }
}
