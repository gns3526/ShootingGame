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
    public float myAlpha;
    public float otherAlpha;
    private void Start()
    {
        musicVolume = PlayerPrefs.GetFloat("MusicVolume");
        uiVolume = PlayerPrefs.GetFloat("UiVolume");
        shotVolume = PlayerPrefs.GetFloat("ShotVolume");

        soundManager.SetMusicVolume(musicVolume);
        soundManager.SetUiVolume(uiVolume);
        soundManager.SetShotVolume(shotVolume);
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.SetFloat("UiVolume", uiVolume);
        PlayerPrefs.SetFloat("ShotVolume", shotVolume);
    }

    public void OptionPanel(bool a)
    {
        optionPanel.SetActive(a);

        if (a)
        {
            musicVolumeText.text = Mathf.Round(musicVolume * 100).ToString();
            musicVolumeText.text = Mathf.Round(uiVolume * 100).ToString();
            musicVolumeText.text = Mathf.Round(shotVolume * 100).ToString();

            musicVolumeSlider.value = musicVolume;
            uiVolumeSlider.value = uiVolume;
            shotVolumeSlider.value = shotVolume;
        }
        else
        {
            dtm.damageColor_M = new Color(dtm.damageColor_M.r, dtm.damageColor_M.g, dtm.damageColor_M.b, myAlpha / 255);
            dtm.criticalColor_M = new Color(dtm.criticalColor_M.r, dtm.criticalColor_M.g, dtm.criticalColor_M.b, myAlpha / 255);
            dtm.healColor_M = new Color(dtm.healColor_M.r, dtm.healColor_M.g, dtm.healColor_M.b, myAlpha / 255);
            dtm.weaponColor_M = new Color(dtm.weaponColor_M.r, dtm.weaponColor_M.g, dtm.weaponColor_M.b, myAlpha / 255);
            dtm.weaponCriticalColor_M = new Color(dtm.weaponCriticalColor_M.r, dtm.weaponCriticalColor_M.g, dtm.weaponCriticalColor_M.b, myAlpha / 255);

            dtm.damageColor_Y = new Color(dtm.damageColor_Y.r, dtm.damageColor_Y.g, dtm.damageColor_Y.b, otherAlpha / 255);
            dtm.criticalColor_Y = new Color(dtm.criticalColor_Y.r, dtm.criticalColor_Y.g, dtm.criticalColor_Y.b, otherAlpha / 255);
            dtm.healColor_Y = new Color(dtm.healColor_Y.r, dtm.healColor_Y.g, dtm.healColor_Y.b, otherAlpha / 255);
            dtm.weaponColor_Y = new Color(dtm.weaponColor_Y.r, dtm.weaponColor_Y.g, dtm.weaponColor_Y.b, otherAlpha / 255);
            dtm.weaponCriticalColor_Y = new Color(dtm.weaponCriticalColor_Y.r, dtm.weaponCriticalColor_Y.g, dtm.weaponCriticalColor_Y.b, otherAlpha / 255);

            dtm.outline_M = new Color(0, 0, 0, myAlpha / 255);
            dtm.outline_Y = new Color(0, 0, 0, otherAlpha / 255);
        }


        SoundManager.Play("Btn_3");
    }
}
