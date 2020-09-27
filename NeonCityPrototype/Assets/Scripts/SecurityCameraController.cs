using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityCameraController : MonoBehaviour
{

    public bool assignedToTerminal;
    public bool hasPower;
    private Animator camAnim;
    private LevelGenerator nexus;
    private bool captured;
    

    // Start is called before the first frame update
    void Start()
    {
        camAnim = GetComponent<Animator>();
        //assignedToTerminal = false;
        nexus = FindObjectOfType<LevelGenerator>();
        captured = false;

    }

    // Update is called once per frame
    void Update()
    {
      
    }

    public void CaptureCamera()
    {
        camAnim.SetBool("Captured", true);
        captured = true;
        
        
    }

    public void raiseAlarm()
    {
        if (captured == false)
        {
            Debug.Log("Camera Sighted Player");
            nexus.guardsAlert();
        }
    }
}
