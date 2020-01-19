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
    private List<string> spawnsList;
    [SerializeField]
    [Header("Spawns")]
    private List<GameObject> defaultSpawns;
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
        EnemyManager.eManager.SetCreatureCount(num);
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
