using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cards : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] GameManager GM;

    public void CardS(int num)
    {
        switch (num)
        {
            case 1:
                if (player.shotCoolTimeReduce != 10)
                {
                    player.shotCoolTimeReduce -= 10;
                }
                GM.SelectComplete();
                break;
            case 2:
                player.moveSpeed += 1;
                GM.SelectComplete();
                break;
            case 3:
                player.increaseDamage += 3;
                GM.SelectComplete();
                break;
            case 4:
                if (player.power != player.maxPower)
                {
                    player.power++;
                }
                GM.SelectComplete();
                break;
            case 5:
                if (player.maxLife != 10)
                {
                    player.maxLife++;
                    player.life++;
                    GM.UpdateLifeIcon(player.life);
                }
                GM.SelectComplete();
                break;
            case 6:
                player.life = player.maxLife;
                GM.UpdateLifeIcon(player.life);
                GM.SelectComplete();
                break;
            case 7:
                player.AddFollower(1);
                GM.SelectComplete();
                break;
            case 8:
                player.AddFollower(2);
                GM.SelectComplete();
                break;
        }
    }
}
