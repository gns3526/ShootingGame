using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour
{
    [SerializeField] SoundManager soundManager;
    [SerializeField] DamageTextManager dtm;

    [SerializeField] GameObject optionPanel;

    [Header("Sound")]
    [SerializeField] GameObject soundPanel;
    public float musicVolume;
    public float uiVolume;
    public float shotVolume;

    [SerializeField] Slider musicVolumeSlider;
    [SerializeField] Slider uiVolumeSlider;
    [SerializeField] Slider shotVolumeSlider;
    public Text musicVolumeText;
    public Text uiVolumeText;
    public Text shotVolumeText;

    [Header("DamageSkin")]
    [SerializeField] GameObject damagePanel;
    public float myAlpha;
    public float otherAlpha;
    [SerializeField] Text myAlphaText;
    [SerializeField] Text otherAlphaText;
    [SerializeField] Text testMyDamageText;
    [SerializeField] Outline testMyOutlineText;
    [SerializeField] Text testOtherDamageText;
    [SerializeField] Outline testOtherOutlineText;

    [SerializeField] Slider myTransparencySlider;
    [SerializeField] Slider otherTransparencySlider;

    private void Start()
    {

        PlayerPrefs.GetInt("Start");
        if(PlayerPrefs.GetInt("Start") == 0)
        {
            PlayerPrefs.SetInt("Start", 1);

            musicVolume = 1;
            uiVolume = 1;
            shotVolume = 1;

            myAlpha = 1;
            otherAlpha = 1;
        }
        else
        {
            musicVolume = PlayerPrefs.GetFloat("MusicVolume");
            uiVolume = PlayerPrefs.GetFloat("UiVolume");
            shotVolume = PlayerPrefs.GetFloat("ShotVolume");

            myAlpha = PlayerPrefs.GetFloat("MyAlpha");
            otherAlpha = PlayerPrefs.GetFloat("OtherAlpha");
        }

        soundManager.SetMusicVolume(musicVolume);
        soundManager.SetUiVolume(uiVolume);
        soundManager.SetShotVolume(shotVolume);

        SoundManager.Play("LobbyMusic_1");
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.SetFloat("UiVolume", uiVolume);
        PlayerPrefs.SetFloat("ShotVolume", shotVolume);

        PlayerPrefs.SetFloat("MyAlpha", myAlpha);
        PlayerPrefs.SetFloat("OtherAlpha", otherAlpha);
    }

    public void OptionPanel(bool a)
    {
        optionPanel.SetActive(a);

        if (a)
        {
            damagePanel.SetActive(false);
            soundPanel.SetActive(true);

            musicVolumeText.text = Mathf.Round(musicVolume * 100).ToString();
            musicVolumeText.text = Mathf.Round(uiVolume * 100).ToString();
            musicVolumeText.text = Mathf.Round(shotVolume * 100).ToString();

            musicVolumeSlider.value = musicVolume;
            uiVolumeSlider.value = uiVolume;
            shotVolumeSlider.value = shotVolume;
        }
        else
        {
            dtm.damageColor_M = new Color(dtm.damageColor_M.r, dtm.damageColor_M.g, dtm.damageColor_M.b, myAlpha);
            dtm.criticalColor_M = new Color(dtm.criticalColor_M.r, dtm.criticalColor_M.g, dtm.criticalColor_M.b, myAlpha);
            dtm.healColor_M = new Color(dtm.healColor_M.r, dtm.healColor_M.g, dtm.healColor_M.b, myAlpha);
            dtm.weaponColor_M = new Color(dtm.weaponColor_M.r, dtm.weaponColor_M.g, dtm.weaponColor_M.b, myAlpha);
            dtm.weaponCriticalColor_M = new Color(dtm.weaponCriticalColor_M.r, dtm.weaponCriticalColor_M.g, dtm.weaponCriticalColor_M.b, myAlpha);

            dtm.damageColor_Y = new Color(dtm.damageColor_Y.r, dtm.damageColor_Y.g, dtm.damageColor_Y.b, otherAlpha);
            dtm.criticalColor_Y = new Color(dtm.criticalColor_Y.r, dtm.criticalColor_Y.g, dtm.criticalColor_Y.b, otherAlpha);
            dtm.healColor_Y = new Color(dtm.healColor_Y.r, dtm.healColor_Y.g, dtm.healColor_Y.b, otherAlpha);
            dtm.weaponColor_Y = new Color(dtm.weaponColor_Y.r, dtm.weaponColor_Y.g, dtm.weaponColor_Y.b, otherAlpha);
            dtm.weaponCriticalColor_Y = new Color(dtm.weaponCriticalColor_Y.r, dtm.weaponCriticalColor_Y.g, dtm.weaponCriticalColor_Y.b, otherAlpha);

            dtm.outline_M = new Color(0, 0, 0, myAlpha);
            dtm.outline_Y = new Color(0, 0, 0, otherAlpha);
        }


        SoundManager.Play("Btn_3");
    }

    public void SoundPanel()
    {
        soundPanel.SetActive(true);

        damagePanel.SetActive(false);

        musicVolumeText.text = Mathf.Round(musicVolume * 100).ToString();
        musicVolumeText.text = Mathf.Round(uiVolume * 100).ToString();
        musicVolumeText.text = Mathf.Round(shotVolume * 100).ToString();

        musicVolumeSlider.value = musicVolume;
        uiVolumeSlider.value = uiVolume;
        shotVolumeSlider.value = shotVolume;

        SoundManager.Play("Btn_1");
    }

    public void BattlePanel()
    {
        damagePanel.SetActive(true);

        soundPanel.SetActive(false);

        myTransparencySlider.value = myAlpha;
        otherTransparencySlider.value = otherAlpha;

        SoundManager.Play("Btn_1");
    }

    public void SetMyDamageTransparency(float value)
    {
        myAlphaText.text = Mathf.Round(value * 100).ToString();

        myAlpha = value;
        testMyDamageText.color = new Color(testMyDamageText.color.r, testMyDamageText.color.g, testMyDamageText.color.b, value);
        testMyOutlineText.effectColor = new Color(0, 0, 0, value);
    }
    public void SetOtherDamageTransparency(float value)
    {
        otherAlphaText.text = Mathf.Round(value * 100).ToString();

        otherAlpha = value;
        testOtherDamageText.color = new Color(testOtherDamageText.color.r, testOtherDamageText.color.g, testOtherDamageText.color.b, value);
        testOtherOutlineText.effectColor = new Color(0, 0, 0, value);
    }
}
