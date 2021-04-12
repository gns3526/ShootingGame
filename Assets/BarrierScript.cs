using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarrierScript : MonoBehaviour
{
    [SerializeField] Player playerScript;
    ObjectPooler op;
    JopManager jm;
    [SerializeField] Text barrierCountText;
    public int barrierCount;

    private void Start()
    {
        op = FindObjectOfType<ObjectPooler>();
        jm = FindObjectOfType<JopManager>();
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        barrierCountText.text = barrierCount.ToString();
        barrierCountText.enabled = true;
    }
    private void OnDisable()
    {
        barrierCountText.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!playerScript.pv.IsMine) return;

        if (other.tag == "Bullet" && !other.GetComponent<BulletScript>().isPlayerAttack)
        {
            barrierCount--;
            barrierCountText.text = barrierCount.ToString();
            op.PoolDestroy(other.gameObject);
            if (barrierCount == 0)
                gameObject.SetActive(false);
        }
    }
}
