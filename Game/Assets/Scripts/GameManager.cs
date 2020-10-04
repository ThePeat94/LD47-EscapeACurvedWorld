using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Player;
using Segments;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Segment[] m_segments;

    private Hazard[] m_hazardsWithoutSegment;

    private static GameManager s_instance;

    public static GameManager Instance => s_instance ?? FindObjectOfType<GameManager>();

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (s_instance == null)
            s_instance = this;
        else
            Destroy(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayerController.Instance.Died += OnPlayerDeath;
        PlayerController.Instance.Respawned += this.OnPlayerRespawned;
        PlayerController.Instance.ReachedGoal += OnPlayerReachedGoal;
        this.m_segments = FindObjectsOfType<Segment>();
        this.m_hazardsWithoutSegment = FindObjectsOfType<Hazard>().Where(h => h.GetComponentInParent<Segment>() == null).ToArray();
    }

    private void OnPlayerReachedGoal(object sender, System.EventArgs e)
    {
        this.ResetSegmentsAndHazards();
    }

    private void OnPlayerRespawned(object sender, System.EventArgs e)
    {
        this.ResetSegmentsAndHazards();
    }

    private void OnPlayerDeath(object sender, System.EventArgs e)
    {

    }

    private void ResetSegmentsAndHazards()
    {
        foreach (var hazard in this.m_hazardsWithoutSegment)
            hazard.ResetHazard();

        foreach(var segment in this.m_segments)
            segment.ResetSegment();
    }
}
