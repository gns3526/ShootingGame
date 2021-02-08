using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class PoolScript : MonoBehaviourPun
{

    private void Awake()
    {

        gameObject.transform.position = new Vector3(4, 4, 0);
    }
    private void OnEnable()
    {
        gameObject.transform.position = new Vector3(4, 4, 0);
    }
    private void OnDisable()
    {
        gameObject.transform.position = new Vector3(4, 4, 0);
    }


    [PunRPC]
    void SetActiveRPC(bool a)
    {
        //gameObject.transform.position = new Vector3(4, 4, 0);
        gameObject.SetActive(a);
        //gameObject.SetActive(a);
    }
}
