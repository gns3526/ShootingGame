using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetManager : MonoBehaviour
{
    public GameObject[] petGroup;
    public Player myPlayerScript;

    public void SetPlayerPetGroup()
    {
        petGroup[0].GetComponent<Pet>().parent = myPlayerScript.gameObject.transform;
        for (int i = 0; i < petGroup.Length; i++)
        {
            myPlayerScript.pets[i] = petGroup[i];
            petGroup[i].GetComponent<Pet>().player = myPlayerScript;
            petGroup[i].SetActive(false);
        }
    }
}
