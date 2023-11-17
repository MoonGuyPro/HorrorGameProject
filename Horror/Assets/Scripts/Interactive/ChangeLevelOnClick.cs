using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeLevelOnClick : Interactive
{
    public string levelName;
    
    public override bool Interact(OldInventory inv)
    {
        SceneManager.LoadScene(levelName);
        return true;
    }
}
