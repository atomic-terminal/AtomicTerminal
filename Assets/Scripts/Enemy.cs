using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum EnemyEnum
{
    cube
}
public class Enemy : MonoBehaviour
{
    private bool isReady;
    protected GameObject enemyObject;
    protected Transform enemyTrans;

    public void Awake()
    {
        this.enemyObject = this.gameObject;
        this.enemyTrans = base.transform;
    }

    public EnemyEnum GetUnitName()
    {
        //return this.attributes.enemyName;
        return EnemyEnum.cube;//test
    }
    protected bool isDead;
    public bool IsDead()
    {
        return this.isDead;
    }
    public virtual void Init()
    {
    }
    public void MoveEnemy(Vector3 pos)
    {
        this.enemyTrans.position = pos;
    }
    public virtual void MarkReady()
    {
        this.isReady = true;
        this.Enable(true);
    }
    protected void Enable(bool enable)
    {
        this.enemyObject.SetActive(enable);
    }

    public int HP;
    public virtual void DepleteHP( int damage = 0)
    {
        if (damage > 0f)
        {
            this.HP -= damage;
        }
        if (this.HP <= 0f)
        {
            this.DeathTrigger();
        }
    }
    public void DeathTrigger()
    {
        HP = 0;
        this.isDead = true;
        this.enemyObject.SetActive(false);
        EnemyManager.eManager.DecreaseCreatureCount();
    }
}
