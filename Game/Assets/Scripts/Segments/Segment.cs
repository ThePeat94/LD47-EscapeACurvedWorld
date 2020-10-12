using System;
using System.Collections.Generic;
using UnityEngine;

namespace Segments
{
    public class Segment : MonoBehaviour
    {
        [SerializeField] protected Transform m_end;
        [SerializeField] protected Transform m_start;

        private Hazard[] m_hazards;
        
        public Transform SegmentEnd => this.m_end;
        public Transform SegmentStart => this.m_start;
        
        private void Start()
        {
            this.m_hazards = this.GetComponentsInChildren<Hazard>();
            this.DisableRenderForPuzzleSlot(this.m_end);
            this.DisableRenderForPuzzleSlot(this.m_start);
        }

        private void DisableRenderForPuzzleSlot(Transform slot)
        {
            var meshRenderer = slot.GetComponent<MeshRenderer>();
            if(meshRenderer != null)
                Destroy(meshRenderer);

            var meshFilter = slot.GetComponent<MeshFilter>();
            if (meshFilter != null)
                Destroy(meshFilter);

            var collider = slot.GetComponent<Collider>();
            if (collider != null)
                Destroy(collider);
        }

        public void ResetSegment()
        {
            if (this.m_hazards == null || this.m_hazards.Length == 0)
                return;
            
           foreach(var hazard in this.m_hazards)
               hazard.ResetHazard();
        }
    }
}
