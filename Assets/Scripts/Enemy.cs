using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using MyTools;
public enum EnemyEnum
{
    Cube
}
public class Enemy : MonoBehaviour
{
    private bool isReady;
    protected GameObject enemyObject;
    protected Transform enemyTrans;
    [Header("Enemy Components")]
    public EnemyAttributes attributes;

    private CharStats stats;

    public void Awake()
    {
        this.enemyObject = this.gameObject;
        this.enemyTrans = base.transform;
       this. stats = GetComponent<CharStats>();
    }

    public EnemyEnum GetUnitName()
    {
        return this.attributes.enemyName;
    }
    protected bool isDead;
    public bool IsDead()
    {
        return this.isDead;
    }
    public virtual void Init()
    {
        this.isDead = false;
        SetAttributes();
        GetComponent<HPTest>().UpdateHeath(this.attributes.health);//test
    }
    public void SetAttributes()
    {
        float floorBasedHP = this.GetFloorBasedHP();
        this.attributes.health = floorBasedHP;
        this.attributes.maxHealth = this.attributes.health;
    }
    private float GetFloorBasedHP()
    {
        string text = string.Empty;
        if (this.stats != null)
        {
            text = this.stats.health;
        }
        float num = 0f;
        num = float.Parse(text, CultureInfo.InvariantCulture);
        float num2;
        num2 = MyUtility.RandValue(5);//test
        return  num+num2;
    }
    public void MoveEnemy(Vector3 pos)
    {
        this.enemyTrans.position = pos;
    }
    public virtual void MarkReady()
    {
        this.isReady = true;
        this.attributes.maxHealth = this.attributes.health;
        this.Enable(true);
    }
    protected void Enable(bool enable)
    {
        this.enemyObject.SetActive(enable);
    }
    public virtual void DepleteHP( int damage = 0)
    {
        if (damage > 0f)
        {
            this.attributes.health -= damage;
        }
        if (this.attributes.health <= 0f)
        {
            this.DeathTrigger();
        }
        GetComponent<HPTest>().UpdateHeath(this.attributes.health);//test
    }
    public void DeathTrigger()
    {
        this.attributes.health = 0f;
        this.isDead = true;
        //this.enemyObject.SetActive(false);
        ObjectPoolHandler.instance.ReturnObjectToPool(this.gameObject);
        EnemyManager.eManager.DecreaseCreatureCount();
    }
}
