using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Player;
using Scripts;
using Segments;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public SpawnPoint CurrentSpawnPoint => this.m_currentSpawnPoint;
    
    private Segment[] m_segments;

    private Hazard[] m_hazardsWithoutSegment;

    private static GameManager s_instance;
    private ItemHolder[] m_itemHolders;
    private SpawnPoint m_currentSpawnPoint;
    public static GameManager Instance => s_instance ?? FindObjectOfType<GameManager>();

    private void Awake()
    {
        if (s_instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            s_instance = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
        SceneManager.sceneLoaded += this.OnsceneLoaded;
    }

    private void OnsceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        this.m_segments = FindObjectsOfType<Segment>();
        this.m_hazardsWithoutSegment = FindObjectsOfType<Hazard>().Where(h => h.GetComponentInParent<Segment>() == null).ToArray();
        this.m_itemHolders = FindObjectsOfType<ItemHolder>();
        this.m_currentSpawnPoint = FindObjectOfType<SpawnPoint>();
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayerController.Instance.Respawned += this.OnPlayerRespawned;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= this.OnsceneLoaded;
        // PlayerController.Instance.Respawned -= this.OnPlayerRespawned;
    }
    
    private void OnPlayerRespawned(object sender, System.EventArgs e)
    {
        this.ResetLevel();
    }

    private void ResetLevel()
    {
        foreach (var hazard in this.m_hazardsWithoutSegment)
            hazard.ResetHazard();

        foreach(var segment in this.m_segments)
            segment.ResetSegment();

        foreach (var itemHolder in this.m_itemHolders)
            itemHolder.ResetHolder();
        
    }
    
    public void LoadNextLevel()
    {
        var currentIndex = SceneManager.GetActiveScene().buildIndex;

        if (currentIndex + 1 < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(++currentIndex);
        else
            SceneManager.LoadScene(0);
    }
    
}
