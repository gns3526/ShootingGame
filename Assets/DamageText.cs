using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class DamageText : MonoBehaviour
{
    public PhotonView pv;

    [SerializeField] GameManager GM;
    [SerializeField] ObjectPooler objectManager;

    [SerializeField] Text damage;

    [SerializeField] Color damageColor;
    [SerializeField] Color healColor;
    [SerializeField] Color criticalColor;

    Color color;

    private void Awake()
    {
        GM = FindObjectOfType<GameManager>();
        objectManager = GM.OP;

        transform.parent = GameObject.Find("DamageDummy").gameObject.transform;

        gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 1);
    }

    [PunRPC]
    public void ChangeTextRPC(float damageAmount, int colornum, bool plus)
    {
        if (plus)
        {
            damage.text = "+" + damageAmount.ToString();
        }
        else
        {
            damage.text = "-" + damageAmount.ToString();
        }
        if(colornum == 0)
        {
            damage.color = healColor;
        }
        else if(colornum == 1)
        {
            damage.color = damageColor;
        }
        else if(colornum == 2)
        {
            damage.color = criticalColor;
        }
    }

    public void PoolDestroy()
    {
        objectManager.PoolDestroy(gameObject);
    }
}
