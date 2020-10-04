using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "Data/Moving Platform")]
    public class MovingPlatformData : ScriptableObject
    {
        [SerializeField] private float m_movementSpeed;
        [SerializeField] private float m_movementDelay;

        public float MovementSpeed
        {
            get => this.m_movementSpeed;
            set => this.m_movementSpeed = value;
        }

        public float MovementDelay
        {
            get => this.m_movementDelay;
            set => this.m_movementDelay = value;
        }
    }
}