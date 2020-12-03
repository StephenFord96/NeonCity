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

    // Start is called before the first frame update
    void Start()
    {
        for (int i = -2; i < 2; i++)
        {
            GameObject g = Instantiate(guard, new Vector3(gameObject.transform.position.x + i, gameObject.transform.position.y -1.35f, 0f), transform.rotation);
            callGuard = g.gameObject.GetComponent<EnemyController>();
            callGuard.xHQ = xHQ;
            callGuard.yHQ = yHQ;
            callGuard.xTether = positionX;
            callGuard.yTether = positionY;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
