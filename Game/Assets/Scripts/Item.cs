using System;
using Data;
using UnityEngine;

namespace Scripts
{
    public class Item : MonoBehaviour
    {
        public ItemData Data
        {
            get => this.m_data;
            set => this.m_data = value;
        }
        private ItemData m_data;
    }
}