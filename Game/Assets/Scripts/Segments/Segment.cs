using UnityEngine;

namespace Segments
{
    public class Segment : MonoBehaviour
    {
        [SerializeField] protected Transform m_end;
        [SerializeField] protected Transform m_start;

        public Transform End => this.m_end;
        public Transform Start => this.m_start;
    }
}
