﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{

    public bool assignedToTerminal;
    public bool hasPower;
    private Animator turretAnim;
    public bool captured;



    // Start is called before the first frame update
    void Start()
    {
        turretAnim = GetComponent<Animator>();
        captured = false;
        
        
    }

    // Update is called once per frame
    void Update()
    {
     
        
    }

    public void CaptureCamera()
    {
        turretAnim.SetBool("Captured", true);
        captured = true;
        GetComponentInChildren<MuzzleFlashController>(captured).Equals(true);
    }
}
