using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools;

public class EnemyPoolManager : MonoBehaviour
{
    public static EnemyPoolManager poolManager;
    private void Awake()
    {
        EnemyPoolManager.poolManager = this;
    }

    public List<Enemy> enemyPool;
    public void LoadPreset(List<string> unitNames, List<GameObject> pos, Room p, bool shuffle)
    {
        List<GameObject> list = new List<GameObject>();
        int num = 0;
        foreach (GameObject gameObject in pos)
        {
            if (gameObject.activeInHierarchy)
            {
                list.Add(gameObject);
            }
        }
        foreach (string name in unitNames)
        {
            if (list.Count == 0)
            {
                break;
            }
            if (shuffle)
            {
                int index = MyTools.MyUtility.RandValue(list.Count);
                this.FetchPooled(list[index].transform.position, name, p);
                list.RemoveAt(index);
            }
            else
            {
                this.FetchPooled(list[num].transform.position, name, p);
                num++;
            }
        }
    }
    private Enemy FetchPooled(Vector3 pos, string name, Room p = null)
    {
        //foreach (Enemy enemy in this.enemyPool)
        //{
        //    if (enemy.GetUnitName().ToString() == name && enemy.IsDead())
        //    {
        //        enemy.Init();
        //        enemy.MoveEnemy(new Vector3(pos.x,0, pos.z));
        //        if (p != null)
        //        {
        //            p.AddEnemy(enemy);
        //        }
        //        return enemy;
        //    }
        //}
        return this.LoadEnemy(pos, name, p);
        //return null;
    }
    private Enemy LoadEnemy(Vector3 pos, string name, Room p)
    {
        GameObject gameObject = null;
        //gameObject = (UnityEngine.Object.Instantiate(Resources.Load(name)) as GameObject);
        gameObject = ObjectPoolHandler.instance.GetObjectFromPool(name, Vector3.zero, Quaternion.identity);
        gameObject.transform.parent = base.transform;
        Enemy component = gameObject.GetComponent<Enemy>();
        if (p != null)
        {
            p.AddEnemy(component);
        }
        component.Init();
        component.MoveEnemy(new Vector3(pos.x,0, pos.z));
        this.enemyPool.Add(component);
        return component;
    }

    public Enemy FetchUnit(string unitName, Vector3 pos, Room p)
    {
        Enemy enemy = this.FetchPooled(pos, unitName, p);
        if (enemy == null)
        {
            Debug.LogError(unitName + " Fetching failed");
        }
        return enemy;
    }
}
