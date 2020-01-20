using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager eManager;

    private int creatureCount;
    public int waveCount = 0;
    public List<GameObject> enemySpawnList;
    private List<string> waveSpawnList = new List<string>();
    private void Awake()
    {
        EnemyManager.eManager = this;
    }
    public void SetCreatureCount(int count,int wavecount)
    {
        this.waveCount = wavecount;
        this.creatureCount = count;
    }
    public void DecreaseCreatureCount()
    {
        this.creatureCount--;
        if (this.creatureCount <= 0 && !Player.player.IsDead())
        {
            this.creatureCount = 0;
            waveCount--;
            if (waveCount<=0)
            {
                this.RoomEnd();
            }
            else
            {
                Reinforcement(Player.player.GetRoom().GetSpawnList());
            }
        }
    }
    private void RoomEnd()
    {
        waveCount = 0;
        Debug.Log("All Dead");//test
        Player.player.RoomTrigger(false, null);
    }

    public void SpawnRoomUnits(List<string> unitNames, List<GameObject> spawns, Room p, bool shuffle = true)
    {
        List<GameObject> list = new List<GameObject>();
        foreach (GameObject gameObject in spawns)
        {
            if (gameObject.activeInHierarchy)
            {
                list.Add(gameObject);
            }
        }
            EnemyPoolManager.poolManager.LoadPreset(unitNames, list, p, shuffle);
    }
    public void Reinforcement(List<string> list)
    {
        this.creatureCount = list.Count;
        this.waveSpawnList.Clear();
        this.waveSpawnList = list;
        this.Spawn(Player.player.GetRoom().GetSpawnPoints());
    }
    public void Spawn(List<GameObject> eSpawns = null)
    {
        if (this.waveSpawnList.Count > 0)
        {
            for (int i = 0; i < this.waveSpawnList.Count; i++)
            {
                Enemy enemy;
                enemy = EnemyPoolManager.poolManager.FetchUnit(this.waveSpawnList[i], this.RandSpawnPos(), Player.player.GetRoom());
                enemy.MarkReady();
            }
        }
        this.waveSpawnList.Clear();
    }
    private Vector3 RandSpawnPos()
    {
        if (this.enemySpawnList == null || this.enemySpawnList.Count == 0)
        {
            this.enemySpawnList = new List<GameObject>(Player.player.GetRoom().GetSpawnPoints());
        }
        int index = MyTools.MyUtility.RandValue(this.enemySpawnList.Count);
        Vector3 position = this.enemySpawnList[index].transform.position;
        position = new Vector3(position.x, 0.5f, position.z);
        this.enemySpawnList.RemoveAt(index);
        return position;
    }
}
