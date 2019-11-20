using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public delegate void delegate_OnEvent();
public delegate void delegate_OnStart();

public class NPCEvent : MonoBehaviour
{
    #region 변수
    public GameObject _QuestInfo;   //? 퀘스트 내용

    public event delegate_OnEvent OnEvent;  //? 이벤트 발생시 시작
    public event delegate_OnStart OnStart;  //? 처음 시작
    
    public void Start()
    {
        //EVENT_PLAY += _Event;
    }
    #endregion
}
