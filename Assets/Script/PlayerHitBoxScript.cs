using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerHitBoxScript : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] Animator playerAni;

    [SerializeField] PhotonView pv;
    GameManager GM;

    private void Start()
    {
        GM = FindObjectOfType<GameManager>();

        //player = GM.myplayer.GetComponent<Player>();

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            if (!pv.IsMine) return;

            if (!player.canHit) return;

            if (other.GetComponent<EnemyBasicScript>().isPassingNodamage) return;

            Hit();
        }
        else if(other.tag == "Bullet")
        {
            if (!pv.IsMine) return;

            if (!player.canHit) return;

            if (other.GetComponent<BulletScript>().isPlayerAttack) return;

            Hit();
        }
        else if (other.tag == "HealBullet")
        {
            if (!pv.IsMine) return;

            if (other.GetComponent<PhotonView>().IsMine) return;

            GM.OP.DamagePoolInstantiate("DamageText", transform.position, Quaternion.identity, 1, 2, GM.DTM.damageSkinCode, true);

            player.life++;
            GM.UpdateLifeIcon(player.life);
        }
    }

    void Hit()
    {
        int randomNum = Random.Range(0, 101);
        if (player.missPercentage > randomNum)
        {
            StartCoroutine(GodTime());
            player.GM.MakeExplosionEffect(transform.position, "Player");//폭발이펙트
            return;
        }

        player.life--;
        player.canHit = false;
        player.GM.UpdateLifeIcon(player.life);
        player.GM.MakeExplosionEffect(transform.position, "Player");//폭발이펙트

        if(GM.jm.jobCode == 4)
        GM.OP.PoolInstantiate("HealWave", transform.position, Quaternion.identity, -2, 0, 0, true);


        if (player.life == 0)
        {
            pv.RPC("PlayerIsDie", RpcTarget.All);
            GM.pv.RPC("AlivePlayerSet", RpcTarget.All);
        }
        else
        {
            player.GM.StartCoroutine("ReSpawnM");
        }
        StartCoroutine(GodTime());
    }


    IEnumerator GodTime()
    {
        playerAni.SetBool("GodOn", true);
        yield return new WaitForSeconds(player.godTime);
        playerAni.SetBool("GodOn", false);
        player.canHit = true;
    }

}
