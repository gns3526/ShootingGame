using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerCody : MonoBehaviourPun
{
    [SerializeField] int bodyCode;
    [SerializeField] Sprite[] bodyCodys;
    [SerializeField] ParticleSystem[] particles;

    [SerializeField] SpriteRenderer spriteRenderer;


    [SerializeField] PhotonView pv;


    [PunRPC]
    void CodyRework(int body, int particle)
    {
        for (int i = 0; i < particles.Length; i++)
            particles[i].Stop();

        spriteRenderer.sprite = bodyCodys[body];
        if(particle > -1)
        particles[particle].Play();
    }

}
