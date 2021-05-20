using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    [SerializeField] OptionManager option;

    public static AudioSource[] audioSource;
    public static Dictionary<string, AudioSource> sounds;

    private void Awake()
    {
        audioSource = GetComponents<AudioSource>();
        sounds = new Dictionary<string, AudioSource>();

        sounds.Add("Gun_1", audioSource[0]);
        sounds.Add("Gun_2", audioSource[1]);
        sounds.Add("Gun_3", audioSource[2]);
        sounds.Add("Gun_4", audioSource[3]);
        sounds.Add("Gun_5", audioSource[4]);
        sounds.Add("Gun_6", audioSource[5]);
        sounds.Add("Laser_Ready", audioSource[6]);
        sounds.Add("Laser_1", audioSource[7]);
        sounds.Add("Laser_2", audioSource[8]);
        sounds.Add("Explosion_1", audioSource[9]);
        sounds.Add("Explosion_2", audioSource[10]);
        sounds.Add("Btn_1", audioSource[11]);
        sounds.Add("Btn_2", audioSource[12]);
        sounds.Add("Btn_3", audioSource[13]);
        sounds.Add("LobbyMusic_1", audioSource[14]);
        sounds.Add("LobbyMusic_2", audioSource[15]);
        sounds.Add("LobbyMusic_3", audioSource[16]);
    }
    //  SoundManager.Play("Effect_Getcombine");
    public static void Play(string soundName)
    {
        if (sounds.ContainsKey(soundName))
        {
            sounds[soundName].Play();
        }
    }

    public static void Stop(string soundName)
    {
        if (sounds.ContainsKey(soundName))
        {
            sounds[soundName].Stop();
        }
    }

    public void SetMusicVolume(float volume)
    {
        sounds["LobbyMusic_1"].volume = volume;
        sounds["LobbyMusic_2"].volume = volume;
        sounds["LobbyMusic_3"].volume = volume;

        option.musicVolumeText.text = Mathf.Round(volume * 100).ToString();

        option.musicVolume = volume;
    }
    public void SetUiVolume(float volume)
    {
        sounds["Btn_1"].volume = volume;
        sounds["Btn_2"].volume = volume;
        sounds["Btn_3"].volume = volume;

        option.uiVolumeText.text = Mathf.Round(volume * 100).ToString();

        option.uiVolume = volume;
    }
    public void SetShotVolume(float volume)
    {
        sounds["Gun_1"].volume = volume;
        sounds["Gun_2"].volume = volume;
        sounds["Gun_3"].volume = volume;
        sounds["Gun_4"].volume = volume;
        sounds["Gun_5"].volume = volume;
        sounds["Gun_6"].volume = volume;
        sounds["Laser_Ready"].volume = volume;
        sounds["Laser_1"].volume = volume;
        sounds["Laser_2"].volume = volume;
        sounds["Explosion_1"].volume = volume;
        sounds["Explosion_2"].volume = volume;

        option.shotVolumeText.text = Mathf.Round(volume * 100).ToString();

        option.shotVolume = volume;
    }
}
