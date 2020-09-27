using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HackPointManager : MonoBehaviour
{
    public Text points;
    public LevelGenerator nexus;
    private int pointCount;

    void Start()
    {
        nexus = FindObjectOfType<LevelGenerator>();
        pointCount = nexus.playerHackPoints;
    }

    
    void Update()
    {
        pointCount = nexus.playerHackPoints;
        points.text = pointCount.ToString();
    }
}
