using System;
using System.Collections;
using Dialogues;
using UnityEngine;
using UnityEngine.Events;

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
        [SerializeField] private float workDayTime = 90;
        [SerializeField] private float responseTime = 2.5f;
        [SerializeField] private float noJudgedTimeout = 2f;

        [Header("Limits")]
        [SerializeField] private float minHappinessClamp = 0;
        [SerializeField] private float maxHappinessClamp = 1;
        
        [Header("Coins")]
        private int _coins;
        public int Coins
        {
            get => _coins;
            set
            {
                _coins += value;

                if (_coins < 0)
                {
                    TriggerGameOver();
                }
            }
        }
        
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
                    _poorPeopleHappiness += value;
                }
            }
        }
        
        private float _richPeopleHappiness;

        private float RichPeopleHappiness
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
                    _richPeopleHappiness += value;
                }
            }
        }

        public DialogueWriter sentenceWriter;
        public DialogueWriter responseWriter;
        private DialogueBias _currentBiasCase = null;

        //jen pro vizual, problikava to tam pri startu
        public GameObject hiddenPanelFirstStart;

        public UnityEvent onJudgedReady;
        public UnityEvent onJudgedDismissed;
        
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
            AdvanceDay();
            StartCoroutine(HidePanelFirstStart());
        }

        private void AdvanceDay()
        {
            DayManager.Instance.IncreaseDay();
            onJudgedDismissed?.Invoke();
            StartCoroutine(FirstJudgedTimeout());
        }

        private void AfterJudging()
        {
            onJudgedDismissed?.Invoke();
            StartCoroutine(NoJudgedTimer());
        }

        private void CreateNewJudged()
        {
            var judgedType = (JudgedType)UnityEngine.Random.Range(0, Enum.GetValues(typeof(JudgedType)).Length);
            var d = DialogueManager.Instance.CreateNewDialogue(judgedType);
            onJudgedReady?.Invoke();
            sentenceWriter.WriteSentence(d.Excerpt.Item1);
            StartCoroutine(ResponseWriteDelay(d.Response.text));
            _currentBiasCase = d.Bias;
        }

        public void JudgeResponse(bool forConviction)
        {
            PoorPeopleHappiness += forConviction ? -_currentBiasCase.poor : _currentBiasCase.poor;
            RichPeopleHappiness += forConviction ? -_currentBiasCase.rich : _currentBiasCase.rich;
            Debug.Log($"Judged for {(forConviction ? "conviction" : "dismissal")}. Poor happiness: {PoorPeopleHappiness}, Rich happiness: {RichPeopleHappiness}");
            AfterJudging();
        }

        private void TriggerGameOver()
        {
            
        }
        
        //Timed
        private IEnumerator FirstJudgedTimeout()
        {
            yield return new WaitForSeconds(daySwitchTime);
            StartCoroutine(InGameDayTimer());
            CreateNewJudged();
        }

        private IEnumerator InGameDayTimer()
        {
            yield return new WaitForSeconds(workDayTime);
            AdvanceDay();
        }
        
        private IEnumerator HidePanelFirstStart()
        {
            yield return new WaitForSeconds(1.2f);
            hiddenPanelFirstStart.SetActive(false);
        }
        
        private IEnumerator NoJudgedTimer()
        {
            yield return new WaitForSeconds(noJudgedTimeout);
            CreateNewJudged();
        }
        
        private IEnumerator ResponseWriteDelay(string response)
        {
            yield return new WaitForSeconds(responseTime);
            responseWriter.WriteSentence(response);
        }
    }
}