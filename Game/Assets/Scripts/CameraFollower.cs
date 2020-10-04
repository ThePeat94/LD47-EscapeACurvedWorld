﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] private Transform m_player;

    // TODO: Calculate
    private float m_initialZDistance;

    private void Start()
    {
        this.m_initialZDistance = this.transform.position.z - this.m_player.position.z;
        Debug.Log(this.m_initialZDistance);
    }

    // Update is called once per frame
    void Update()
    {
        var targetPos = this.transform.position;
        targetPos.z = this.m_player.transform.position.z + this.m_initialZDistance;
        this.transform.position = targetPos;
    }
}