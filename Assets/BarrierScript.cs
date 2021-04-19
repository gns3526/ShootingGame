using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class BarrierScript : MonoBehaviour
{
    [SerializeField] Player playerScript;

    ObjectPooler op;
    JopManager jm;

    [SerializeField] Text barrierCountText;

    public int barrierCount;

    public PhotonView pv;
    [SerializeField] CircleCollider2D circleCollider;

    private void Start()
    {
        op = playerScript.OP;
        jm = playerScript.JM;
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        if (playerScript.pv.IsMine)
        {
            barrierCountText.text = barrierCount.ToString();
            barrierCountText.enabled = true;

            circleCollider.enabled = true;

            pv.RPC(nameof(BarrierCountUpdate), RpcTarget.All, barrierCount);
        }
        else
        {
            circleCollider.enabled = false;
        }
    }
    private void OnDisable()
    {
        barrierCountText.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!playerScript.pv.IsMine) return;

        if (other.tag == "Bullet" && !other.GetComponent<BulletScript>().isPlayerAttack)
        {
            barrierCount--;
            pv.RPC(nameof(BarrierCountUpdate), RpcTarget.All, barrierCount);
            op.PoolDestroy(other.gameObject);
            if (barrierCount == 0)
                pv.RPC(nameof(BarrierOn), RpcTarget.All, false);
        }
    }

    [PunRPC]
    public void BarrierOn(bool active)
    {
        gameObject.SetActive(active);
    }

    [PunRPC]
    void BarrierCountUpdate(int barrierIndex)
    {
        barrierCount = barrierIndex;
        barrierCountText.text = barrierIndex.ToString();
    }
}
