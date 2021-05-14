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

            if (other.GetComponent<EnemyBasicScript>().isPassingNodamage) return;

            Hit();
        }
        else if(other.tag == "Bullet")
        {
            if (!pv.IsMine) return;

            if (!myPlayerScript.canHit) return;

            if (other.GetComponent<BulletScript>().isPlayerAttack) return;

            Hit();
        }
        else if (other.tag == "HealBullet")
        {
            if (!pv.IsMine) return;

            if (other.GetComponent<PhotonView>().IsMine) return;

            if (GM.jm.jobCode == 4) return;

            GM.OP.DamagePoolInstantiate("DamageText", transform.position, Quaternion.identity, 1, 2, GM.DTM.damageSkinCode, true);

            ps.life++;
            GM.UpdateLifeIcon(ps.life);
        }
    }

    void Hit()
    {
        int randomNum = Random.Range(0, 101);
        if (ps.missPercentage > randomNum)
        {
            StartCoroutine(GodTime());
            myPlayerScript.GM.MakeExplosionEffect(transform.position, "Player");//폭발이펙트
            return;
        }

        ps.life--;
        myPlayerScript.canHit = false;
        myPlayerScript.GM.UpdateLifeIcon(ps.life);
        myPlayerScript.GM.MakeExplosionEffect(transform.position, "Player");//폭발이펙트

        if(GM.jm.jobCode == 4 && !myPlayerScript.isDie)
        GM.OP.PoolInstantiate("HealWave", transform.position, Quaternion.identity, -2, 0, 0, true);

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
