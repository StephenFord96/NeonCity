using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIConversationController : MonoBehaviour
{

    public float slideID;
    private Animator anim;
    

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("SlideCount", slideID);
    }


    public void ExitConversation()
    {
        Destroy(gameObject, 0f);
    }
}
