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
    private Segment[] m_segments;

    private Hazard[] m_hazardsWithoutSegment;

    private static GameManager s_instance;
    private ItemHolder[] m_itemHolders;
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
        SceneManager.sceneLoaded +=OnsceneLoaded;
    }

    private void OnsceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        this.m_segments = FindObjectsOfType<Segment>();
        this.m_hazardsWithoutSegment = FindObjectsOfType<Hazard>().Where(h => h.GetComponentInParent<Segment>() == null).ToArray();
        this.m_itemHolders = FindObjectsOfType<ItemHolder>();
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayerController.Instance.Died += OnPlayerDeath;
        PlayerController.Instance.Respawned += this.OnPlayerRespawned;
        PlayerController.Instance.ReachedGoal += OnPlayerReachedGoal;


    }

    private void OnPlayerReachedGoal(object sender, System.EventArgs e)
    {
        this.ResetLevel();
    }

    private void OnPlayerRespawned(object sender, System.EventArgs e)
    {
        this.ResetLevel();
    }

    private void OnPlayerDeath(object sender, System.EventArgs e)
    {

    }

    private void ResetItemHolders()
    {
        
    }

    private void ResetLevel()
    {
        foreach (var hazard in this.m_hazardsWithoutSegment)
            hazard.ResetHazard();

        foreach(var segment in this.m_segments)
            segment.ResetSegment();

        foreach (var itemHolder in this.m_itemHolders)
        {
            itemHolder.ResetHolder();
        }
    }
}
