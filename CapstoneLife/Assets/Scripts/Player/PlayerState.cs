using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
//? 플레이어의 스텟 설정
public class PlayerState : Singleton<PlayerState>
{
    #region 변수
    public string character_Name = "한돌이";
    private uint study_stat;     //? 공부력 
    private uint social_stat;    //? 인싸력
    private uint love_stat;      //? 사량
    private uint money;              //? 돈


    private uint current_MapID;
    private uint current_hp;             //? 현재건강
    private uint max_hp;                  //? 최대 건강력
    
    public Dictionary<string,uint> status_arrayList = new Dictionary<string,uint>();
    public Dictionary<string, Text> status_TextList = new Dictionary<string, Text>();

    [Title("UI Setting")]
    public GameObject ui_statusBox;         //? 스테이터스 부모 창
    public Text[] ui_stateText = new Text[4];                 //? 스테이스 능력 텍스트 상자


    //? UI 창
    public bool is_true = false;

    public enum STATUSENUM
    {
        NONE = 0,Study =1, Social, Love, Money, MapID, MaxHP, CurrHP
    }

    public STATUSENUM statusenum;

    public static bool is_tutorial;         //? 튜토리얼을 끝냈는가?
    #endregion


    /// <summary>
    /// 눙력치
    /// </summary>
    /// <param name="t">요소</param>
    /// <param name="n">값</param>
    public void SettingStatus(STATUSENUM t, uint n)
    {
        switch(t)
        {
            case 0:
                break;
            case STATUSENUM.Study:
                Study = n;
                break;
            case STATUSENUM.Social:
                Social = n;
                break;
            case STATUSENUM.Love:
                Love = n;
                break;
            case STATUSENUM.Money:
                AddMoney = n;
                break;
            case STATUSENUM.CurrHP:
                CurrentHpADD = n;
                break;

        }
    }


    #region 프로퍼티
    public uint Study
    {
        get { return study_stat; }
        set { study_stat += value; }
    }
    public uint Social
    {
        get { return social_stat; }
        set { social_stat += value; }
    }
    public uint Love
    {
        get { return love_stat; }
        set { love_stat += value;}
    }
    /// <summary>
    /// 돈 증가
    /// </summary>
    public uint AddMoney
    {
        get { return money; }
        set { money += value; }
    }

    /// <summary>
    /// 돈 감소
    /// </summary>
    public uint SubMoney
    {
        get { return money; }
        set { money -= value; }
    }

    public uint MapID
    {
        get { return current_MapID; }
        set { current_MapID = value; }
    }
    public uint MaxHP
    {
        get { return max_hp; }
        set { max_hp = value; }
    }
    public uint CurrentHpADD
    {
        get { return current_hp; }
        set
        {
            uint n = current_hp + value;
            if( n > MaxHP)  //? 최대체력이 높으면?
            {
                current_hp = MaxHP; //? 최대 채력만큼 회복
            }
            else
            {
                current_hp = n; //? 아니면 증가
            }
        }
    }
    public uint CurrentHPSub
    {
        get { return current_hp; }
        set
        {
            int n = (int)current_hp - (int)value;

            if(n > 0)
            {
                current_hp = (uint)n;
            }
            else
            {
                current_hp = 0;
                GameManager.isDead = true;      //? 죽음 확정
            }

        }
    }
    #endregion


    private void Awake()
    {
       
    }

    private void Update()
    {
        if (is_true)
        {
            ui_stateText[0].text = string.Format("{0}", Study);
            ui_stateText[1].text = string.Format("{0}", Social);
            ui_stateText[2].text = string.Format("{0}", Love);
            ui_stateText[3].text = string.Format("{0}", money);
        }
        
    }

    public void MenuButtonClick()
    {
        if (HJ.Manager.DialogueManager.is_Msg) return;
        is_true = !is_true;
        if (is_true) ui_statusBox.SetActive(true);
        else ui_statusBox.SetActive(false);
    }

    void InitGame()
    {
        status_arrayList.Clear();

        status_arrayList.Add("Study", Study);
        status_arrayList.Add("Social", Social);
        status_arrayList.Add("Love", Love);
        status_arrayList.Add("Money", money);
        status_arrayList.Add("MapID", current_MapID);
        status_arrayList.Add("MaxHP", max_hp);
        status_arrayList.Add("CurrHP", current_hp);


        statusenum = STATUSENUM.NONE;
    }
    /// <summary>
    /// 스테이터스 능력치 추가
    /// </summary>
    /// <param name="status"></param>
    /// <param name="n"></param>
    public void ADDStatusList(STATUSENUM status, uint n)
    {
        status_arrayList[status.ToString()] += n;
    }

    

}
