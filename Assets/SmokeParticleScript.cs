using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeParticleScript : MonoBehaviour
{
    [SerializeField] ParticleSystem particle;
    public GameObject followTarget;

    bool following;

    private void OnEnable()
    {
        StartCoroutine(StartDelay());
    }

    IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(0.01f);
        following = true;
    }

    private void Update()
    {
        if (following)
        {
            transform.position = followTarget.transform.position;
            if (!followTarget.activeSelf)
                ParticleDisappear();
        }
        
    }

    public void ParticleDisappear()
    {
        following = false;
        transform.position = new Vector3(16, 16, 0);
        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }
}
