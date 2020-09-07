using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireSpliceGameController : MonoBehaviour
{
    private TerminalController callTerminal;
    private GameObject[] possibleTerminals;
    public GameObject[] numberUI = new GameObject[9];
    public GameObject[] destroyables;
    public GameObject[] remainingChips;
    public GameObject chip;
    public GameObject wireNode;
    public GameObject lead;
    private GameObject _leader;
    public int leaderPosX;
    public int leaderPosY;

    private int tempCount;

    List<int> zeroizeGridX = new List<int>();
    List<int> zeroizeGridY = new List<int>();

    public int[,] nodeLocations = new int[10, 2];

    // grid[x coord, y coord] = danger value
    public int[,] grid = new int[17, 12];

    public GameObject[,] chipArchive = new GameObject[17, 12];


    // Start is called before the first frame update
    void Start()
    {


        for (int n = 0; n < nodeLocations.Length / 2; n++)
        {
            do
            {
                //gets x value
                nodeLocations[n, 0] = Random.Range(1, 17);
                //gets y value
                nodeLocations[n, 1] = Random.Range(1, 12);

                for (int i = 0; i < n; i++)
                {
                    if (nodeLocations[i, 0] == nodeLocations[n, 0] && nodeLocations[i, 1] == nodeLocations[n, 1])
                    {
                        nodeLocations[n, 0] = 0;
                        nodeLocations[n, 1] = 0;
                        //Debug.Log("caught one");
                    }
                }


            } while (nodeLocations[n, 0] == 0 || nodeLocations[n, 1] == 0);

            Instantiate(wireNode, new Vector3(gameObject.transform.position.x - 3.828f + (nodeLocations[n, 0] * 0.578f), gameObject.transform.position.y - 3.453f + (nodeLocations[n, 1] * 0.578f), gameObject.transform.position.z), transform.rotation);


            //self
            grid[nodeLocations[n, 0], nodeLocations[n, 1]] += 1;

            //east
            if (nodeLocations[n, 0] < 16)
            {
                grid[nodeLocations[n, 0] + 1 , nodeLocations[n, 1]] += 1;
            }

            //northeast
            if (nodeLocations[n, 0] < 16 && nodeLocations[n, 1] < 11)
            {
                grid[nodeLocations[n, 0] + 1, nodeLocations[n, 1] + 1] += 1;
            }

            //southeast
            if (nodeLocations[n, 0] < 16 && nodeLocations[n, 1] > 1)
            {
                grid[nodeLocations[n, 0] + 1, nodeLocations[n, 1] - 1] += 1;
            }

            //west
            if (nodeLocations[n, 0] > 1)
            {
                grid[nodeLocations[n, 0] - 1, nodeLocations[n, 1]] += 1;
            }


            //northwest
            if (nodeLocations[n, 0] > 1 && nodeLocations[n, 1] < 11)
            {
                grid[nodeLocations[n, 0] - 1, nodeLocations[n, 1] + 1] += 1;
            }
            
            //southwest
            if (nodeLocations[n, 0] > 1 && nodeLocations[n, 1] > 1)
            {
                grid[nodeLocations[n, 0] - 1, nodeLocations[n, 1] - 1] += 1;
            }

            //north
            if (nodeLocations[n, 1] < 11)
            {
                grid[nodeLocations[n, 0], nodeLocations[n, 1] + 1] += 1;
            }

            //south
            if (nodeLocations[n, 1] > 1)
            {
                grid[nodeLocations[n, 0], nodeLocations[n, 1] - 1] += 1;
            }

        }

        for (int w = 1; w < 17; w++)
        {

            for (int h = 1; h < 12; h++)
            {
                GameObject o = Instantiate(chip, new Vector3(gameObject.transform.position.x - 3.828f + (w * 0.578f), gameObject.transform.position.y - 3.453f + (h * 0.578f), gameObject.transform.position.z), transform.rotation);

                chipArchive[w, h] = o.gameObject;


                if (grid[w,h] >= 1)
                {
                    
                    Instantiate(numberUI[grid[w, h] - 1], new Vector3(gameObject.transform.position.x - 3.828f + (w * 0.578f), gameObject.transform.position.y - 3.453f + (h * 0.578f), gameObject.transform.position.z), transform.rotation);
                }

            }


        }

        GameObject l = Instantiate(lead, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z), transform.rotation);
        _leader = l.gameObject;


        Cursor.visible = false;

        possibleTerminals = GameObject.FindGameObjectsWithTag("Terminal");

        foreach (GameObject t in possibleTerminals)
        {
            callTerminal = t.gameObject.GetComponent<TerminalController>();
            if(callTerminal.playerPicking == true)
            {
                break;
            }
            else { callTerminal = null; }
        }




    }

    // Update is called once per frame
    void Update()
    {
        remainingChips = GameObject.FindGameObjectsWithTag("Chip");

        destroyables = GameObject.FindGameObjectsWithTag("Wire Fodder");

        if (Input.GetKeyDown("escape"))
        {

            Cursor.visible = true;

            foreach (GameObject d in destroyables)
            {
                Destroy(d.gameObject, 0f);
            }

            foreach(GameObject c in remainingChips)
            {
                Destroy(c.gameObject, 0f);
            }

            Destroy(gameObject, 0f);
        }
        

        if (leaderPosX < 17 && leaderPosX > 0 && leaderPosY < 12 && leaderPosY > 0)
        {
              _leader.transform.Translate(new Vector3(Input.GetAxisRaw("Mouse X") * 50f, Input.GetAxisRaw("Mouse Y") * 50f, 0f) * Time.deltaTime);
        }

        leaderPosX = Mathf.CeilToInt((_leader.gameObject.transform.position.x - gameObject.transform.position.x + 3.539f) / 0.578f);
        leaderPosY = Mathf.CeilToInt((_leader.gameObject.transform.position.y - gameObject.transform.position.y + 3.164f) / 0.578f);

        

        if (leaderPosX > 16)
        {
            _leader.transform.Translate(new Vector3(-10f, 0f, 0f) * Time.deltaTime);
        }

        if (leaderPosX < 1)
        {
            _leader.transform.Translate(new Vector3(10f, 0f, 0f) * Time.deltaTime);
        }

        if (leaderPosY < 1)
        {
            _leader.transform.Translate(new Vector3(0f, 10f, 0f) * Time.deltaTime);
        }

        if (leaderPosY > 11)
        {
            _leader.transform.Translate(new Vector3(0f, -10f, 0f) * Time.deltaTime);
        }

        if (Input.GetMouseButtonDown(0))
        {

            for (int c = 0; c < nodeLocations.Length/2; c++)
            {
                if(leaderPosX == nodeLocations[c,0] && leaderPosY == nodeLocations[c, 1])
                {
                    Debug.Log("Hit a wire!");
                }
            }

            if (grid[leaderPosX, leaderPosY] == 0)
            {
                Destroy(chipArchive[leaderPosX, leaderPosY], 0f);
                fill(leaderPosX, leaderPosY);
            }
            else
            {
                Destroy(chipArchive[leaderPosX, leaderPosY], 0f);
            }
        }

        
        if(zeroizeGridX.Count > 0)
        {


            tempCount = Mathf.Min(zeroizeGridX.Count, 128);


            for (int c = 0; c < tempCount; c++)
            {
                fill(zeroizeGridX[c], zeroizeGridY[c]);
                
            }


            zeroizeGridX.RemoveRange(0, tempCount);
            zeroizeGridY.RemoveRange(0, tempCount);
            
        }

       
        if(remainingChips.Length <= nodeLocations.Length / 2)
        {
            Debug.Log("You Win");

            callTerminal.Hack();

            Cursor.visible = true;

            foreach (GameObject d in destroyables)
            {
                Destroy(d.gameObject, 0f);
            }

            foreach (GameObject c in remainingChips)
            {
                Destroy(c.gameObject, 0f);
            }

            Destroy(gameObject, 0f);
        }

        
    }


    private void fill(int posX, int posY)
    {
             
        //NE
        if (posX < 16 && posY < 11)
        {
            
            Destroy(chipArchive[posX + 1, posY + 1], 0f);

            if (grid[posX + 1, posY + 1] == 0 && chipArchive[posX + 1, posY + 1] != null)
            {
                
                zeroizeGridX.Add(posX + 1);
                zeroizeGridY.Add(posY + 1);
                
            }
        }

        //SE
        if (posX < 16 && posY > 1)
        {
            Destroy(chipArchive[posX + 1, posY - 1], 0f);

            if (grid[posX + 1, posY -1] == 0 && chipArchive[posX + 1, posY - 1] != null)
            {
                zeroizeGridX.Add(posX + 1);
                zeroizeGridY.Add(posY - 1);
                
            }
        }
        
        

        
        //NW
        if (posX > 1 && posY < 11)
        {
            Destroy(chipArchive[posX - 1, posY + 1], 0f);

            if (grid[posX - 1, posY +1] == 0 && chipArchive[posX - 1, posY + 1] != null)
            {
                zeroizeGridX.Add(posX - 1);
                zeroizeGridY.Add(posY + 1);

            }
        }

        //SW
        if (posX > 1 && posY > 1)
        {
            
            Destroy(chipArchive[posX - 1, posY - 1], 0f);

            if (grid[posX - 1, posY -1] == 0 && chipArchive[posX - 1, posY - 1] != null)
            {
                zeroizeGridX.Add(posX - 1);
                zeroizeGridY.Add(posY - 1);

            }
        }
        


        //W
        if (posX > 1)
        {
            if (chipArchive[posX - 1, posY] != null)
            {
                Destroy(chipArchive[posX - 1, posY], 0f);
            }

            if (grid[posX -1, posY] == 0 && chipArchive[posX - 1, posY] != null)
            {
                    zeroizeGridX.Add(posX - 1);
                    zeroizeGridY.Add(posY);

            }
        }

        //E
        if (posX < 16)
        {
            if (chipArchive[posX + 1, posY] != null)
            {
                Destroy(chipArchive[posX + 1, posY], 0f);
            }
            if (grid[posX + 1, posY] == 0 && chipArchive[posX + 1, posY] != null)
            {
                    zeroizeGridX.Add(posX + 1);
                    zeroizeGridY.Add(posY);

            }
        }

        //N
        if (posY < 11)
        {
            if (chipArchive[posX, posY + 1] != null)
            {
                Destroy(chipArchive[posX, posY + 1], 0f);
            }

            if (grid[posX, posY + 1] == 0 && chipArchive[posX, posY + 1] != null)
            {
                    zeroizeGridX.Add(posX);
                    zeroizeGridY.Add(posY + 1);
                        
            }
        }

        //S
        if (posY > 1)
        {
            if (chipArchive[posX, posY - 1] != null)
            {
                Destroy(chipArchive[posX, posY - 1], 0f);
            }

            if (grid[posX, posY -1 ] == 0 && chipArchive[posX, posY - 1] != null)
            {
 
                        zeroizeGridX.Add(posX);
                        zeroizeGridY.Add(posY - 1);
                        
            }
        }

        
    }

    
}



