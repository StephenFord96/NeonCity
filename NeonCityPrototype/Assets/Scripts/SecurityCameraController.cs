using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityCameraController : MonoBehaviour
{

    public bool assignedToTerminal;
    public bool hasPower;
    private Animator camAnim;
    private LevelGenerator nexus;

    // Start is called before the first frame update
    void Start()
    {
        camAnim = GetComponent<Animator>();
        //assignedToTerminal = false;
        nexus = FindObjectOfType<LevelGenerator>();

    }

    // Update is called once per frame
    void Update()
    {
      
    }

    public void CaptureCamera()
    {
        camAnim.SetBool("Captured", true);
    }

    public void raiseAlarm()
    {
        Debug.Log("Camera Sighted Player");
        nexus.guardsAlert();
    }
}
