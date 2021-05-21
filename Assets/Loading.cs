using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    [SerializeField] Text loadingText;

    private void OnEnable()
    {
        StartCoroutine(LoadingTextAni());
    }

    IEnumerator LoadingTextAni()
    {
        loadingText.text = "Loading.";
        yield return new WaitForSeconds(0.5f);
        loadingText.text = "Loading..";
        yield return new WaitForSeconds(0.5f);
        loadingText.text = "Loading...";
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(LoadingTextAni());
    }
}
