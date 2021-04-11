using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierScript : MonoBehaviour
{
    [SerializeField] Player playerScript;
    ObjectPooler op;
    public int barrierCount;

    private void Start()
    {
        gameObject.SetActive(false);
        op = FindObjectOfType<ObjectPooler>();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!playerScript.pv.IsMine) return;

        if (other.tag == "Bullet" && !other.GetComponent<BulletScript>().isPlayerAttack)
        {
            barrierCount--;
            op.PoolDestroy(other.gameObject);
            if (barrierCount == 0)
                gameObject.SetActive(false);
        }
    }
}
