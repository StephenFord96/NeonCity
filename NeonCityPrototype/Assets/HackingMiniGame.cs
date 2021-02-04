using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackingMiniGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            Destroy(gameObject, 0f);
        }
    }
}
