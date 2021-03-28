using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerCody : MonoBehaviourPun
{
    [SerializeField] Player playerScript;
    [SerializeField] ParticleSystem[] particles;

    [SerializeField] SpriteRenderer bodySpriteRenderer;
    [SerializeField] SpriteRenderer mainSpriteRenderer;

    [SerializeField] PhotonView pv;


    [PunRPC]
    void CodyRework(int main,int body, int particle)
    {
        for (int i = 0; i < particles.Length; i++)
            particles[i].Stop();

        bodySpriteRenderer.sprite = playerScript.GM.lobbyCodyDummy[body];
        mainSpriteRenderer.sprite = playerScript.GM.lobbyCodyMainDummy[main];

        if(particle > -1)
        particles[particle].Play();
    }

}
