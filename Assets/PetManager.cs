using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PetManager : MonoBehaviour
{
    public JobManager JM;
    public ObjectPooler OP;
    public PlayerState ps;

    public GameObject[] petGroup;
    public Player myPlayerScript;

    [SerializeField] PhotonView pv;

    public void SetPlayerPetGroup()
    {
        
        for (int i = 0; i < petGroup.Length; i++)
        {
            if(i == 0)
                petGroup[0].GetComponent<Pet>().followTarget = myPlayerScript.gameObject.transform;
            else if(i > 0 && i < 10)
                petGroup[i].GetComponent<Pet>().followTarget = petGroup[i - 1].gameObject.transform;


            myPlayerScript.pets[i] = petGroup[i];
            petGroup[i].GetComponent<Pet>().player = myPlayerScript;
            petGroup[i].SetActive(false);
        }
    }


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha3)) OP.PoolInstantiate("Pet", myPlayerScript.transform.position, Quaternion.identity, -3, 1, -1, 0, true);
    }

    [PunRPC]
    public void AddPet(int type)
    {


        for (int i = 0; i < petGroup.Length; i++)
        {
            if (!petGroup[i].activeSelf)
            {
                

                petGroup[i].SetActive(true);

                Pet petScript = petGroup[i].GetComponent<Pet>();

                pv.RPC("petSpriteChangeRPC", RpcTarget.All, i, type);

                switch (type)
                {
                    case 1:
                        petScript.maxShotCoolTime = 0.2f;
                        petScript.bulletType = 1;
                        break;
                    case 2:
                        petScript.maxShotCoolTime = 2f;
                        petScript.bulletType = 2;
                        break;
                }
                break;
            }
        }
    }
}
