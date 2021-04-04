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
        if (pv.IsMine)
        {
            GM = FindObjectOfType<GameManager>();
            player = GM.myplayer.GetComponent<Player>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            if (player == null) return;

            if (!player.canHit) return;

            if (other.GetComponent<EnemyBasicScript>().isPassingNodamage) return;

            Hit();
        }
        else if(other.tag == "Bullet")
        {
            if (player == null) return;

            if (!player.canHit) return;

            if (other.GetComponent<BulletScript>().isPlayerAttack) return;

            Hit();
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
