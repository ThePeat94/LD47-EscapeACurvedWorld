using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "Player Data", menuName = "Data", order = 0)]
    public class PlayerData : ScriptableObject
    {
        public float JumpForce => this.m_jumpForce;

        public float WalkSpeed => this.m_walkSpeed;

        public float RunSpeed => this.m_runSpeed;

        public int Lives => this.m_lives;
        
        public float RotationSpeed => this.m_rotationSpeed;
        
        [SerializeField] private float m_jumpForce;
        [SerializeField] private float m_walkSpeed;
        [SerializeField] private float m_runSpeed;
        [SerializeField] private int m_lives;
        [SerializeField] private float m_rotationSpeed;


    }
}