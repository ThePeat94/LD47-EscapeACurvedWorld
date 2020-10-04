using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "Data/Enemy Data")]
    public class EnemyData : ScriptableObject
    {
        [SerializeField] private float m_movementSpeed;
        [SerializeField] private float m_chasingDistance;
        [SerializeField] private AudioClip m_zombieIdle;
        [SerializeField] private AudioClip m_zombieChase;
        [SerializeField] private AudioClip m_zombieDie;
        [SerializeField] private AudioClip m_zombieVictory;

        public AudioClip ZombieIdle => this.m_zombieIdle;

        public AudioClip ZombieChase => this.m_zombieChase;

        public AudioClip ZombieDie => this.m_zombieDie;

        public AudioClip ZombieVictory => this.m_zombieVictory;

        public float MovementSpeed => this.m_movementSpeed;

        public float ChasingDistance => this.m_chasingDistance;
    }
}