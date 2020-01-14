#define EnableTest

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class PlayerTest : MonoBehaviour
{
    [Conditional("EnableTest")]
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Player.player.HPLoss(1);
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            Player.player.BloodHeal(2);
        }
    }
}
