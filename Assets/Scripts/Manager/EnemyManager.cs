using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager eManager;

    private int creatureCount;
    private void Awake()
    {
        EnemyManager.eManager = this;
    }
    public void SetCreatureCount(int count)
    {
        this.creatureCount = count;
    }
    public void DecreaseCreatureCount()
    {
        this.creatureCount--;
        if (this.creatureCount <= 0)
        {
            this.creatureCount = 0;
        }
        if (this.creatureCount <= 0 && !Player.player.IsDead())
        {
            this.RoomEnd();
        }
    }
    private void RoomEnd()
    {
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
}
