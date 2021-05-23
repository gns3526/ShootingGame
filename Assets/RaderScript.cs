using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaderScript : MonoBehaviour
{
    public Player myPlayerScript;

    [SerializeField] GameManager gm;

    public List<GameObject> enemys;

    [SerializeField] CircleCollider2D circleCol;

    void Update()
    {
        gameObject.transform.position = myPlayerScript.transform.position;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            enemys.Add(other.gameObject);

            CheckEnemyList();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {

            enemys.Clear();
            circleCol.enabled = false;
            circleCol.enabled = true;

            CheckEnemyList();
        }
    }

    void CheckEnemyList()
    {
        if (enemys.Count == 0)
        {
            gm.canShotWeapon = false;
            if (gm.isAndroid)
                gm.mobileWeaponLockOb.SetActive(true);
            else
                gm.deskTopWeaponLockOb.SetActive(true);
        }
        else
        {
            gm.canShotWeapon = true;
            if (gm.isAndroid)
                gm.mobileWeaponLockOb.SetActive(false);
            else
                gm.deskTopWeaponLockOb.SetActive(false);
        }
    }
}
