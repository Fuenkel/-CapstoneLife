using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using HJ.Dialogue;
using HJ.Manager;
using HJ.NPC;
using DG.Tweening;
using System.Linq;
using Sirenix.OdinInspector;


namespace HJ.Manager
{
    
    public class EventPageManager : Singleton<EventPageManager>
    {
        //? 이벤트 목록 열거형
        [InfoBox("FadeOut - 페이드아웃, FadeIn - 페이드 인, Fade - 페이드 아웃/인\n" +
            "PlayerOn 플레이어 표시, PlayerOff 플레이어 비표시\n" +
            "Add#### 능력치 증가, Sub### 능력치 감소\n" +
            "Wait 잠깐대기, Alert 알람 표시, SetWeather 날씨 변경, RandomAddMoney 랜덤값 돈 증가\n" +
            "AddTimer 시간 증가")]
        [DisplayAsString, LabelText("Update List")]
        public string _display;
        public enum EventMessageEnum
        {
            FadeOUT, FadeIn, Fade, PlayerOn, PlayerOff, Addstudy, Addsocial, AddLove, Addmoney, Submoney, AddHp, SubHp, Wait, Alert, SetWeather,
            RandomAddMoney, AddTimer, MAPWARP
        }


        /// <summary>
        /// 이벤트 시작
        /// </summary>
        public Action<MessageCommend> OnPlay ;

        private void Start()
        {
            OnPlay = Message;

            
        }

        /// <summary>
        /// 이벤트를 출력합니다.
        /// </summary>
        /// <param name="msg">사용방법은 : 영어:숫자  숫자는 시간초 혹은 값을 뜻합니다.</param>
        public void Message(MessageCommend commend)
        {
            EventMessageEnum init = commend.msgenum;
            float time = commend.timer;
            string msg = commend.msg;

            //! 사용방법 : EventPageManager.Instance.OnPlay("문자열",값);           
            switch (init)
            {
                case EventMessageEnum.FadeOUT: //? 페이드 아웃 이벤트
                    StartCoroutine(FadeOut(time));
                    break;
                case EventMessageEnum.FadeIn:  //? 페이드 인 이벤트
                    StartCoroutine(FadeIn(time));
                    break;
                case EventMessageEnum.Fade : //? 페이드 아웃, 인 이벤트
                    GameManager.Instance.Fade(time);
                    break;
                case EventMessageEnum.PlayerOn :    //? 플레이어 보이기
                    GameManager.Instance._Player.gameObject.SetActive(true);
                    GameManager.Instance._MainCamera.gameObject.GetComponent<CameraManager>().enabled = true;
                    break;
                case EventMessageEnum.PlayerOff : //? 플레이어 숨기기
                    GameManager.Instance._MainCamera.gameObject.GetComponent<CameraManager>().enabled = false;
                    GameManager.Instance._Player.gameObject.SetActive(false);
                    break;
                case EventMessageEnum.Addstudy: //? 지식 증가
                    PlayerState.Instance.Study = (uint) Mathf.Round(time);
                    break;
                case EventMessageEnum.Addsocial: //? 인싸력 증가
                    PlayerState.Instance.Social = (uint)Mathf.Round(time);
                    break;
                case EventMessageEnum.AddLove: //? 하트 증가
                    PlayerState.Instance.Love = (uint)Mathf.Round(time);
                    break;
                case EventMessageEnum.Addmoney: //? 돈 증가
                    PlayerState.Instance.AddMoney = (uint)Mathf.Round(time);
                    break;
                case EventMessageEnum.Submoney: //? 돈 감소
                    PlayerState.Instance.SubMoney = (uint)Mathf.Round(time);
                    break;
                case EventMessageEnum.AddHp: //? 건강 증가
                    PlayerState.Instance.CurrentHpADD = (uint)Mathf.Round(time);
                    break;
                case EventMessageEnum.SubHp: //? 건강 감소
                    PlayerState.Instance.CurrentHPSub = (uint)Mathf.Round(time);
                    break;
                case EventMessageEnum.Wait: //? 잠깐 대기
                    StartCoroutine( DialogueManager.Instance.WaitMethod(time));
                    break;
                case EventMessageEnum.Alert: //? 알람
                    StartCoroutine(DialogueManager.Instance.AlermShow(msg));
                    break;
                case EventMessageEnum.SetWeather: // 날씨
                    TimeManager.Instance.WEATHERSET = commend.weather;
                    break;
                case EventMessageEnum.RandomAddMoney:   //? 랜덤 돈
                    uint rand = (uint)UnityEngine.Random.Range(commend.minMoney, commend.maxMoney);
                    PlayerState.Instance.AddMoney = rand;
                    break;
                case EventMessageEnum.AddTimer:   //? 랜덤 돈
                    TimeManager.Instance.AddTime((int)time);
                    break;
                case EventMessageEnum.MAPWARP:  //? 맵이동
                    if (commend.mapwarp == null) commend.mapwarp.position = new Vector3(0, 0, 0);

                    StartCoroutine(MapWarp(commend.mapwarp.position));
                    
                    break;
                default :
                    Debug.LogError("등록되지 않은 이벤트이다.");
                    break;
            }

            time = 0;
            msg = null;
        }

        #region EVENT PAGE
        //? 페이드 아웃
        public IEnumerator FadeOut(float t)
        {
            GameManager.Instance._FadeOutImage.DOFade(1f, t);
            DialogueManager.is_keypad = false;
            yield return new WaitForSeconds(t + 0.4f);
        }
        //? 페이드 인
        public IEnumerator FadeIn(float t)
        {
            GameManager.Instance._FadeOutImage.DOFade(0f, t);
            DialogueManager.is_keypad = true;
            yield return new WaitForSeconds(t + 0.4f);
        }
        //? 맵 워프
        public IEnumerator MapWarp(Vector3 pos)
        {
            WaitForSeconds wait = new WaitForSeconds(1.25f);
            GameManager.Instance.Fade(0.8f);
            yield return wait;
            GameManager.Instance._Player.transform.position = pos;
            GameManager.Instance._MainCamera.transform.position = new Vector2(pos.x, pos.y);
        }
        #endregion


    }


    /// <summary>
    /// 메세지 명령어창
    /// </summary>
    [Serializable]
    public class MessageCommend
    {
        [Title("이벤트 실행"),LabelText("이벤트 목록"),EnumPaging]   
        public HJ.Manager.EventPageManager.EventMessageEnum msgenum;

        [LabelText("Value")]
        public float timer;

        [LabelText("알람메세지 Alert 에서만 사용가능")]
        public string msg;

        [LabelText("날씨 전용 변경")]
        public WEATHERENUM weather;

        [Title("RandomMoney 전용")]
        [LabelText("MinMoney")]
        public uint minMoney;
        [LabelText("MaxMoney")]
        public uint maxMoney;

        [LabelText("MAPWARP 전용")]
        public Transform mapwarp;

        
    }


}

