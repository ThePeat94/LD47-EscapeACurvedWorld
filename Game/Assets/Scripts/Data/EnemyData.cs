using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "Data/Enemy Data")]
    public class EnemyData : ScriptableObject
    {
        [SerializeField] private float m_movementSpeed;
        [SerializeField] private float m_chasingDistance;

        public float MovementSpeed => this.m_movementSpeed;

        public float ChasingDistance => this.m_chasingDistance;
    }
}