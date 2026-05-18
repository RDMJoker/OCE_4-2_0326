using UnityEngine;

namespace Scripts
{
    public class HealthSystem : MonoBehaviour
    {
        [SerializeField] float maximumHP;

        float currentHP;
        
        public float CurrentHP
        {
            get => currentHP;
            private set
            {
                if (Mathf.Approximately(value, CurrentHP)) return;
                currentHP = value;
                Debug.Log("New HP: " + CurrentHP);
            }
        }

        void Awake()
        {
            CurrentHP = maximumHP;
        }

        public void TakeDamage(float _damageValue)
        {
            CurrentHP =  Mathf.Max(0,  CurrentHP - _damageValue);
        }
    }
}