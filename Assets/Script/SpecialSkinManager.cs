using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialSkinManager : MonoBehaviour
{
    public bool[] challenge;

    public void ChallengeClear(int index)
    {
        challenge[index] = true;
    }
}
