using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaderScript : MonoBehaviour
{
    public Player myPlayerScript;

    [SerializeField] GameManager gm;

    public List<GameObject> enemys;

    void Update()
    {
        gameObject.transform.position = myPlayerScript.transform.position;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            enemys.Add(other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            enemys.Remove(other.gameObject);
        }
    }
}
