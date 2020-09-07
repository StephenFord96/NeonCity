using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    private int travelling;
    
    // Start is called before the first frame update
    void Start()
    {

        travelling = 1;
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player" && travelling ==1)
        {
            SceneManager.LoadScene("MissionArea");
            travelling = 0;
        }else if(other.gameObject.tag == "Player" && travelling == 0)
        {
            SceneManager.LoadScene("BarHideOut");
            travelling = 1;
        }


    }
}
