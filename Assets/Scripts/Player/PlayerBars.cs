using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerBars
{
    [Header("Hearts")]
    private List<HeartTween> hearts;
    [SerializeField]
    private Transform heartsParent;
    [SerializeField]
    private HeartTween heartPrefab;
    public void ResetHearts()
    {
        for (int i = 0; i < this.hearts.Count; i++)
        {
            this.hearts[i].ResetFlash();
        }
    }

    public void InitHearts(int _maxHp)
    {
        hearts = new List<HeartTween>();
        for (int i = 0; i < _maxHp; i++)
        {
            GameObject h = UnityEngine.Object.Instantiate(heartPrefab.gameObject) as GameObject;
            h.transform.parent = heartsParent;
            hearts.Add(h.GetComponent<HeartTween>());
        }
    }

    public void UpdateHearts(float _curHp)
    {
        for (int i = 0; i < this.hearts.Count; i++)
        {
            if ((float)i < _curHp)
            {
                this.hearts[i].SetHeartFG(true);
            }
            else
            {
                this.hearts[i].SetHeartFG(false);
            }
            if ((float)i < Player.player.GetAttribute().Health)
            {
                this.hearts[i].SetHeartBG(true);
            }
            else
            {
                this.hearts[i].SetHeartBG(false);
            }
        }
    }

    public void HpLost(float _curHp, float _hpLost)
    {
        for (int i = 0; i < this.hearts.Count; i++)
        {
            if ((float)i >= _curHp && (float)i < _curHp + _hpLost)
            {
                this.hearts[i].Hit();
            }
        }
    }

    public void HpGained(float _curHp, float _hpGained)
    {
        for (int i = 0; i < this.hearts.Count; i++)
        {
            if ((float)i >= _curHp - _hpGained && (float)i < _curHp)
            {
                this.hearts[i].Flash(Color.green);
            }
        }
    }
}
