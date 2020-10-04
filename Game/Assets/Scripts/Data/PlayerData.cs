using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "Data/Player Data")]
    public class PlayerData : ScriptableObject
    {

        [SerializeField] private float m_jumpForce;
        [SerializeField] private float m_walkSpeed;
        [SerializeField] private float m_runSpeed;
        [SerializeField] private int m_lives;
        [SerializeField] private float m_rotationSpeed;
        [SerializeField] private AudioClip m_jumpSound;
        [SerializeField] private AudioClip m_powerUpSound;
        [SerializeField] private AudioClip m_victorySound;
        [SerializeField] private AudioClip m_shootingSound;
        
        public AudioClip ShootingSound => this.m_shootingSound;
        public float JumpForce => this.m_jumpForce;

        public float WalkSpeed => this.m_walkSpeed;

        public float RunSpeed => this.m_runSpeed;

        public int Lives => this.m_lives;
        
        public float RotationSpeed => this.m_rotationSpeed;

        public AudioClip JumpSound => this.m_jumpSound;

        public AudioClip PowerUpSound => this.m_powerUpSound;

        public AudioClip VictorySound => this.m_victorySound;
    }
}