using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class BarrierScript : MonoBehaviourPunCallbacks, IPunObservable
{
    public Player myPlayerScript;

    public GameManager gm;
    public ObjectPooler op;

    [SerializeField] Text barrierCountText;

    public int barrierCount;
    public float duraction;

    public PhotonView pv;
    [SerializeField] CircleCollider2D circleCollider;

    [SerializeField] private Animator barrierHitAni;

    bool active;
    [SerializeField] Vector3 curPosPv;

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
        if (myPlayerScript == null) return;

        if (!myPlayerScript.pv.IsMine) return;

        if (other.tag == "Bullet" && !other.GetComponent<BulletScript>().isPlayerAttack)
        {
            barrierCount--;
            pv.RPC(nameof(BarrierCountUpdate), RpcTarget.All, barrierCount);
            op.PoolDestroy(other.gameObject);

            pv.RPC(nameof(BarrierAniRPC), RpcTarget.All);
           
            if (barrierCount == 0)
                pv.RPC(nameof(BarrierOn), RpcTarget.All, false);
        }
    }

    private void Update()
    {
        if (pv.IsMine && duraction > 0 && active)
        {
            duraction -= Time.deltaTime;
            gameObject.transform.position = myPlayerScript.transform.position;

            if(duraction < 0)
                op.PoolDestroy(gameObject);
        }
        else
        {
            //if ((transform.position - curPosPv).sqrMagnitude >= 3) transform.position = curPosPv;
            //else
            //    transform.position = Vector3.Lerp(transform.position, curPosPv, Time.deltaTime * 10);

            transform.position = curPosPv;
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

    [PunRPC]
    void BarrierAniRPC()
    {
        barrierHitAni.SetTrigger("Hit");
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
