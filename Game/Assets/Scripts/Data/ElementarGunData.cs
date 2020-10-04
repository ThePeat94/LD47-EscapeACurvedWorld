using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "Data/Elementar Gun Data")]
    public class ElementarGunData : ScriptableObject
    {
        [SerializeField] private float m_delayBetweenFire;
        [SerializeField] private float m_fireDuration;
        [SerializeField] private GameObject m_particleSystem;
        [SerializeField] private GameObject m_bullet;

        public float DelayBetweenFire => this.m_delayBetweenFire;

        public float FireDuration => this.m_fireDuration;

        public GameObject ParticleSystem => this.m_particleSystem;

        public GameObject Bullet => this.m_bullet;
    }
}