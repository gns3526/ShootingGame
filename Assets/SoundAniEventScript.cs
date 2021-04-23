using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundAniEventScript : MonoBehaviour
{
    public void SoundPlay(string soundName)
    {
        SoundManager.Play(soundName);
    }
}
