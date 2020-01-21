using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    private bool roomStarted;
    private List<Enemy> enemiesOfThisRoom;
    [Header("Room Start / End")]
    [SerializeField]
    private List<GameObject> portals;

    [SerializeField]
    private List<EnemyEnum> enemySpawnsList;
    private List<string> spawnsList;
    [SerializeField]
    private int waveCount;
    [SerializeField]
    [Header("Spawns")]
    private List<GameObject> defaultSpawns;

    public List<GameObject> GetSpawnPoints()
    {
        List<GameObject> list = new List<GameObject>();
        foreach (GameObject gameObject in this.defaultSpawns)
        {
            if (gameObject.activeInHierarchy)
            {
                list.Add(gameObject);
            }
        }
        return list;
    }
    public List<string> GetSpawnList()
    {
        return this.spawnsList;
    }
    private void Awake()
    {
        spawnsList = new List<string>();
        for (int i = 0; i < enemySpawnsList.Count; i++)
        {
            spawnsList.Add(enemySpawnsList[i].ToString());
        }
    }
    public void LoadEnemies()
    {
        List<GameObject> list = new List<GameObject>();
        list.AddRange(this.defaultSpawns);
        EnemyManager.eManager.SpawnRoomUnits(this.spawnsList, list, this, true);
    }
    public void AddEnemy(Enemy e)
    {
        if (this.enemiesOfThisRoom == null)
        {
            this.enemiesOfThisRoom = new List<Enemy>();
        }
        if (!this.enemiesOfThisRoom.Contains(e))
        {
            this.enemiesOfThisRoom.Add(e);
        }
    }

    public void StartRoom()
    {
        Player.player.SetPlayerRoom(this);
        this.roomStarted = true;
        this.PortalGates(true);
        int num = 0;
        foreach (Enemy enemy in this.enemiesOfThisRoom)
        {
            enemy.MarkReady();
            num++;
        }
        EnemyManager.eManager.SetCreatureCount(num, waveCount);
    }
    public void RoomEnd()
    {
        this.PortalGates(false);
        GameManager.gManager.SetGameState(GameState.GameOver);//test
    }
    public void PortalGates(bool enable)
    {
        foreach (GameObject gameObject in this.portals)
        {
            gameObject.SetActive(enable);
        }
    }
    }
