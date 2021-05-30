using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerHitBoxScript : MonoBehaviour
{
    [SerializeField] Player myPlayerScript;
    [SerializeField] Animator playerAni;

    [SerializeField] PhotonView pv;
    GameManager GM;
    PlayerState ps;

    private void Start()
    {
        GM = myPlayerScript.GM;
        ps = GM.ps;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            if (!pv.IsMine) return;

            if (!myPlayerScript.canHit) return;

            Hit();
        }
        else if(other.tag == "Bullet")
        {
            if (!pv.IsMine) return;

            if (!myPlayerScript.canHit) return;

            if (other.GetComponent<BulletScript>().isPlayerAttack) return;

            Hit();
        }
    }

    void Hit()
    {
        if (!myPlayerScript.canHit) return;

        int randomNum = Random.Range(0, 101);
        if (ps.missPercentage >= randomNum)
        {
            StartCoroutine(GodTime());
            myPlayerScript.GM.MakeExplosionEffect(transform.position, "Player");//폭발이펙트
            myPlayerScript.canHit = false;
            return;
        }

        ps.life--;
        myPlayerScript.canHit = false;
        myPlayerScript.GM.UpdateLifeIcon(ps.life);
        myPlayerScript.GM.MakeExplosionEffect(transform.position, "Player");//폭발이펙트
        //pv.RPC(nameof(myPlayerScript.LifeUpdate), RpcTarget.All, ps.life);
        GM.NM.pv.RPC(nameof(GM.NM.PlayerInfoUpdate), RpcTarget.All, GM.NM.playerInfoGroupInt, GM.NM.playerIconCode, GM.jm.jobCode, ps.life);

        if (GM.jm.jobCode == 4 && !myPlayerScript.isDie)
        {
            GM.OP.PoolInstantiate("HealWave", transform.position, Quaternion.identity, -2, 0, 0, 0, true);
            GM.pv.RPC("Heal", RpcTarget.All, 1, GM.DTM.damageSkinCode);
        }


        if (ps.life == 0)
        {
            pv.RPC("PlayerIsDie", RpcTarget.All);
            GM.pv.RPC("AlivePlayerSet", RpcTarget.All);
        }
        else
        {
            myPlayerScript.GM.StartCoroutine("ReSpawnM");
        }
        StartCoroutine(GodTime());
    }



    IEnumerator GodTime()
    {
        playerAni.SetBool("GodOn", true);
        yield return new WaitForSeconds(ps.godTime);
        playerAni.SetBool("GodOn", false);
        myPlayerScript.canHit = true;
    }
}
