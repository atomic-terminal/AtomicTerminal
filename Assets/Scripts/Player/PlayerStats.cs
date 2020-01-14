using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats stats;
    [SerializeField]
    private float minMS, maxMS, minAS, maxAS;
    [SerializeField]
    private int maxHP;
    [SerializeField]
    private StartingStats startStats;
    public void Awake()
    {
        stats = this;
    }
    public float MinMoveSpeed
    {
        get
        {
            return minMS;
        }
    }
    public float MaxMoveSpeed
    {
        get
        {
            return maxMS;
        }
    }
    public float MoveSpeedDiff
    {
        get
        {
            return this.MaxMoveSpeed - this.MinMoveSpeed;
        }
    }

    public float MinMeleeAttackSpeed
    {
        get
        {
            return minAS;
        }
    }
    public float MaxMeleeAttackSpeed
    {
        get
        {
            return maxAS;
        }
    }
    public float MeleeAttackSpeedDiff
    {
        get
        {
            return this.MeleeAttackSpeedDiff - this.MinMeleeAttackSpeed;
        }
    }

    public int MaxHP
    {
        get
        {
            return maxHP;
        }
    }

    public StartingStats GetStartingStat()
    {
        return this.startStats;
    }
}
[Serializable]
public class StartingStats
{
    [Header("Stats")]
    public int health = 3;
    public float moveSpeed = 10f;
    public float meleeAttackSpeed = 1f;
}
