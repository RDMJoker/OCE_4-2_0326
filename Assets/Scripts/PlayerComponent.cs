using System;
using UnityEngine;

namespace Scripts
{
    [RequireComponent(typeof(HealthSystem))]
    public class PlayerComponent : MonoBehaviour, IDamageable
    {
        HealthSystem healthSystem;

        void Awake()
        {
            healthSystem = GetComponent<HealthSystem>();
        }
        
        public void TakeDamage(float _damageValue)
        {
            healthSystem.TakeDamage(_damageValue);
        }
    }
}