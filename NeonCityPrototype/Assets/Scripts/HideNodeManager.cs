using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideNodeManager : MonoBehaviour
{

    private PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        player = FindObjectOfType<PlayerController>();

        if (player.playerHidden == false)
        {
            Destroy(gameObject, 0f);
        }
    }
}
