using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CodySelectUpdate : MonoBehaviour
{
    [SerializeField] GameManager gm;
    [SerializeField] NetworkManager nm;
    [SerializeField] JobManager jm;
    [SerializeField] GameObject[] codys;
    [SerializeField] int codyTypeCode;

    [Header("ItemInfo")]
    [SerializeField] bool explainInfo;
    [SerializeField] Text itemNameText;
    [SerializeField] Text itemInfoText;

    [SerializeField] string[] itemNames;
    [SerializeField] string[] itemInfos;


    private void OnEnable()
    {
        for (int i = 0; i < codys.Length; i++)
            codys[i].transform.GetChild(3).gameObject.SetActive(false);

        if (explainInfo)
        {
            switch (codyTypeCode)
            {
                case 0:
                    itemNameText.text = itemNames[gm.codyMainCode];
                    itemInfoText.text = itemInfos[gm.codyMainCode];
                    break;
                case 1:
                    itemNameText.text = itemNames[gm.codyBodyCode];
                    itemInfoText.text = itemInfos[gm.codyBodyCode];
                    break;
                case 2:
                    itemNameText.text = itemNames[gm.codyParticleCode];
                    itemInfoText.text = itemInfos[gm.codyParticleCode];
                    break;
                case 3:
                    itemNameText.text = itemNames[nm.playerIconCode];
                    itemInfoText.text = itemInfos[nm.playerIconCode];
                    break;
                case 4:
                    itemNameText.text = itemNames[jm.jobCode];
                    itemInfoText.text = itemInfos[jm.jobCode];
                    break;
            }
            
            
        }
    }


    public void CodyOnClick(int index)
    {
        for (int i = 0; i < codys.Length; i++)
            codys[i].transform.GetChild(3).gameObject.SetActive(false);
        Debug.Log(index + "누름");
        
        codys[index].transform.GetChild(3).gameObject.SetActive(true);

        if (explainInfo)
        {
            itemNameText.text = "\"" + itemNames[index].ToString() + "\"";
            itemInfoText.text = itemInfos[index].ToString();
        }

        if (!codys[index].transform.GetChild(1).gameObject.activeSelf)
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

        SoundManager.Play("Btn_3");
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
