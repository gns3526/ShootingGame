using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class PoolScript : MonoBehaviourPun
{
    bool once;
    private void Start()
    {
        once = true;
    }



    [PunRPC]
    void SetActiveRPC(bool a)
    {

        gameObject.transform.position = new Vector3(4, 4, 0);

        gameObject.SetActive(a);
        once = false;
    }
}
