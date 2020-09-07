using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleFlashController : MonoBehaviour
{

    public GameObject muzzleFlash;
    public GameObject bullet;
    private bool cooling;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && cooling == false)
        {

            cooling = true;
            Instantiate(muzzleFlash, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z), gameObject.transform.rotation);
            GameObject b = Instantiate(bullet, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z), transform.rotation);
            b.gameObject.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
            StartCoroutine(shotCooldown());
        }
    }


    IEnumerator shotCooldown()
    {

        yield return new WaitForSeconds(0.333f);
        cooling = false;
    }
}
