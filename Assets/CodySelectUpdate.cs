using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CodySelectUpdate : MonoBehaviour
{
    GameManager gm;
    NetworkManager nm;
    JobManager jm;
    [SerializeField] GameObject[] codys;
    [SerializeField] int codyTypeCode;

    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
        nm = FindObjectOfType<NetworkManager>();
        jm = FindObjectOfType<JobManager>();
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
                break;
            case 3:
                nm.playerIconCode = index;
                gm.playerIcon.sprite = nm.icons[nm.playerIconCode];
                break;
            case 4:
                jm.jobCode = index;
                break;
        }
        Select();
    }
    //0 = main
    //1 = body
    //2 = particle
    //3 = icon
    //4 = jop;

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
                break;
            case 3:
                for (int i = 0; i < codys.Length; i++)
                    codys[i].transform.GetChild(2).gameObject.SetActive(nm.playerIconCode == i ? true : false);
                break;
            case 4:
                for (int i = 0; i < codys.Length; i++)
                    codys[i].transform.GetChild(2).gameObject.SetActive(jm.jobCode == i ? true : false);
                break;
        }
        

    }
}
