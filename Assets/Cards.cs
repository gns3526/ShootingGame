using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cards : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] GameManager GM;
    public void Card1()
    {

        GM.SelectComplete();
    }
    public void Card2()
    {
        GM.SelectComplete();
    }
    public void Card3()
    {
        GM.SelectComplete();
    }
    public void Card4()
    {
        GM.SelectComplete();
    }
}
