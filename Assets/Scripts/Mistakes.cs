using System.Collections.Generic;
using UnityEngine;

public class Mistakes
{
    public bool WrongDoor { get => PlayerPrefs.GetInt("wrongDoor") == 0; set 
    {
        PlayerPrefs.SetInt("wrongDoor", value ? 0 : 1);
        PlayerPrefs.Save();
    } 
    }
}
