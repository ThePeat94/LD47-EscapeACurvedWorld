using UnityEngine;

namespace EventArgs
{
    public class PlayerDiedEventArgs : System.EventArgs
    {
        public GameObject Killer { get; }

        public PlayerDiedEventArgs(GameObject killer)
        {
            this.Killer = killer;
        }
    }
}