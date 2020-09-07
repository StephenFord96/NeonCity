using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private PlayerController target;
    private GameObject player;
    private float xLeg;
    private float yLeg;
    

    // Start is called before the first frame update
    void Start()
    {
        target = FindObjectOfType<PlayerController>();
        player = GameObject.FindGameObjectWithTag("Player");
        xLeg = (gameObject.transform.position.x - player.transform.position.x + Random.Range(-0.5f,0.5f)) * 6f;
        yLeg = (gameObject.transform.position.y - player.transform.position.y) * 6f;
      
    }


    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(-xLeg *Time.deltaTime, -yLeg *Time.deltaTime));

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.SetActive(false);
        }

        Destroy(gameObject, 0f);
    }
}
