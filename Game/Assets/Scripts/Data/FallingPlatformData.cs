using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "Data/Falling Platform")]
    public class FallingPlatformData : ScriptableObject
    {
        [SerializeField] private float m_fallDelay;
        [SerializeField] private float m_fallSpeed;

        public float FallSpeed => this.m_fallSpeed;
        public float FallDelay => this.m_fallDelay;
    }
}