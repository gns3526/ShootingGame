using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerCody : MonoBehaviourPun
{
    [SerializeField] Player playerScript;
    [SerializeField] ParticleSystem[] particles;

    [SerializeField] SpriteRenderer spriteRenderer;


    [SerializeField] PhotonView pv;


    [PunRPC]
    void CodyRework(int main,int body, int particle)
    {
        for (int i = 0; i < particles.Length; i++)
            particles[i].Stop();

        spriteRenderer.sprite = playerScript.GM.lobbyCodyDummy[body];
        playerScript.gameObject.GetComponent<SpriteRenderer>().sprite = playerScript.GM.lobbyCodyMainDummy[main];

        if(particle > -1)
        particles[particle].Play();
    }

}
