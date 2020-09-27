using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarTenderSpeech : MonoBehaviour
{
    public Text assignment;
    private int missionID;
    private LevelManager missionData;
    public GameObject missionPortal;
    public LevelManager oldPortal;

    void Start()
    {
        missionID = Random.Range(0, 3);
        if (missionID == 0)
        {
            assignment.text = "Our client needs you to get the black brief case";
        }
        else if (missionID == 1)
        {
            assignment.text = "Our client needs you to get the brown brief case";
        }
        else if (missionID == 2)
        {
            assignment.text = "Our client needs you to get the grey brief case";
        }
        oldPortal = FindObjectOfType<LevelManager>();
        if(oldPortal != null)
        {
            Destroy(oldPortal.gameObject, 0f);
        }
        Instantiate(missionPortal, new Vector3(-9.5f, -1.5f, 0f), transform.rotation);
    }

    
    void Update()
    {
        missionData = FindObjectOfType<LevelManager>();
        missionData.missionGoal = missionID;
    }
}
