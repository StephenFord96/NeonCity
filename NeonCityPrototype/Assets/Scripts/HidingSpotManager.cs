using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingSpotManager : MonoBehaviour
{

    private LevelGenerator nexus;
    public GameObject[] furniture = new GameObject[3];
    private bool posFree;


    private void Awake()
    {
        nexus = FindObjectOfType<LevelGenerator>();
        
    }

    void Start()
    {
        posFree = false;
        StartCoroutine(lockedIn());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag != "Enemy" && posFree == false)
        {
            Destroy(gameObject, 0f);
        }

    }


    IEnumerator lockedIn()
    {
        yield return new WaitForSeconds(0.5f);
        posFree = true;
    }
    

    public void spawnFurniture()
    {
       
        Instantiate(furniture[Random.Range(0, 3)], new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
        Destroy(gameObject, 0f);
    }
}
