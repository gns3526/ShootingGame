using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Explosion : MonoBehaviourPun
{
    [SerializeField] Animator ani;
    ObjectPooler OP;

    Vector3 curPosPv;

    private void OnEnable()
    {
        transform.position = curPosPv;

        OP = FindObjectOfType<ObjectPooler>();

        ani.SetTrigger("Active");
    }
    void Disable()
    {
        curPosPv = new Vector3(16, 16, 0);
        OP.PoolDestroy(gameObject);
    }

    public void ExplosionSound()
    {
        SoundManager.Play("Explosion_1");
    }
}
