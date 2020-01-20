using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyAttributes
{
    public EnemyEnum enemyName;
    [HideInInspector]
    public float maxHealth;
    [HideInInspector]
    public float health;
}
