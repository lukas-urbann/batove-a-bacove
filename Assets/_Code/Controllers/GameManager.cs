using System;
using Dialogues;
using UnityEngine;

namespace Controllers
{
    public enum JudgedType
    {
        Poor = -1,
        Rich = 1
    }
    
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        
        public Action OnGamePause;
        public Action OnGameResume;

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

        //jen jednou po vstupu do game sceny
        private void Start()
        {
            DayManager.Instance.IncreaseDay();
        }

        public void CreateNewJudged()
        {
            //random judged type
            var judgedType = (JudgedType)UnityEngine.Random.Range(0, Enum.GetValues(typeof(JudgedType)).Length);
            DialogueManager.Instance.CreateNewDialogue(judgedType);
            
        }
        
        public void OnJudgingFinished()
        {
            if (Mathf.Approximately(RichPeopleHappiness, minHappinessClamp) || Mathf.Approximately(PoorPeopleHappiness, minHappinessClamp))
            {
                AnnounceGameOver();
            }
        }
        
        public void AnnounceGameOver()
        {
            
        }
    }
}