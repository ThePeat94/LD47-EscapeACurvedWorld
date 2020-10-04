using System;
using System.Collections;
using Data;
using UnityEngine;

namespace Hazards
{
    public class ElementarGun : MonoBehaviour
    {
        [SerializeField] private ElementarGunData m_data;
        [SerializeField] private GameObject m_barrelEnd;
        
        private WaitForSeconds m_waitForDelay;
        private WaitForSeconds m_waitForShoot;
        private ParticleSystem m_instantiatedParticleSystem;
        private Coroutine m_currentCoroutine;
        
        private void Awake()
        {
            this.m_waitForDelay = new WaitForSeconds(this.m_data.DelayBetweenFire);
            this.m_waitForShoot = new WaitForSeconds(this.m_data.DelayBetweenFire);

            this.m_instantiatedParticleSystem = Instantiate(this.m_data.ParticleSystem, this.m_barrelEnd.transform).GetComponent<ParticleSystem>();
        }

        private void Start()
        {
            this.m_currentCoroutine = this.StartCoroutine(this.Shoot());
        }

        private void Update()
        {
            if(this.m_currentCoroutine == null)             
                this.m_currentCoroutine = this.StartCoroutine(this.Shoot());
        }

        private IEnumerator Shoot()
        {
            yield return this.m_waitForDelay;
            this.m_instantiatedParticleSystem.Play();
            yield return this.m_waitForShoot;
            this.m_instantiatedParticleSystem.Stop();
            this.m_currentCoroutine = null;
        }
    }
}