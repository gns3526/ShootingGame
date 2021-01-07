using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ReadyArea : MonoBehaviour
{
    [SerializeField] NetworkManager NM;
    [SerializeField] PhotonView NMPV;


    private void Start()
    {


    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        NM.RoomRenewal();
        
    }
}
