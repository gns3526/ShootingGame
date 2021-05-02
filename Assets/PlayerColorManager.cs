using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerColorManager : MonoBehaviour
{
    [SerializeField] GameManager GM;

    public float[] playerColors;

    public GameObject colorChangePanel;
    [SerializeField] GameObject colorNormalPanel;
    [SerializeField] GameObject colorPremiumPanel;

    [SerializeField] Slider colorRSlider;
    [SerializeField] Slider colorGSlider;
    [SerializeField] Slider colorBSlider;
    [SerializeField] Text[] colorRGBTexts;
    [SerializeField] Image playerColorTest;

    public void ColorChangePanelOpenOrClose(bool a)
    {
        colorChangePanel.SetActive(a);


        if (a)
        {
            playerColorTest.sprite = GM.lobbyCodyMainDummy[GM.codyMainCode];
            playerColorTest.color = new Color(playerColors[0] / 255f, playerColors[1] / 255f, playerColors[2] / 255f, 1);
            colorNormalPanel.SetActive(true);
            colorPremiumPanel.SetActive(false);
            GM.codyIconPanel.SetActive(false);
            GM.codyMainPanel.SetActive(false);
            GM.codyPanel.SetActive(false);
            GM.abilityPanel.SetActive(false);
        }

        SoundManager.Play("Btn_2");
    }

    public void NormalColorOpenOrClose(bool a)
    {
        colorNormalPanel.SetActive(a);

        SoundManager.Play("Btn_2");
    }
    public void PremiumColorOpenOrClose(bool a)
    {
        colorPremiumPanel.SetActive(a);

        if (a)
        {
            colorRSlider.value = playerColors[0] / 255f;
            colorGSlider.value = playerColors[1] / 255f;
            colorBSlider.value = playerColors[2] / 255f;
        }

        SoundManager.Play("Btn_2");
    }

    public void NormalColorChange()
    {
        R = Random.Range(0, 256);
        G = Random.Range(0, 256);
        B = Random.Range(0, 256);

        playerColorTest.color = new Color(R / 255f, G / 255f, B / 255f, 1);
        //myplayer.GetComponent<PhotonView>().RPC("ChangeColorRPC", RpcTarget.All,playerColors[0], playerColors[1], playerColors[2]);

        SoundManager.Play("Btn_2");
    }

    void PlayerColorTest()
    {
        R = colorRSlider.value * 255;
        G = colorGSlider.value * 255;
        B = colorBSlider.value * 255;
        colorRGBTexts[0].text = Mathf.Round(R).ToString();
        colorRGBTexts[1].text = Mathf.Round(G).ToString();
        colorRGBTexts[2].text = Mathf.Round(B).ToString();
        playerColorTest.color = new Color(R / 255f, G / 255f, B / 255f, 1);
    }

    float R;
    float G;
    float B;
    public void NormalColorChangeComplete()
    {
        playerColors[0] = Mathf.Round(R);
        playerColors[1] = Mathf.Round(G);
        playerColors[2] = Mathf.Round(B);

        GM.lobbyPlayer.color = new Color(playerColors[0] / 255f, playerColors[1] / 255f, playerColors[2] / 255f, 1);

        colorChangePanel.SetActive(false);

        SoundManager.Play("Btn_2");
    }
    public void PremiumColorChangeComplete()
    {
        R = colorRSlider.value * 255;
        G = colorGSlider.value * 255;
        B = colorBSlider.value * 255;

        playerColors[0] = Mathf.Round(R);
        playerColors[1] = Mathf.Round(G);
        playerColors[2] = Mathf.Round(B);

        GM.lobbyPlayer.color = new Color(playerColors[0] / 255f, playerColors[1] / 255f, playerColors[2] / 255f, 1);

        colorChangePanel.SetActive(false);

        SoundManager.Play("Btn_2");
    }





    private void Update()
    {
        if (colorPremiumPanel.activeSelf)
            PlayerColorTest();
    }
}
