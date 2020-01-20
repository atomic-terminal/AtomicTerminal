#define EnableTest

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class PlayerTest : MonoBehaviour
{
    public Room room;
    [Conditional("EnableTest")]
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.L))
        //{
        //    Player.player.HPLoss(1);
        //}
        //if (Input.GetKeyDown(KeyCode.H))
        //{
        //    Player.player.BloodHeal(2);
        //}
        if (Input.GetKeyDown(KeyCode.B))
        {
            ballmove = !ballmove;
        }
        if (ballmove)
        {
            for (int i = 0; i < ball.Count; i++)
            {
                ball[i].DoUpdate();
            }
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            GameSetUp.setUp.Setup();
            GameManager.gManager.SetGameState(GameState.Running);//test
        }
        if (GameManager.gManager.GetGameState()== GameState.GameOver)
        {
            ballmove = false;
        }
    }
    [Conditional("EnableTest")]
    private void Start()
    {
        ball = new List<BallRC>();
        StartCoroutine(CreateCubes(cubeCount));

        GameSetUp.setUp.Setup();
        GameManager.gManager.SetGameState(GameState.Running);//test
        //StartCoroutine(CreateBalls(ballCount));
    }
    public Transform ballParent;
    public BallRC ballPrefab;
    public GameObject cubePrefab;
    private List<BallRC> ball;
    
    private bool ballmove;
    public int cubeCount = 50;
    public int ballCount=100;
    HashSet<Vector2> points = new HashSet<Vector2>();
    IEnumerator CreateCubes(int count)
    {
        Vector2 temp = new Vector2();
        for (int i = 0; i < count; i++)
        {
            do
            {
                temp.Set(Random.Range(-10, 11), Random.Range(0, 9));
                yield return null;
            } while (points.Contains(temp));
            points.Add(temp);
            GameObject go = GameObject.Instantiate(cubePrefab) as GameObject;
            go.transform.localPosition = new Vector3(temp.x, -0.25f, temp.y);

            go.transform.SetParent(ballParent);
        }
        yield return StartCoroutine(CreateBalls(ballCount));
    }
    IEnumerator CreateBalls(int count)
    {
        Vector2 temp = new Vector2();
        for (int i = 0; i < count; i++)
        {
            do
            {
                temp.Set(Random.Range(-10, 11) , Random.Range(-6, 9) );
                yield return null;
            } while (points.Contains(temp));
            points.Add(temp);
            GameObject go = GameObject.Instantiate(ballPrefab.gameObject)as GameObject;
            ball.Add(go.GetComponent<BallRC>());
            go.transform.localPosition = new Vector3(temp.x,-0.25f,temp.y);

            go.transform.SetParent(ballParent);
        }
    }
}
