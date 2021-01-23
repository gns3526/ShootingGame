using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitBoxScript : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] Animator playerAni;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy" || other.tag == "EnemyBullet")
        {
            if (player.canHit) return;

            int randomNum = Random.Range(0, 101);
            if(player.missPercentage > randomNum)
            {
                return;
            }

            player.life--;
            player.canHit = false;
            player.GM.UpdateLifeIcon(player.life);
            player.GM.MakeExplosionEffect(transform.position, "Player");//폭발이펙트

            if (player.life == 0)
            {
                player.GM.GameOver();
            }
            else
            {

                player.GM.StartCoroutine("ReSpawnM");
            }
            StartCoroutine(GodTime());
        }
    }

    IEnumerator GodTime()
    {
        playerAni.SetBool("GodOn", true);
        yield return new WaitForSeconds(player.godTime);
        playerAni.SetBool("GodOn", false);
        player.canHit = true;
    }

}
