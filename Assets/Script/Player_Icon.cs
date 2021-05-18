using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class Player_Icon : MonoBehaviourPun
{ 
    [SerializeField] NetworkManager NM;

    public int playerIconCode;
    [SerializeField] Sprite[] imageGroup;

    public Image profileImage;
    public Image jobImage;
    public Text hpText;
    [SerializeField] Text text;


}


