using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] Animator ani;
    ObjectPooler OP;

    private void Start()
    {

    }
    private void OnEnable()
    {
        OP = FindObjectOfType<ObjectPooler>();
        ani.SetTrigger("Active");
        Invoke("Disable", 2f);
    }
    void Disable()
    {
        OP.PoolDestroy(gameObject);
    }

    public void StartExplosion(string monsterType)
    {


        switch (monsterType)
        {
            case "Player":
                transform.localScale = Vector3.one * 1;
                break;
            case "monster1":
                transform.localScale = Vector3.one * 0.7f;
                break;
            case "monster2":
                transform.localScale = Vector3.one * 1;
                break;
            case "monster3":
                transform.localScale = Vector3.one * 2;
                break;
            case "Boss1":
                transform.localScale = Vector3.one * 3;
                break;
        }
    }
}
