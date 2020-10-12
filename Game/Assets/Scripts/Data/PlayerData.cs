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

        [Header("Animations")] 
        [SerializeField] private string m_idleClipName;
        [SerializeField] private string m_walkClipName;
        [SerializeField] private string m_runClipName;
        [SerializeField] private string m_startJumpClipName;
        [SerializeField] private string m_midAirClipName;
        [SerializeField] private string m_endJumpClipName;
        [SerializeField] private string m_dieClipName;
        [SerializeField] private string m_RespawnClipName;
        [SerializeField] private string m_cheerClipName;
        
        public AudioClip ShootingSound => this.m_shootingSound;
        public float JumpForce => this.m_jumpForce;
        public float WalkSpeed => this.m_walkSpeed;
        public float RunSpeed => this.m_runSpeed;
        public int Lives => this.m_lives;
        public float RotationSpeed => this.m_rotationSpeed;
        public AudioClip JumpSound => this.m_jumpSound;
        public AudioClip PowerUpSound => this.m_powerUpSound;
        public AudioClip VictorySound => this.m_victorySound;
        public string IdleClipName => this.m_idleClipName;
        public string WalkClipName => this.m_walkClipName;
        public string RunClipName => this.m_runClipName;
        public string StartJumpClipName => this.m_startJumpClipName;
        public string MidAirClipName => this.m_midAirClipName;
        public string EndJumpClipName => this.m_endJumpClipName;
        public string DieClipName => this.m_dieClipName;
        public string RespawnClipName => this.m_RespawnClipName;
        public string CheerClipName => this.m_cheerClipName;
    }
}