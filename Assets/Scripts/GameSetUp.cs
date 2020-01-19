using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSetUp : MonoBehaviour
{
    public static GameSetUp setUp;
    public Room room;
    public void Awake()
    {
        GameSetUp.setUp = this;
    }
    public void Setup()
    {
        this.StartGame();
    }
    public void StartGame()
    {
       this.StartCoroutine(this.LoadGame());
    }
    private IEnumerator LoadGame()
    {
        yield return new WaitForSeconds(0);
        room.LoadEnemies();
        room.StartRoom();
        yield break;
    }
}
