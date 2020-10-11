using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{  
    public void BtnPlay_Click()
    {
        SceneManager.LoadScene(1);
    }

    public void BtnQuit_Click()
    {
        Application.Quit();
    }

    public void BtnHowTo_Click()
    {

    }

    public void BtnCredits_Click()
    {

    }
}
