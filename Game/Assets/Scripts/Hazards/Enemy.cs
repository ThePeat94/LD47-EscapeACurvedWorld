using System;
using Data;
using EventArgs;
using Player;
using UnityEngine;
using UnityEngine.AI;

namespace Hazards
{
    public class Enemy : Hazard
    {
        [SerializeField] private EnemyData m_enemyData;
        [SerializeField] private AudioSource m_audioSource;
        private Animator m_animator;
    
        private bool m_isDead;
        private bool m_isAbleToChase = true;
        private Collider m_collider;
    
        private Transform m_target;
        private Vector3 m_startingPos;
        private Quaternion m_startingRot;

        private NavMeshAgent m_navMeshAgent;

        private Rigidbody m_rigidbody;

        private void Awake()
        {
            this.m_animator = this.GetComponent<Animator>();
            this.m_navMeshAgent = this.GetComponent<NavMeshAgent>();
            this.m_collider = this.GetComponent<Collider>();
            this.m_navMeshAgent.speed = this.m_enemyData.MovementSpeed;
            this.m_rigidbody = this.GetComponent<Rigidbody>();
        }

        private void Start()
        {
            PlayerController.Instance.Died += PlayerDied;

            this.m_startingPos = this.transform.position;
            this.m_startingRot = this.transform.rotation;
            this.m_audioSource.clip = this.m_enemyData.ZombieIdle;
            this.m_audioSource.Play();

        }

        private void PlayerDied(object sender, PlayerDiedEventArgs args)
        {
            if (args.Killer == this.gameObject)
            {
                this.TryPlayAudioClip(this.m_enemyData.ZombieVictory,false);
                this.m_animator.Play("Cheering");
            }
            this.m_target = null;
            this.m_navMeshAgent.SetDestination(this.transform.position);
            this.m_navMeshAgent.isStopped = true;
            this.m_rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            this.m_rigidbody.velocity = Vector3.zero;
            this.m_isAbleToChase = false;
        }

        public void Update()
        {
            if (this.m_isDead || !this.m_isAbleToChase)
            {
                return;
            }
        
            if (this.m_target == null)
            {
                this.TryPlayAudioClip(this.m_enemyData.ZombieIdle, true);
                this.m_animator.SetFloat("Velocity", 0f);
                var colliders = Physics.OverlapSphere(this.transform.position, this.m_enemyData.ChasingDistance, 1 << LayerMask.NameToLayer("Player"));

                if (colliders.Length < 1)
                    return;

                this.m_target = colliders[0].transform;
                this.m_navMeshAgent.isStopped = false;
            }
            else
            {
                this.TryPlayAudioClip(this.m_enemyData.ZombieChase, true);
                this.m_navMeshAgent.SetDestination(this.m_target.position);
                this.m_animator.SetFloat("Velocity", 1f);
                if (this.m_navMeshAgent.remainingDistance > this.m_enemyData.ChasingDistance)
                {
                    this.m_target = null;
                    this.m_navMeshAgent.isStopped = true;
                    this.m_animator.SetFloat("Velocity", 0f);
                    return;
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(this.transform.position, this.m_enemyData.ChasingDistance);
        }

        public override void ResetHazard()
        {
            base.ResetHazard();
            this.m_isDead = false;
            this.transform.position = this.m_startingPos;
            this.transform.rotation = this.m_startingRot;
            this.m_rigidbody.constraints = RigidbodyConstraints.None;
            this.m_animator.SetFloat("Velocity", 0f);
            this.m_animator.Play("Idle");
            this.m_collider.enabled = true;
            this.m_isAbleToChase = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            var bullet = other.GetComponent<Bullet>();
            if (bullet != null)
            {
                this.HandleDeath();
                Destroy(bullet.gameObject);
            }
        }

        private void HandleDeath()
        {
            this.m_rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            this.TryPlayAudioClip(this.m_enemyData.ZombieDie, false);
            this.m_isDead = true;
            this.m_animator.Play("Die");
            this.m_navMeshAgent.isStopped = true;
            this.m_collider.enabled = false;
        }


        private void TryPlayAudioClip(AudioClip clipToPlay, bool loop)
        {
            if (this.m_audioSource.clip != clipToPlay)
            {
                this.m_audioSource.loop = loop;
                this.m_audioSource.clip = clipToPlay;
                this.m_audioSource.Play();
            }
        }

        private void OnDestroy()
        {
            PlayerController.Instance.Died -= this.PlayerDied;
        }
    }
}