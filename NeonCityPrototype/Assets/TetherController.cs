using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetherController : MonoBehaviour
{
    public int positionX;
    public int positionY;
    public int xHQ;
    public int yHQ;
    public GameObject guard;
    private EnemyController callGuard;
    private GameObject[] roster = new GameObject[4];


    public int teamProgress;


    // 0 is tether
    // 1 is HQ
    // 2 is patrolling
    // this is what the guards on this squad are currently doing
    public int[] tasks = new int[4];

    public int teamSize;
    public bool spawned;
    public bool patrolHQ;
    public bool patrolTether;
    public bool stackedTether;
    public bool stackedHQ;
    public int guard1;
    public int guard2;

    // Start is called before the first frame update
    void Start()
    {
        teamSize = 2;
        teamProgress = 0;
        patrolHQ = false;
        patrolTether = false;
        spawned = false;


        for (int i = 1; i < 5; i++)
        {
            GameObject g = Instantiate(guard, new Vector3(gameObject.transform.position.x + ((i-2.5f) * 2), gameObject.transform.position.y -1.35f, 0f), transform.rotation);
            callGuard = g.gameObject.GetComponent<EnemyController>();
            callGuard.xHQ = xHQ;
            callGuard.yHQ = yHQ;
            callGuard.xTether = positionX;
            callGuard.yTether = positionY;
            callGuard.squadID = i;
            callGuard.myTether = gameObject;
            roster[i - 1] = g.gameObject;
        }
    }

    void Update()
    {

    }


    public void GuardInPosition(int guardID, int guardTask)
    {
        tasks[guardID - 1] = guardTask;
        teamProgress = teamProgress + 1;
        if(teamProgress == 4)
        {
            spawned = true;
            teamProgress = 0;
            Invoke("firstPatrol", 0f);
        }

        if(teamProgress == teamSize && spawned == true)
        {
            //team is set
            StartCoroutine("patrolTimer", 0f);
        }
    }

    public void firstPatrol()
    {
        guard1 = Random.Range(0, 4);
        guard2 = Random.Range(0, 4);
        while(guard2 == guard1)
        {
            guard2 = Random.Range(0, 4);
        }

        tasks[guard1] = 2;
        tasks[guard2] = 2;

        callGuard = roster[guard1].GetComponent<EnemyController>();
        callGuard.patrolHQ();
        callGuard = roster[guard2].GetComponent<EnemyController>();
        callGuard.patrolHQ();

    }


    IEnumerator patrolTimer()
    {
        yield return new WaitForSeconds(Random.Range(3f, 6f));
        callGuard = roster[guard1].GetComponent<EnemyController>();
        callGuard.patrolTether();
        callGuard = roster[guard2].GetComponent<EnemyController>();
        callGuard.patrolTether();

        tasks[guard1] = 2;
        tasks[guard2] = 2;
    }
}
