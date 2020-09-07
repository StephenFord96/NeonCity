using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpottedEffectFollower : MonoBehaviour
{

    public GameObject followTarget;
    private bool targetAcquired;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    

    // Update is called once per frame
    void Update()
    {

        if (followTarget != null)
        {

            if (targetAcquired == true)
            {
                transform.position = new Vector3(followTarget.transform.position.x, followTarget.transform.position.y + 0.8f, followTarget.transform.position.z);
            }

            if (followTarget.gameObject.activeInHierarchy == false)
            {
                followTarget = null;
                Destroy(gameObject);
            }

        } 

        
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (targetAcquired == false)
        {

            if (other.gameObject.CompareTag("Enemy"))
            {

                followTarget = other.gameObject;
                targetAcquired = true;
            }
        }
    }


}

