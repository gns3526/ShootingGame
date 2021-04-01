using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CodySelectUpdate : MonoBehaviour
{
    GameManager gm;
    [SerializeField] GameObject[] codys;
    [SerializeField] int codyTypeCode;

    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
    }


    public void CodyOnClick(int index)
    {
        switch (codyTypeCode)
        {
            case 0:
                gm.codyMainCode = index;
                gm.lobbyPlayer.sprite = gm.lobbyCodyMainDummy[index];
                break;
            case 1:
                gm.codyBodyCode = index;
                gm.lobbyPlayer.transform.GetChild(0).GetComponent<Image>().sprite = gm.lobbyCodyDummy[index];
                break;
            case 2:
                gm.codyParticleCode = index;
                for (int i = 0; i < gm.lobbyParticleDummy.Length; i++)
                    gm.lobbyParticleDummy[i].Stop();
                gm.lobbyParticleDummy[index].Play();
                Debug.Log("bbb");
                break;
        }
        Select();
    }
    //0 = main
    //1 = body
    //2 = particle


    public void Select()
    {
        switch (codyTypeCode)
        {
            case 0:
                for (int i = 0; i < codys.Length; i++)
                    codys[i].transform.GetChild(2).gameObject.SetActive(gm.codyMainCode == i ? true : false);
                break;
            case 1:
                for (int i = 0; i < codys.Length; i++)
                    codys[i].transform.GetChild(2).gameObject.SetActive(gm.codyBodyCode == i ? true : false);
                break;
            case 2:
                for (int i = 0; i < codys.Length; i++)
                    codys[i].transform.GetChild(2).gameObject.SetActive(gm.codyParticleCode == i ? true : false);
                Debug.Log("aaa");
                break;
        }
        

    }
}
