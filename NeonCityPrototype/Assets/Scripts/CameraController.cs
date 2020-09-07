using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public GameObject followTarget;
    public GameObject player;
    public GameObject crosshairBlue;
    private GameObject currentCrossHair;
    public GameObject glitchEffect;

    // Start is called before the first frame update
    void Start()
    {
        followTarget = GameObject.FindGameObjectWithTag("Player");
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {

        transform.position = new Vector3(followTarget.transform.position.x, followTarget.transform.position.y + 2.15f, -10f);


        if ((Input.GetKeyDown("escape") || Input.GetKeyDown("3")) && followTarget != player)
        {
            followTarget = player;
            Destroy(currentCrossHair, 0f);
        }

    }


    public void camFocus(GameObject newTarget)
    {
        if (currentCrossHair != null)
        {
            Destroy(currentCrossHair, 0f);
        }

        followTarget = newTarget;
        GameObject c = Instantiate(crosshairBlue, new Vector3(newTarget.gameObject.transform.position.x, newTarget.gameObject.transform.position.y, newTarget.gameObject.transform.position.z), transform.rotation);
        currentCrossHair = c;
        GameObject g = Instantiate(glitchEffect, new Vector3(newTarget.gameObject.transform.position.x, newTarget.gameObject.transform.position.y, newTarget.gameObject.transform.position.z), transform.rotation);
        Destroy(g.gameObject, 0.33f);
    }
}
