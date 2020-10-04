using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "Data/Item Data")]
    public class ItemData : ScriptableObject
    {

        [SerializeField] private string m_name;
        [SerializeField] private Sprite m_icon;
        [SerializeField] private GameObject m_prefab;
        [SerializeField] private GameObject m_bullet;

        public string Name => this.m_name;
        public Sprite Icon => this.m_icon;
        public GameObject Prefab => this.m_prefab;
        public GameObject Bullet => this.m_bullet;
    }
}