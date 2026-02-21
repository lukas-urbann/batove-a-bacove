using UnityEngine;

namespace Controllers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Manages Game Progression")]
        [SerializeField] private int day = 0;
        [Header("Limits")]
        [SerializeField] private float minHappinessClamp = 0;
        [SerializeField] private float maxHappinessClamp = 100;
        
        private float _poorPeopleHappiness;
        private float PoorPeopleHappiness
        {
            get => _poorPeopleHappiness;
            set
            {
                if (value > maxHappinessClamp)
                {
                    _poorPeopleHappiness = (int)maxHappinessClamp;
                }
                else if (value < minHappinessClamp)
                {
                    _poorPeopleHappiness = minHappinessClamp;
                }
                else
                {
                    _poorPeopleHappiness = value;
                }
            }
        }
        
        private float _richPeopleHappiness;
        public float RichPeopleHappiness
        {
            get => _richPeopleHappiness;
            set
            {
                if (value > maxHappinessClamp)
                {
                    _richPeopleHappiness = (int)maxHappinessClamp;
                }
                else if (value < minHappinessClamp)
                {
                    _richPeopleHappiness = minHappinessClamp;
                }
                else
                {
                    _richPeopleHappiness = value;
                }
            }
        }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        class Day
        {
            
        }
    }
}