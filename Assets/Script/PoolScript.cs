using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class PoolScript : MonoBehaviourPun
{
    public void DestroyOb()
    {
        GetComponent<PhotonView>().RPC("SetActiveRPC",RpcTarget.All,false);
    }

    [PunRPC]
    void SetActiveRPC(bool a)
    {
        if(!a)
        gameObject.transform.position = new Vector3(16, 16, 0);
        gameObject.SetActive(a);
    }
}
