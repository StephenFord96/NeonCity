using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private CameraController playerCamera;
    private int travelling;
    public int missionGoal;
    public bool playerLiving;
    public GameObject gameOver;
    
    
    // Start is called before the first frame update
    void Start()
    {

        travelling = 1;
        DontDestroyOnLoad(gameObject);
        playerLiving = true;
        
    }

    // Update is called once per frame
    void Update()
    {

        if (playerLiving == false)
        {
            StartCoroutine("missionFailed");
        }
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

    IEnumerator missionFailed()
    {
        playerCamera = FindObjectOfType<CameraController>();
        GameObject gOver = Instantiate(gameOver, new Vector3(0f, 0f, 0f), transform.rotation);
        playerCamera.gameOver(gOver);

        yield return new WaitForSeconds(3f);
        playerLiving = true;
        SceneManager.LoadScene("BarHideOut");
        travelling = 1;
    }
}
