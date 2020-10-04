using System;
using Data;
using UnityEngine;

namespace Scripts
{
    public class ItemHolder : MonoBehaviour
    {
        [SerializeField] private ItemData m_data;

        private GameObject m_instantiated;
        
        private void Awake()
        {
            this.LoadItem();
            this.DisableRenderForItemHolder(this.transform);
        }
        
        private void DisableRenderForItemHolder(Transform slot)
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

        private void LoadItem()
        {
            this.m_instantiated = Instantiate(this.m_data.Prefab, this.transform.position, Quaternion.identity, this.transform);

            this.m_instantiated.GetComponent<Item>(). Data = this.m_data;
        }

        public void ResetHolder()
        {
            Destroy(this.m_instantiated);
            this.LoadItem();
        }
    }
}