using Enemies.BasicEnemy.HealthRelated.Bases;
using UnityEngine;
using UnityEngine.Events;

namespace Enemies.BasicEnemy.HealthRelated
{
    public class Health : MonoBehaviour, IDamageable
    {
        [Header("Parameters")]
        [SerializeField] private uint maxHealth;
        [SerializeField] [Range(0, 100)] private int initialHealthPercentage;
        [Space]
        public UnityEvent <int> HealthChanged;
        [Space]
        public UnityEvent Died;
        
        private bool _hasDied;
        private int _currentHealth;
        
        public bool IsDead => _hasDied;
        
        public int CurrentHealth
        {
            get => _currentHealth;
            set
            {
                if (value < 0)
                {
                    _currentHealth = 0;

                    if (!_hasDied)
                    {
                        _hasDied = true;
                        Died.Invoke();
                    }
                }
                else if (value > maxHealth)
                {
                    _currentHealth = (int)maxHealth;
                }
                else
                {
                    _currentHealth = value;
                }
                
                HealthChanged.Invoke(_currentHealth);
            }
        }

        private void Awake()
        {
            CurrentHealth = (int)((initialHealthPercentage / 100) * maxHealth);
        }

        public void ReceiveDamage(int damage)
        {
            CurrentHealth -= damage;
        }
    }
}
