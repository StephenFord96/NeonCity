using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalSlotManager : MonoBehaviour
{
    private LevelGenerator nexus;
    public GameObject terminal;

    private void Awake()
    {
        nexus = FindObjectOfType<LevelGenerator>();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag != "Enemy")
        {
            Destroy(gameObject, 0f);
        }
    }

    public void spawnTerminals()
    {
        Instantiate(terminal, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z), transform.rotation);
        Destroy(gameObject, 0f);
    }

}
