using System;
using System.Collections;
using EventArgs;
using Player;
using UnityEngine;

namespace Scripts
{
    public class MusicPlayer : MonoBehaviour
    {
        [SerializeField] private AudioSource m_audioSource;
        [SerializeField] private AudioClip m_gameLoopTrack;
        [SerializeField] private AudioClip m_gameOverSound;

        private static MusicPlayer s_instance;

        private void Awake()
        {
            if (s_instance == null)
            {
                DontDestroyOnLoad(this.gameObject);
                s_instance = this;
            }
            else
            {
                Destroy(this.gameObject);
                return;
            }
        }

        private void Start()
        {
            PlayerController.Instance.Died += PlayerDied;
        }

        private void PlayerDied(object sender, PlayerDiedEventArgs e)
        {
            this.m_audioSource.clip = this.m_gameOverSound;
            this.m_audioSource.volume = 1f;
            this.m_audioSource.Play();
            StartCoroutine(this.PlayLoopAfterGameOver());
        }

        private IEnumerator PlayLoopAfterGameOver()
        {
            yield return new WaitForSeconds(this.m_gameOverSound.length);
            this.m_audioSource.clip = this.m_gameLoopTrack;
            this.m_audioSource.volume = 0.1f;
            this.m_audioSource.Play();
        }
    }
}