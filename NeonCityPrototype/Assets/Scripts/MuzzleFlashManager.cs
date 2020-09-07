using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleFlashManager : MonoBehaviour
{
    public bool end;

    // Start is called before the first frame update
    void Start()
    {
        end = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(end == true)
        {
            Destroy(gameObject, 0f);
        }
    }
}
