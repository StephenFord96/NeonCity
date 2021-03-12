using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HackingMiniGame : MonoBehaviour

{
    private PlayerController player;
    private TerminalController terminal;
    private GameObject[] allTerminals;

    public string input;
    public InputField playerInput;
    public Text[] lines;
    public Text mission;
    public int currentLine;
    public int dataID;
    public string currentFolder;

    // Start is called before the first frame update
    void Start()
    {
        currentFolder = ("Main");


        playerInput.text = ("");
        currentLine = 1;

        lines[0].text = ("Type Help for a list of commands. Commands ARE case sensetive");

        playerInput.ActivateInputField();
        //playerInput.text = ("123");

        allTerminals = GameObject.FindGameObjectsWithTag("Terminal");

        foreach (GameObject t in allTerminals)
        {
            terminal = t.GetComponent<TerminalController>();
            if(terminal.playerHacking == true)
            {
                dataID = terminal.dataCacheID;
                mission.text = "Data cache's ID: " + (dataID.ToString());
                break;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            player = FindObjectOfType<PlayerController>();
            Destroy(gameObject, 0f);
        }

    }

    public void ReadPlayerInput(string s)
    {

        input = s;

        //bump and scroll
        while(currentLine >= 20)
        {
            
            for (int x = 0; x < lines.Length-1; x++)
            {
                lines[x].text = lines[x + 1].text;
            }

            currentLine = currentLine - 1;
        }

        lines[currentLine].text = input;
        currentLine = currentLine + 2;


        string[] commandPhrase = input.Split(' ');

        if (commandPhrase.Length == 1)
        {

            if (input == "help")
            {
                Invoke("Help", 0f);
            }
            else if (input == "clear")
            {
                Invoke("Clear", 0f);
            }
            else if (input == "download")
            {
                Invoke("Download", 0f);
            }
            else if (input == "main")
            {
                Invoke("Main", 0f);
            }
            else if (input == "localscan")
            {
                Invoke("LocalScan", 0f);
            }
            else
            {
                //bump and scroll
                while (currentLine >= 20)
                {

                    for (int x = 0; x < lines.Length - 1; x++)
                    {
                        lines[x].text = lines[x + 1].text;
                    }

                    currentLine = currentLine - 1;
                }

                lines[currentLine].text = ("INVALID SYNTAX");
                currentLine = currentLine + 2;
            }
                    
        }
        else if (commandPhrase.Length == 2)
        {

            if (commandPhrase[0] == ("open"))
            {
                Open(commandPhrase[1]);
            }else if (commandPhrase[0] == ("directscan"))
            {
                DirectScan(commandPhrase[1]);
            }
            else
            {
                //bump and scroll
                while (currentLine >= 20)
                {

                    for (int x = 0; x < lines.Length - 1; x++)
                    {
                        lines[x].text = lines[x + 1].text;
                    }

                    currentLine = currentLine - 1;
                }

                lines[currentLine].text = ("INVALID SYNTAX");
                currentLine = currentLine + 2;
            }
        }
        else if (commandPhrase.Length > 2)
        {
            //bump and scroll
            while (currentLine >= 20)
            {

                for (int x = 0; x < lines.Length - 1; x++)
                {
                    lines[x].text = lines[x + 1].text;
                }

                currentLine = currentLine - 1;
            }

            lines[currentLine].text = ("INVALID SYNTAX");
            currentLine = currentLine + 2;
        }



        input = "";
        playerInput.text = ("");

        playerInput.ActivateInputField();
    }

    public void Help()
    {
        //bump and big scroll
        while (currentLine >= 14)
        {

            for (int x = 0; x < lines.Length - 1; x++)
            {
                lines[x].text = lines[x + 1].text;
            }

            currentLine = currentLine - 1;
        }

        lines[currentLine].text = ("download  :  Use inside the folder that has the data cache");
        currentLine = currentLine + 1;

        lines[currentLine].text = ("open  :  Will open the specified folder by name");
        currentLine = currentLine + 1;

        lines[currentLine].text = ("localscan  :  Will return the user all the folder and their names from current folder");
        currentLine = currentLine + 1;

        lines[currentLine].text = ("directscan  :  Will return the content ID of the specified folder");
        currentLine = currentLine + 1;

        lines[currentLine].text = ("main  :  Will return the user to the initial folder");
        currentLine = currentLine + 1;

        lines[currentLine].text = ("clear  :  Will erase all prior terminal messages");
        currentLine = currentLine + 2;
    }

    public void Clear()
    {
        for (int c = 0; c < lines.Length; c++)
        {
            lines[c].text = ("");
        }

        currentLine = 0;
    }

    public void Download()
    {
        //bump and scroll
        while (currentLine >= 20)
        {

            for (int x = 0; x < lines.Length - 1; x++)
            {
                lines[x].text = lines[x + 1].text;
            }

            currentLine = currentLine - 1;
        }

        for (int f = 0; f < 40; f++)
        {
            if (terminal.allFolders[f].name == currentFolder)
            {
                if (terminal.allFolders[f].storage == 1)
                {
                    lines[currentLine].text = ("://DOWNLOADING DATA CACHE");
                    currentLine = currentLine + 2;
                    terminal.Invoke("Win", 1f);
                    Destroy(gameObject, 0f);
                    break;
                }else if(terminal.allFolders[f].storage == 2)
                {
                    lines[currentLine].text = ("://DOWNLOADING MALWARE");
                    currentLine = currentLine + 2;
                    break;
                }
                else
                {
                    lines[currentLine].text = (">>>FOLDER HAS NO DOWNLOADABLE CONTENT<<<");
                    currentLine = currentLine + 2;
                    break;
                }
            }
        }

    }

    public void Main()
    {
        currentFolder = ("Main");

        //bump and scroll
        while (currentLine >= 20)
        {

            for (int x = 0; x < lines.Length - 1; x++)
            {
                lines[x].text = lines[x + 1].text;
            }

            currentLine = currentLine - 1;
        }

        lines[currentLine].text = ("://RETURNING TO MAIN");
        currentLine = currentLine + 2;
    }

    public void LocalScan()
    {
        //bump and big scroll
        while (currentLine >= 15)
        {

            for (int x = 0; x < lines.Length - 1; x++)
            {
                lines[x].text = lines[x + 1].text;
            }

            currentLine = currentLine - 1;
        }


        for(int f = 0; f < 40; f++)
        {
            if(terminal.allFolders[f].name == currentFolder && terminal.allFolders[f].childrenName.Length > 0)
            {
                int d = 0;

                if(f >= 1 && f <= 3)
                {
                    d = 1;
                }else if(f >= 4 && f <= 12)
                {
                    d = 2;
                }else if(f >= 13)
                {
                    d = 3;
                }

                lines[currentLine].text = ("://SCANNING>>>  " + currentFolder + " AT DEPTH " + d);
                currentLine = currentLine + 2;

                lines[currentLine].text = terminal.allFolders[f].childrenName[0];
                currentLine = currentLine + 1;
                lines[currentLine].text = terminal.allFolders[f].childrenName[1];
                currentLine = currentLine + 1;
                lines[currentLine].text = terminal.allFolders[f].childrenName[2];
                currentLine = currentLine + 2;
                break;
            }
        }
    }

    public void Open(string folderToOpen)
    {
        //bump and scroll
        while (currentLine >= 20)
        {

            for (int x = 0; x < lines.Length - 1; x++)
            {
                lines[x].text = lines[x + 1].text;
            }

            currentLine = currentLine - 1;
        }

        for (int f = 0; f < 40; f++)
        {
            if(terminal.allFolders[f].name == folderToOpen)
            {
                currentFolder = folderToOpen;
                lines[currentLine].text = ("://OPENING " + folderToOpen);
                currentLine = currentLine + 2;
                break;
            }else if(f == 39)
            {
                lines[currentLine].text = ("://FOLDER " + folderToOpen + " DOES NOT EXIST");
                currentLine = currentLine + 2;
            }
        }
    }

    public void DirectScan(string folderToScan)
    {

        //bump and scroll
        while (currentLine >= 20)
        {

            for (int x = 0; x < lines.Length - 1; x++)
            {
                lines[x].text = lines[x + 1].text;
            }

            currentLine = currentLine - 1;
        }

        for (int f = 0; f < 40; f++)
        {
            if (terminal.allFolders[f].name == folderToScan)
            {
                lines[currentLine].text = ("://SCANNING " + folderToScan);
                currentLine = currentLine + 2;

                //bump and scroll for extra text
                while (currentLine >= 20)
                {

                    for (int x = 0; x < lines.Length - 1; x++)
                    {
                        lines[x].text = lines[x + 1].text;
                    }

                    currentLine = currentLine - 1;
                }

                if(terminal.allFolders[f].storage == 1)
                {
                    lines[currentLine].text = ("://FOLDER CONTAINS " + (dataID.ToString()));
                    currentLine = currentLine + 2;
                }else if(terminal.allFolders[f].storage == 0)
                {
                    lines[currentLine].text = ("://FOLDER CONTAINS NOTHING");
                    currentLine = currentLine + 2;
                }
                else if (terminal.allFolders[f].storage == 2)
                {
                    int falsePositive = dataID + Mathf.RoundToInt(Mathf.Pow(10f, (Random.Range(0, 5))));

                    lines[currentLine].text = ("://FOLDER CONTAINS " + (falsePositive.ToString()));
                    currentLine = currentLine + 2;
                }
                break;
            }
            else if (f == 39)
            {
                lines[currentLine].text = ("://FOLDER " + folderToScan + " DOES NOT EXIST");
                currentLine = currentLine + 2;
            }
        }
    }

}
