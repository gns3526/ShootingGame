using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerColorManager : MonoBehaviour
{
    [SerializeField] GameManager GM;

    public float[] playerMainColors;
    public float[] playerBoosterColors;

    public GameObject colorChangePanel;
    [SerializeField] GameObject colorNormalPanel;
    [SerializeField] GameObject colorPremiumPanel;

    [SerializeField] Slider colorRSlider_M ;
    [SerializeField] Slider colorGSlider_M;
    [SerializeField] Slider colorBSlider_M;

    [SerializeField] Slider colorRSlider_B;
    [SerializeField] Slider colorGSlider_B;
    [SerializeField] Slider colorBSlider_B;

    [SerializeField] Text[] colorRGBTexts_M;
    [SerializeField] Text[] colorRGBTexts_B;

    [SerializeField] Image playerMainColor;
    [SerializeField] Image playerBoosterColor;


    float Main_R;
    float Main_G;
    float Main_B;

    float Booster_R;
    float Booster_G;
    float Booster_B;
    public void ColorChangePanelOpenOrClose(bool a)
    {
        colorChangePanel.SetActive(a);


        if (a)
        {
            playerMainColor.sprite = GM.lobbyCodyMainDummy[GM.codyMainCode];

            playerMainColor.color = new Color(playerMainColors[0] / 255f, playerMainColors[1] / 255f, playerMainColors[2] / 255f, 1);
            playerBoosterColor.color = new Color(playerBoosterColors[0] / 255f, playerBoosterColors[1] / 255f, playerBoosterColors[2] / 255f, 1);

            colorNormalPanel.SetActive(true);
            colorPremiumPanel.SetActive(false);
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
            colorRSlider_M.value = playerMainColors[0] / 255f;
            colorGSlider_M.value = playerMainColors[1] / 255f;
            colorBSlider_M.value = playerMainColors[2] / 255f;
        }

        SoundManager.Play("Btn_2");
    }

    public void NormalColorChange()
    {
        Main_R = Random.Range(0, 256);
        Main_G = Random.Range(0, 256);
        Main_B = Random.Range(0, 256);

        Booster_R = Random.Range(0, 256);
        Booster_G = Random.Range(0, 256);
        Booster_B = Random.Range(0, 256);

        playerMainColor.color = new Color(Main_R / 255f, Main_G / 255f, Main_B / 255f, 1);
        playerBoosterColor.color = new Color(Booster_R / 255f, Booster_G / 255f, Booster_B / 255f, 1);

        SoundManager.Play("Btn_2");
    }

    

    public void NormalColorChangeComplete()
    {
        playerMainColors[0] = Mathf.Round(Main_R);
        playerMainColors[1] = Mathf.Round(Main_G);
        playerMainColors[2] = Mathf.Round(Main_B);

        playerBoosterColors[0] = Mathf.Round(Booster_R);
        playerBoosterColors[1] = Mathf.Round(Booster_G);
        playerBoosterColors[2] = Mathf.Round(Booster_B);

        GM.lobbyPlayer.color = new Color(playerMainColors[0] / 255f, playerMainColors[1] / 255f, playerMainColors[2] / 255f, 1);

        colorChangePanel.SetActive(false);

        SoundManager.Play("Btn_2");
    }
    public void PremiumColorChangeComplete()
    {
        Main_R = colorRSlider_M.value * 255;
        Main_G = colorGSlider_M.value * 255;
        Main_B = colorBSlider_M.value * 255;

        Booster_R = Random.Range(0, 256);
        Booster_G = Random.Range(0, 256);
        Booster_B = Random.Range(0, 256);


        playerMainColors[0] = Mathf.Round(Main_R);
        playerMainColors[1] = Mathf.Round(Main_G);
        playerMainColors[2] = Mathf.Round(Main_B);

        playerBoosterColors[0] = Mathf.Round(Booster_R);
        playerBoosterColors[1] = Mathf.Round(Booster_G);
        playerBoosterColors[2] = Mathf.Round(Booster_B);

        GM.lobbyPlayer.color = new Color(playerMainColors[0] / 255f, playerMainColors[1] / 255f, playerMainColors[2] / 255f, 1);

        colorChangePanel.SetActive(false);

        SoundManager.Play("Btn_2");
    }

    void PlayerColorTest()
    {
        Main_R = colorRSlider_M.value * 255;
        Main_G = colorGSlider_M.value * 255;
        Main_B = colorBSlider_M.value * 255;

        Booster_R = colorRSlider_B.value * 255;
        Booster_G = colorGSlider_B.value * 255;
        Booster_B = colorBSlider_B.value * 255;

        colorRGBTexts_M[0].text = Mathf.Round(Main_R).ToString();
        colorRGBTexts_M[1].text = Mathf.Round(Main_G).ToString();
        colorRGBTexts_M[2].text = Mathf.Round(Main_B).ToString();

        colorRGBTexts_B[0].text = Mathf.Round(Booster_R).ToString();
        colorRGBTexts_B[1].text = Mathf.Round(Booster_G).ToString();
        colorRGBTexts_B[2].text = Mathf.Round(Booster_B).ToString();

        playerMainColor.color = new Color(Main_R / 255f, Main_G / 255f, Main_B / 255f, 1);
        playerBoosterColor.color = new Color(Booster_R / 255f, Booster_G / 255f, Booster_B / 255f, 1);
    }



    private void Update()
    {
        if (colorPremiumPanel.activeSelf)
            PlayerColorTest();
    }
}
