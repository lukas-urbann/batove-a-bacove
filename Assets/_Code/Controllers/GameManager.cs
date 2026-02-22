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
        public float workDayTime = 90;
        [SerializeField] private float responseTime = 2.5f;
        [SerializeField] private float noJudgedTimeout = 2f;

        [Header("Limits")]
        [SerializeField] private float minHappinessClamp = 0;
        [SerializeField] private float maxHappinessClamp = 1;
        
        [Header("Coins")]
        private int _coins = 30;
        
        public void AddCoins(int amount)
        {
            _coins += amount;
            onCoinsUpdated?.Invoke(_coins);
        }
        
        private float _poorPeopleHappiness = 1;
        
        private float _richPeopleHappiness = 1;
        
        public void SetRichPeopleHappiness(float value)
        {
            _richPeopleHappiness = Mathf.Clamp(value, minHappinessClamp, maxHappinessClamp);
            onRichHappinessUpdated?.Invoke(_richPeopleHappiness);
            CheckHappinessStatus();
        }
        
        public void SetPoorPeopleHappiness(float value)
        {
            _poorPeopleHappiness = Mathf.Clamp(value, minHappinessClamp, maxHappinessClamp);
            onPoorHappinessUpdated?.Invoke(_poorPeopleHappiness);
            CheckHappinessStatus();
        }
        
        public void AddRichPeopleHappiness(float amount)
        {
            SetRichPeopleHappiness(_richPeopleHappiness + amount);
        }
        
        public void AddPoorPeopleHappiness(float amount)
        {
            SetPoorPeopleHappiness(_poorPeopleHappiness + amount);
        }

        private void CheckHappinessStatus()
        {
            if (_poorPeopleHappiness <= minHappinessClamp)
            {
                Debug.Log("Poor people are unhappy");
                TriggerGameOver();
            }
            else if (_richPeopleHappiness <= minHappinessClamp)
            {
                Debug.Log("Rich people are unhappy");
                TriggerGameOver();
            }
        }

        public DialogueWriter sentenceWriter;
        public DialogueWriter responseWriter;
        private DialogueBias _currentBiasCase = null;

        //jen pro vizual, problikava to tam pri startu
        public GameObject hiddenPanelFirstStart;

        public UnityEvent onJudgedReady;
        public UnityEvent onJudgedDismissed;
        public UnityEvent onJudgedBribery;
        public UnityEvent onGameOverTriggered;
        public UnityEvent<float> onRichHappinessUpdated;
        public UnityEvent<float> onPoorHappinessUpdated;
        public UnityEvent<int> onCoinsUpdated;
        public UnityEvent<JudgedType> newJudgedTypeReady;
        public UnityEvent onNewDayTimerStarted;
        
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
            onCoinsUpdated?.Invoke(_coins);
        }

        private void AdvanceDay()
        {
            DayManager.Instance.IncreaseDay();

            if (DayManager.Instance.Day > 1)
            {
                AddCoins(-50);
                if (_coins<0)
                {
                    TriggerGameOver();
                    return;
                }
            }
            
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
            newJudgedTypeReady?.Invoke(judgedType);
        }

        public void JudgeResponse(bool forConviction)
        {
            AddPoorPeopleHappiness(forConviction ? -_currentBiasCase.poor : _currentBiasCase.poor);
            AddRichPeopleHappiness(forConviction ? -_currentBiasCase.rich : _currentBiasCase.rich);
            Debug.Log($"Judged for {(forConviction ? "conviction" : "dismissal")}. Poor happiness: {_poorPeopleHappiness}, Rich happiness: {_richPeopleHappiness}");
            AfterJudging();

            if (forConviction)
            {
                AddCoins(3);
            }
            else
            {
                AddCoins(2);
            }
        }

        private void TriggerGameOver()
        {
            onGameOverTriggered?.Invoke();
        }
        
        public void BriberyBias()
        {
            if (_currentBiasCase.rich > _currentBiasCase.poor)
            {
                AddRichPeopleHappiness(0.1f);
                AddPoorPeopleHappiness(-0.2f);
            }
            else
            {
                AddRichPeopleHappiness(-0.2f);
                AddPoorPeopleHappiness(0.1f);
            }
        }

        private void ResetHappiness()
        {
            SetPoorPeopleHappiness(1);
            SetRichPeopleHappiness(1);
        }
        
        //Timed
        private IEnumerator FirstJudgedTimeout()
        {
            yield return new WaitForSeconds(daySwitchTime);
            ResetHappiness();
            StartCoroutine(InGameDayTimer());
            CreateNewJudged();
        }

        private IEnumerator InGameDayTimer()
        {
            onNewDayTimerStarted?.Invoke();
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
            
            // sance ze se pri vypovedi pokusi podplatit
            if (UnityEngine.Random.value < 1)
            {
                onJudgedBribery?.Invoke();
            }
        }
    }
}