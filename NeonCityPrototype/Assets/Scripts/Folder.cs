using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Folder
{
    public string name;
    public string parentName;
    public string[] childrenName;
    public int[] ownership;
    public int storage;
}
