using System;
using System.Linq;
using Data;
using Player;
using UnityEngine;
using UnityEngine.AI;

namespace DefaultNamespace
{
    public class Enemy : Hazard
    {
        [SerializeField] private EnemyData m_enemyData;
        private Animator m_animator;
        
        private bool m_isWandering;
        private bool m_isChasing;

        private Transform m_target;
        private Vector3 m_startingPos;
        private Quaternion m_startingRot;

        private NavMeshAgent m_navMeshAgent;

        private void Awake()
        {
            this.m_animator = this.GetComponent<Animator>();
            this.m_navMeshAgent = GetComponent<NavMeshAgent>();

            this.m_navMeshAgent.speed = this.m_enemyData.MovementSpeed;
        }

        private void Start()
        {
            PlayerController.Instance.Died += (sender, args) =>
            {
                if (args.Killer == this.gameObject)
                    this.m_animator.Play("Cheering");
                
                this.m_target = null;
            };

            this.m_startingPos = this.transform.position;
            this.m_startingRot = this.transform.rotation;
            
        }

        public void Update()
        {
            if (PlayerController.Instance.IsDead)
            {
                return;
            }


            if (this.m_target == null)
            {
                this.m_animator.SetFloat("Velocity", 0f);
                var colliders = Physics.OverlapSphere(this.transform.position, this.m_enemyData.ChasingDistance, 1 << LayerMask.NameToLayer("Player"));

                if (colliders.Length < 1)
                    return;

                this.m_target = colliders[0].transform;
                this.m_navMeshAgent.isStopped = false;
            }
            else
            {
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
            this.transform.position = this.m_startingPos;
            this.transform.rotation = this.m_startingRot;
            this.m_animator.SetFloat("Velocity", 0f);
            this.m_animator.Play("Idle");
        }
    }
}