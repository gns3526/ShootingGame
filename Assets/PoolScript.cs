using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class PoolScript : MonoBehaviourPun
{
    [PunRPC]
    void SetActiveRPC(bool a)
    {
        gameObject.SetActive(a);
    }
}
