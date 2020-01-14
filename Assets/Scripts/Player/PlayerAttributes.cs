using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerAttributes 
{
    private float meleeAttackSpeed;
    private float moveSpeed;
    private float health;
    private float bonusHealth;
    private Player pController;

    public bool Invulerable { get; set; }

    public void Init(Player p)
    {
        pController = p;
        this.meleeAttackSpeed = pController.GetPlayerStats().GetStartingStat().meleeAttackSpeed;
        this.moveSpeed = pController.GetPlayerStats().GetStartingStat().moveSpeed;
    }

    public float MeleeAttackSpeed
    {
        get
        {
            return Mathf.Clamp(this.meleeAttackSpeed, pController.GetPlayerStats().MinMeleeAttackSpeed, pController.GetPlayerStats().MaxMeleeAttackSpeed);
        }
        set
        {
            this.meleeAttackSpeed += value / 100f * pController.GetPlayerStats().MeleeAttackSpeedDiff;
        }
    }
    public float MoveSpeed
    {
        get
        {
            return Mathf.Clamp(this.moveSpeed, pController.GetPlayerStats().MinMoveSpeed, pController.GetPlayerStats().MaxMoveSpeed);
        }
        set
        {
            this.moveSpeed += value / 100f * pController.GetPlayerStats().MoveSpeedDiff;
        }
    }

    public float Health
    {
        get
        {
            float num = this.health +pController.GetPlayerStats().GetStartingStat().health + this.BonusHealth;
            return (num <= pController.GetPlayerStats().MaxHP) ? num : pController.GetPlayerStats().MaxHP;
        }
        set
        {
            this.health = value;
        }
    }
    public float BonusHealth
    {
        get
        {
            return this.bonusHealth;
        }
        set
        {
            this.bonusHealth = value;
        }
    }
}
