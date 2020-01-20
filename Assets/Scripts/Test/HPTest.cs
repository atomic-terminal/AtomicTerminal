using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HPTest : MonoBehaviour
{
    public TextMeshProUGUI uGUI;
    public void UpdateHeath(float hp)
    {
        uGUI.text = hp.ToString();
    }
}
