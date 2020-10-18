using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject m_StartMenu;
    [SerializeField] private GameObject m_SelectPlayMode;
    [SerializeField] private GameObject m_ChooseLevel;

    public void BtnPlay_Click()
    {
        this.m_StartMenu.SetActive(false);
        this.m_SelectPlayMode.SetActive(true);        
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

    public void BtnStoryMode_Click()
    {
        this.m_SelectPlayMode.SetActive(false);
        this.m_ChooseLevel.SetActive(true);
    }

    public void BtnRandomMode_Click()
    {

    }

    public void BtnBackToStartScreen_Click()
    {
        this.m_SelectPlayMode.SetActive(false);
        this.m_StartMenu.SetActive(true);
    }

    public void BtnLvl_Click(int level)
    {
        SceneManager.LoadScene(level);
    }

    public void BtnBackToSelectPlayMode_Click()
    {
        this.m_ChooseLevel.SetActive(false);
        this.m_SelectPlayMode.SetActive(true);
        
    }
}
