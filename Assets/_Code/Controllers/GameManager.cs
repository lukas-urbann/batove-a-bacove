using UnityEngine;

namespace Controllers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Limits")]
        [SerializeField] private float minHappinessClamp = 0;
        [SerializeField] private float maxHappinessClamp = 1;
        
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

        public void CreateNewJudged()
        {
            
        }
    }
}