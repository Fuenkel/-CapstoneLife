using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
namespace HJ.Manager
{
    public enum TIMEENUM
    {
        MORNING,          //? 아침
        LUNCH,              //? 점심
        NIGHT,              //? 저녘
        DAWN                //? 새벽
    }
    public interface ITime
    {
        int Day { get; set; }
        int Time { get; set; }
        TIMEENUM TIMESET { get; set; }
        void SetDay(int n);
        
    }
    public class TimeManager : MonoBehaviour, ITime
    {
        // 요일
        private int _day;

        // 열거형 타입의 오늘의 시간
        [SerializeField, EnumPaging]
        private TIMEENUM _timenum;

        // 시간초
        private int _time;

        // 인터페이스
        public ITime Itime;

        #region 싱글턴
        // 인스턴스
        private static TimeManager instance;

        public static TimeManager Instance
        {
            get { return instance; }
            set { instance = value; }
        }
        void Awake()
        {
            if (instance == null) instance = this;
            else if (instance != this) Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
        }
        #endregion

        #region 인터페이스 구현
        int ITime.Day { get => _day; set => _day = value; }
        public TIMEENUM TIMESET { get => _timenum; set =>_timenum = value; }
        public int Time { get => _time; set => _time = value; }

        /// <summary>
        /// 시간에 따른 하늘 상태 변경
        /// </summary>
        /// <param name="n"></param>
        public void SetDay(int n)
        {
             if(n >= 0 && n < 6)
            {
                TIMESET = TIMEENUM.MORNING;
            }else if(n >= 6 && n < 12)
            {
                TIMESET = TIMEENUM.LUNCH;
            }else if(n >= 12 && n < 18)
            {
                TIMESET = TIMEENUM.NIGHT;
            }else if(n >= 18 && n < 24)
            {
                TIMESET = TIMEENUM.DAWN;
            }

             
        }
        #endregion


        #region 메서드
        /// <summary>
        /// 다음날 시작
        /// </summary>
        private void NextDay()
        {
            Itime.Time = 0;
            ++Itime.Day;
        }

        /// <summary>
        /// 시간 확인
        /// </summary>
        public void CheckDay()
        {
            // 24 숫자가 되면 하루 지남
            if(Itime.Time >= 24)
            {
                NextDay();
            }

            // 상태변경
            SetDay(Itime.Time);
        }

        #endregion
    }
}

