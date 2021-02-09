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
        Invoke("Disable", 2f);
    }
    void Disable()
    {
        curPosPv = new Vector3(16, 16, 0);
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
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)//isMine = true
        {
            stream.SendNext(transform.position);
        }
        else
        {
            curPosPv = (Vector3)stream.ReceiveNext();
        }
    }
}
