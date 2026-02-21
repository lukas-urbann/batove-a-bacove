using System;
using System.Collections;
using Dialogues;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

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
        [SerializeField] private float workDayTime = 60;
        [SerializeField] private float responseTime = 2.5f;
        [SerializeField] private float noJudgedTimeoff = 2f;

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
        public DialogueWriter responseWriter;
        private DialogueBias currentBiasCase = null;

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

        private IEnumerator NewDayTimeoff()
        {
            RemoveJudged();
            yield return new WaitForSeconds(daySwitchTime);
            CreateNewJudged();
        }
        
        //jen jednou po vstupu do game sceny
        private void Start()
        {
            AdvanceDay();
        }

        private void AdvanceDay()
        {
            DayManager.Instance.IncreaseDay();
            StartCoroutine(NewDayTimeoff());
        }

        private void Update()
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                AdvanceDay();
            }
            
            if (Keyboard.current.hKey.isPressed)
            {
                JudgeResponse(true);
                onJudgedDismissed?.Invoke();
                StartCoroutine(NoJudgedTimer());
            }
            
            if (Keyboard.current.kKey.isPressed)
            {
                JudgeResponse(false);
                onJudgedDismissed?.Invoke();
                StartCoroutine(NoJudgedTimer());
            }
        }
        
        private IEnumerator NoJudgedTimer()
        {
            yield return new WaitForSeconds(noJudgedTimeoff);
            CreateNewJudged();
        }

        public void CreateNewJudged()
        {
            //random judged type
            var judgedType = (JudgedType)UnityEngine.Random.Range(0, Enum.GetValues(typeof(JudgedType)).Length);
            var d = DialogueManager.Instance.CreateNewDialogue(judgedType);
            onJudgedReady?.Invoke();
            sentenceWriter.WriteSentence(d.Excerpt.Item1);
            StartCoroutine(ResponseWriteDelay(d.Response.text));
            currentBiasCase = d.Bias;
        }

        private IEnumerator ResponseWriteDelay(string response)
        {
            yield return new WaitForSeconds(responseTime);
            responseWriter.WriteSentence(response);
        }
        
        public void RemoveJudged()
        {
            onJudgedDismissed?.Invoke();
        }
        
        public void JudgeResponse(bool forConviction)
        {
            PoorPeopleHappiness += forConviction ? -currentBiasCase.poor : currentBiasCase.poor;
            RichPeopleHappiness += forConviction ? -currentBiasCase.rich : currentBiasCase.rich;
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