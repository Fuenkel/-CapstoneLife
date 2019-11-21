using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using HJ.Dialogue;
using HJ.NPC;
using System;
using DG.Tweening;
namespace HJ.Manager
{
    
    public class DialogueManager : Singleton<DialogueManager>
    {
        #region Delegate
        public delegate void ClickDelegate(int n);                          //? 클릭이벤트
        public delegate void ChangeTextDelegate(Text t, string s);  //? 택스트 변경 이벤트
        #endregion

        #region EVENT
        private event ClickDelegate OnEvent_ClickButton;
        private event ChangeTextDelegate OnEvent_ChangeText;
        #endregion

        #region Variables
        //? Dialogue UI Variables
        public GameObject _DialogueBox;

        public Text _DialogueText;
        public GameObject[] _PlayerFace;

        //? Choice UI Variables
        public GameObject _ChoiceBox;

        public Text _ChoiceMainText;
        public Text _ChoiceText1;
        public Text _ChoiceText2;
        public Text _ChoiceText3;

        //? 알람
        public Text _Alerm;


        //? Component Script
        [HideInInspector]
        public ComponentDialogueShow _componentDialogue;


        public static bool is_Msg = false;     //! 대화중인가?
        public static bool is_keypad = true;  //! 방향키 이동 가능한가?
        public static bool is_nextmsg = false;  //! 다음 대화 내용 있나?
        private static bool is_choice = false;  //! 선택창이 떳는가?
        private static bool is_Wait = false;    //! 대기중 인가?
        public Queue<string> _msgDialogueList = new Queue<string>();
        private Queue<float> _msgtimer = new Queue<float>();
        public List<string> _choiceList = new List<string>();

        private int _count = 0;
        private string _m;
        private string _msgTitle;

        private Dialogue.Dialogue tempdb;
        private Dialogue.ChoiceDialogue tempchoicedb;

        #endregion

        /// <summary>
        /// 초기화
        /// </summary>
        private void Awake()
        {
            if (_DialogueText != null) _DialogueText.text = "";

            // 이벤트 등록
            OnEvent_ClickButton += OnClickNumber;
            OnEvent_ChangeText += ChangeText;
        }

        /// <summary>
        /// 다이얼로그 파일 불러오기
        /// </summary>
        public void LoadDialogue(Dialogue.Dialogue db)
        {
            _msgDialogueList.Clear();       //! 청소
            _msgtimer.Clear();
            _msgTitle = null;
            tempdb = null;
            _count = 0;
            var n =
                from set in db.setting
                where set.description != null
                select set.description;

            for (int m = 0; m < db.setting.Length; m++){
                _msgtimer.Enqueue(db.setting[m].descriptionTime);
            }

            foreach (var msg in n)
            {
                _msgDialogueList.Enqueue(msg);
            }
            _msgTitle = db.dialogueTitle;
            tempdb = db;
            Debug.Log(string.Format("Load Dialogue : {0}" ,_msgDialogueList.Count));
        }

        public void LoadChoiceDialogue(ChoiceDialogue db)
        {
            _choiceList.Clear();
            _msgTitle = null;
            tempchoicedb = null;
            List<string> s = db.ChoiceMsgList;
            if(s.Count >= 1)
            {
                _msgTitle = db.dialogueTitle;
                for (int i = 0; i < s.Count; i++)
                {
                    _choiceList.Add(s[i]);
                }
                tempchoicedb = db;
            }
        }

        public void NextButton()
        {
            if(is_Msg)
            {
                
                _DialogueText.DOKill();
                _DialogueText.text = "";
                _DialogueText.text = _m;
                is_Msg = false;
                if(is_nextmsg)
                {
                    StartCoroutine(ShowDialogue());
                }
            }
            else if(is_nextmsg)
            {
                StartCoroutine(ShowDialogue());
            }

            
        }

        /// <summary>
        /// 다이얼로그 보여주기
        /// </summary>
        public void SetDialogue()
        {
            StartCoroutine(ShowDialogue());
        }
        /// <summary>
        /// 선택창 보여주기
        /// </summary>
        public void SetChoiceDialogue()
        {
            StartCoroutine(ShowChoiceDialogue());
        }

        /// <summary>
        /// 다이얼로그 보여주기
        /// </summary>
        /// <returns>다이얼로그 에셋</returns>
        public IEnumerator ShowDialogue()
        {
           
            float t;
            is_Msg = true;
            is_nextmsg = true;

            if (_msgDialogueList.Count <= 0 || _msgtimer.Count <= 0)
            {
                //Debug.LogError(_msgDialogueList.Count +" " + _msgtimer.Count);

                //? 이벤트 실행
                if(tempdb.subEvnet.Length > 0)
                    StartCoroutine(PlayEvent(0));
                

                switch (tempdb.nextNODE._flow)
                {
                    case FlowNode.FLOW.DIALOGUE:
                        //? 다음 노드가 다이얼로그 인 경우
                        if(tempdb.nextNODE._NextNode != null)
                        {
                            LoadDialogue(tempdb.nextNODE._NextNode);

                            StartCoroutine(ShowDialogue());
                            yield break;
                        }
                        
                        
                        break;
                    case FlowNode.FLOW.CHOICEDIALOGUE:
                        //? 다음 노드가 선택로그 인 경우
                        if (tempdb.nextNODE._ChoiceNode != null)
                        {
                            LoadChoiceDialogue(tempdb.nextNODE._ChoiceNode);

                            StartCoroutine(ShowChoiceDialogue());
                            yield break;
                        }

                        break;
                }
                /*
                if(tempdb.subEvnet.GetPersistentEventCount() > 0)
                    tempdb.subEvnet.Invoke();
                    */

               

                is_keypad = true;
                is_Msg = false;
                is_nextmsg = false;
                HideDialogueObject();     //? 오브젝트 비활성화

               

                yield break;
                
            }

            ShowDialogueObject();   //? 오브젝트 표시
            t = _msgtimer.Dequeue();
            _m = _msgDialogueList.Dequeue();
            _DialogueText.text = "";
            _DialogueText.DOText(_m,t);
            yield return new WaitForSeconds(t + 0.7f);
            _m = null;
            is_Msg = false;
        }
        
        /// <summary>
        /// 선택창 보여주기
        /// </summary>
        /// <returns>선택창 에셋</returns>
        public IEnumerator ShowChoiceDialogue()
        {
            int cnt = _choiceList.Count;
            
            is_choice = false;
            is_Msg = true;


            //? 선택창 뛰우기
            ShowChoiceDialogueObject();

            //? 이벤트 실행 : 텍스트 변경
            OnEvent_ChangeText(_ChoiceMainText, tempchoicedb.dialogueTitle);
            OnEvent_ChangeText(_ChoiceText1, _choiceList[0]);
            OnEvent_ChangeText(_ChoiceText2, _choiceList[1]);
            OnEvent_ChangeText(_ChoiceText3, _choiceList[2]);

            yield return new WaitForSeconds(2f);
            is_choice = true;

            

            
            yield return null;
        }

        #region 다이얼로그창
        void ShowDialogueObject()
        {
            _DialogueBox.SetActive(true);
            _DialogueBox.transform.DOShakeScale(0.2f, 0.2f, 8);
            _DialogueText.text = "";
        }

        void HideDialogueObject()
        {
            _DialogueBox.transform.DOShakeScale(1f, 0.5f, 8);
            _DialogueText.text = "";
            _DialogueBox.SetActive(false);
        }

        void ShowChoiceDialogueObject()
        {
            _ChoiceBox.transform.DOShakeScale(0.2f, 0.2f, 8);
            _ChoiceBox.SetActive(true);
            _ChoiceMainText.text = "";
            _ChoiceText1.text = "";
            _ChoiceText2.text = "";
            _ChoiceText3.text = "";
        }

        void HideChoiceDialogueObject()
        {
            _ChoiceBox.transform.DOShakeScale(0.2f, 0.2f, 8);
            _ChoiceBox.SetActive(false);
            _ChoiceMainText.text = "";
            _ChoiceText1.text = "";
            _ChoiceText2.text = "";
            _ChoiceText3.text = "";
        }
        void ChangeText(Text t,string s)
        {
            t.DOText(s, 1.6f);
        }
        #endregion

        //? Choice 선택
        public void Choice1()
        {
            if (is_choice)
            {
                OnEvent_ClickButton(1);
                
                HideChoiceDialogueObject();
            }
            else
            {
                Debug.Log("Button SEtting Waitting....");
            }
        }
        public void Choice2()
        {
            if (is_choice)
            {
                OnEvent_ClickButton(2);
                   
                HideChoiceDialogueObject();
            }
            else
            {
                Debug.Log("Button SEtting Waitting....");
            }
        }
        public void Choice3()
        {
            if (is_choice)
            {
                OnEvent_ClickButton(3);
                   
                HideChoiceDialogueObject();
            }
            else
            {
                Debug.Log("Button SEtting Waitting....");
            }
        }

        void OnClickNumber(int n)
        {
            FlowNode node = null;
            if (n == 1) node = tempchoicedb._NextChoice1Node;
            else if (n == 2) node = tempchoicedb._NextChoice2Node;
            else if (n == 3) node = tempchoicedb._NextChoice3Node;
            else
            {
                Debug.LogError("OnClickNumber Method Node null");
                return;
            }

            //? 이벤트 실행
            switch (n)
            {
                case 1:
                    //? 이벤트 실행
                    if (tempchoicedb.subevent1.Length > 0)
                        StartCoroutine(PlayEvent(1));
                    break;
                case 2:
                    //? 이벤트 실행
                    if (tempchoicedb.subevent2.Length > 0)
                        StartCoroutine(PlayEvent(2));
                    break;
                case 3:
                    //? 이벤트 실행
                    if (tempchoicedb.subevent3.Length > 0)
                        StartCoroutine(PlayEvent(3));
                    break;
            }


            switch (node._flow)
            {
                case FlowNode.FLOW.DIALOGUE:
                    LoadDialogue(node._NextNode);
                    tempchoicedb = null;
                    StartCoroutine(ShowDialogue());
                    break;
                case FlowNode.FLOW.CHOICEDIALOGUE:
                    LoadChoiceDialogue(node._ChoiceNode);
                    StartCoroutine(ShowChoiceDialogue());
                    break;
            }
                    
             

            is_Msg = false;
            is_choice = false;
        }

        //? 이벤트 시작 코루틴
        IEnumerator PlayEvent(int n)
        {
            switch(n)
            {
                case 0:     // Dialogue
                    for (int i = 0; i < tempdb.subEvnet.Length; i++)
                    {
                        while (is_Wait)
                        {
                            yield return null;
                        }
                        EventPageManager.Instance.OnPlay(tempdb.subEvnet[i]);
                    }
                    break;
                case 1:     // Choice 1
                    for (int i = 0; i < tempchoicedb.subevent1.Length; i++)
                    {
                        while(is_Wait)
                        {
                            yield return null;
                        }
                        EventPageManager.Instance.OnPlay(tempchoicedb.subevent1[i]);
                    }
                    break;
                case 2:     // Choice2
                    for (int i = 0; i < tempchoicedb.subevent2.Length; i++)
                    {
                        while (is_Wait)
                        {
                            yield return null;
                        }
                        EventPageManager.Instance.OnPlay(tempchoicedb.subevent2[i]);
                    }
                    break;
                case 3:     // Choice 3
                    for (int i = 0; i < tempchoicedb.subevent3.Length; i++)
                    {
                        while (is_Wait)
                        {
                            yield return null;
                        }
                        EventPageManager.Instance.OnPlay(tempchoicedb.subevent3[i]);
                    }
                    break;
                default:    // Null

                    break;
            }
        }

        /// <summary>
        /// 대기 시간
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public IEnumerator WaitMethod(float t =0.1f)
        {
            is_Wait = true;
            WaitForSeconds wait = new WaitForSeconds(t);
            yield return wait;
            is_Wait = false;
            wait = null;
        }

        /// <summary>
        /// 알람 표시
        /// </summary>
        /// <param name="msg">메세지 내용</param>
        /// <returns></returns>
        public IEnumerator AlermShow(string msg)
        {
            WaitForSeconds wait = new WaitForSeconds(2f);

            while (is_Wait)
            {
                yield return null;
            }

            _Alerm.DOText(msg, 1.2f);

            yield return wait;

            _Alerm.text = "";

            wait = null;
        }
    }
}