using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

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
    }
    //  SoundManager.Play("Effect_Getcombine");
    public static void Play(string soundName)
    {
        if (sounds.ContainsKey(soundName))
        {
            sounds[soundName].Play();
        }
    }
}
