using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeParticleScript : MonoBehaviour
{
    [SerializeField] ParticleSystem particle;
    [SerializeField] TrailRenderer trail;

    public GameObject followTarget;

    public int particleAmount;

    bool following;

    private void OnEnable()
    {
        StartCoroutine(StartDelay());
    }

    IEnumerator StartDelay()
    {
        if (trail != null) trail.emitting = false;
        yield return new WaitForSeconds(0.01f);
        if(particle != null)
        {
            ParticleSystem.EmissionModule ps = particle.emission;
            ps.rateOverTime = particleAmount;
        }
        if (trail != null) trail.emitting = true;
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
        if(particle != null)
        transform.position = new Vector3(16, 16, 0);
        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }
}
