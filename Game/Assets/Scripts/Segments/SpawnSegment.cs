using UnityEngine;

namespace Segments
{
    public class SpawnSegment : Segment
    {
        [SerializeField] private Transform m_spawnPoint;

        public Transform SpawnPoint => this.m_spawnPoint;
    }
}