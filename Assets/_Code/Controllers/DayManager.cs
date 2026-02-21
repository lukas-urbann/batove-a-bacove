using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Controllers
{
    public class DayManager : MonoBehaviour
    {
        public static DayManager Instance { get; private set; }

        [SerializeField] private int leastPeopleToJudge = 4;
        [SerializeField] private int mostPeopleToJudge = 25;
        
        public Action OnNewDayEnded;
        public Action<int> OnNewDayStarted;
        
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

        private int _day = 0;
        private int _toJudge = 0;

        public void IncreaseDay()
        {
            _day++;
            OnNewDayStarted?.Invoke(_day);
            _toJudge = Mathf.Clamp(_day + Random.Range(0, 4), leastPeopleToJudge, mostPeopleToJudge);
        }
    }
}