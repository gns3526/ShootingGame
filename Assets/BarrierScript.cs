using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class BarrierScript : MonoBehaviour
{
    public Player myPlayerScript;

    public GameManager gm;
    public ObjectPooler op;

    [SerializeField] Text barrierCountText;

    public int barrierCount;
    public float duraction;

    public PhotonView pv;
    [SerializeField] CircleCollider2D circleCollider;

    bool active;

    public void BarrierActive()
    {

        if (myPlayerScript.pv.IsMine)
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

        active = true;
    }

    private void OnDisable()
    {
        barrierCountText.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!myPlayerScript.pv.IsMine) return;

        if (other.tag == "Bullet" && !other.GetComponent<BulletScript>().isPlayerAttack)
        {
            barrierCount--;
            pv.RPC(nameof(BarrierCountUpdate), RpcTarget.All, barrierCount);
            op.PoolDestroy(other.gameObject);
            if (barrierCount == 0)
                pv.RPC(nameof(BarrierOn), RpcTarget.All, false);
        }
    }

    private void Update()
    {
        if (myPlayerScript != null && duraction > 0 && active)
        {
            duraction -= Time.deltaTime;
            gameObject.transform.position = myPlayerScript.transform.position;

            if(duraction < 0)
                op.PoolDestroy(gameObject);
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
