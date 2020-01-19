using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum GameState
{
    Running,
    GameOver,
}
public class GameManager : MonoBehaviour
{
    public static GameManager gManager;
    private void Awake()
    {
        gManager = this;
    }

    [SerializeField]
    private GameState gState;
    public void SetGameState(GameState state)
    {
        this.gState = state;
    }
    public GameState GetGameState()
    {
        return this.gState;
    }
}
