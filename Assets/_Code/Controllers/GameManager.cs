using System;
using System.Collections;
using Dialogues;
using UnityEngine;

namespace Controllers
{
    public enum JudgedType
    {
        Poor = 0,
        Rich = 1
    }
    
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        
        public Action OnGamePause;
        public Action OnGameResume;
        
        [Header("Timers")]
        [SerializeField] private float daySwitchTime = 4;

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

        public DialogueWriter sentenceWriter;
        
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

        private IEnumerator NewDayTimeoff()
        {
            RemoveJudged();
            yield return new WaitForSeconds(daySwitchTime);
            CreateNewJudged();
        }
        
        //jen jednou po vstupu do game sceny
        private void Start()
        {
            DayManager.Instance.IncreaseDay();
            StartCoroutine(NewDayTimeoff());
        }

        public void CreateNewJudged()
        {
            //random judged type
            var judgedType = (JudgedType)UnityEngine.Random.Range(0, Enum.GetValues(typeof(JudgedType)).Length);
            var d = DialogueManager.Instance.CreateNewDialogue(judgedType);
            sentenceWriter.WriteSentence(d.Excerpt.Item1);
        }
        
        public void RemoveJudged()
        {
            
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