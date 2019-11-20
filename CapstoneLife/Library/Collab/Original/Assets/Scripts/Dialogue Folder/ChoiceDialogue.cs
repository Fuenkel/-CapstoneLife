﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


namespace HJ.Dialogue
{
    [CreateAssetMenu(fileName = "New ChoiceDialogue", menuName = "Dialogue/New ChoiceDialogue")]
    [TypeInfoBox("ChoiceDialogue File")]
    public class ChoiceDialogue : ScriptableObject
    {
        [InfoBox("다음 대화로 이어갈 때 NodeID를 이용하여 이동한다. \n -1은 ID 설정 사용을 안하겠다는 뜻이다.")]
        [Title("Scriptable 고유번호")]
        
        public int NodeID = -1;

        [LabelText("파일 제목")]
        public string dialogueTitle;

        [Title("선택메세지 설정"),InfoBox("선택 메세지 3개 입력칸.")]
        [LabelText("선택 메세지1"),BoxGroup("Setting")]
        public string _ChoiceMsg1;
        [LabelText("이동할 DB1"),BoxGroup("Setting")]
        public Dialogue _NextChoice1Node;
        [LabelText("선택 메세지2"),BoxGroup("Setting")]
        public string _ChoiceMsg2;
        [LabelText("이동할 DB2"),BoxGroup("Setting")]
        public Dialogue _NextChoice2Node;

        [LabelText("선택 메세지3"),BoxGroup("Setting")]
        public string _ChoiceMsg3;

        [LabelText("이동할 DB3"),BoxGroup("Setting")]
        public Dialogue _NextChoice3Node;


        public List<string> ChoiceMsgList{
            get{
                List<string> temp = new List<string>();
                temp.Clear();
                temp.Add(_ChoiceMsg1);
                temp.Add(_ChoiceMsg2);
                temp.Add(_ChoiceMsg3);


                return temp;
            }
        }

        public List<Dialogue> DialogueList{
            get{
                List<Dialogue> temp = new List<Dialogue>();
                temp.Clear();
                temp.Add(_NextChoice1Node);
                temp.Add(_NextChoice2Node);
                temp.Add(_NextChoice3Node);

                return temp;
            }
        }

        
    }
}

