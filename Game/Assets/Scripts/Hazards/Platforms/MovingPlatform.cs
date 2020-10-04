using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using Player;
using UnityEngine;

public class MovingPlatform : Hazard
{
    [SerializeField] private Transform m_start;
    [SerializeField] private Transform m_end;
    [SerializeField] private MovingPlatformData m_data;

    private WaitForSeconds m_waitForDelay;
    
    private Transform m_currentTarget;

    private void Start()
    {
        this.transform.position = this.m_start.position;
        this.m_waitForDelay = new WaitForSeconds(this.m_data.MovementDelay);
        this.m_currentTarget = this.m_end;

        this.m_start.gameObject.SetActive(false);
        this.m_end.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (this.m_currentTarget == null)
            return;
        
        var distance = Vector3.Distance(this.transform.position, this.m_currentTarget.position);
        if (Mathf.Abs(distance) <= 0.5f)
        {
            StartCoroutine(this.DelayMoving());
            return;
        }

        var moveVector = this.m_currentTarget.transform.position - this.transform.position;

        this.transform.Translate(moveVector.normalized * this.m_data.MovementSpeed * Time.deltaTime);
    }

    private IEnumerator DelayMoving()
    {
        var lastTarget = this.m_currentTarget.gameObject;
        this.m_currentTarget = null;
        yield return this.m_waitForDelay;
        this.m_currentTarget = lastTarget == this.m_end.gameObject ? this.m_start : this.m_end;
    }
    
    private void OnCollisionEnter(Collision other)
    {
        var player = other.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            player.transform.SetParent(this.transform);
        }
    }

    private void OnCollisionExit(Collision other)
    {
        var player = other.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            player.transform.SetParent(null);
        }
    }

    public override void ResetHazard()
    {
        base.ResetHazard();
        this.m_currentTarget = this.m_end;
        this.transform.position = this.m_start.position;
    }
}
