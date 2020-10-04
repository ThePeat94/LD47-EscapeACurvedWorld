using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] private Transform m_player;
    
    private float m_initialZDistance;

    private static CameraFollower s_instance;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (s_instance == null)
            s_instance = this;
        else
            Destroy(s_instance.gameObject);
        
        this.m_initialZDistance = this.transform.position.z - this.m_player.position.z;
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        var targetPos = this.transform.position;
        targetPos.z = this.m_player.transform.position.z + this.m_initialZDistance;
        this.transform.position = targetPos;
    }
}
