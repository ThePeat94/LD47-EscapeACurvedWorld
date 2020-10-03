using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] private Transform m_player;
    // Update is called once per frame
    void Update()
    {
        var targetPos = this.transform.position;
        targetPos.z = this.m_player.transform.position.z + 5f;
        this.transform.position = targetPos;
    }
}
