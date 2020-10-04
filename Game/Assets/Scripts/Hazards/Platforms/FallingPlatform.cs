using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using Player;
using UnityEngine;

namespace Platforms
{
    public class FallingPlatform : Hazard
    {
        [SerializeField] private FallingPlatformData m_fallingPlatformData;
        [SerializeField] private Animator m_animator;

        private Vector3 m_startingPos;
        private WaitForSeconds m_waitForFalling;

        private Transform m_oldPlayerParent;

        private bool m_fallDown;

        private void Awake()
        {
            this.m_waitForFalling = new WaitForSeconds(this.m_fallingPlatformData.FallDelay);
        }

        private void Start()
        {
            this.m_startingPos = transform.position;
        }

        private void Update()
        {
            if (this.m_fallDown)
            {
                this.transform.Translate(Vector3.down * this.m_fallingPlatformData.FallSpeed * Time.deltaTime);
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            var player = other.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                this.m_oldPlayerParent = player.transform.parent;
                player.transform.SetParent(this.transform);
                StartCoroutine(this.Fall());
            }
        }

        private void OnCollisionExit(Collision other)
        {
            var player = other.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                player.transform.SetParent(this.m_oldPlayerParent);
                DontDestroyOnLoad(player.gameObject);
            }
        }

        private IEnumerator Fall()
        {
            this.m_animator.Play("Wiggle");
            yield return this.m_waitForFalling;
            this.m_animator.Play("Idle");
            this.m_fallDown = true;
        }

        public override void ResetHazard()
        {
            base.ResetHazard();
            this.m_fallDown = false;
            this.transform.position = this.m_startingPos;
        }
    }
}