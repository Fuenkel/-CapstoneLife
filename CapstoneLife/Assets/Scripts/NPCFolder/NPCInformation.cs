using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using HJ.Dialogue;
namespace HJ.NPC
{
    public class NPCInformation : MonoBehaviour
    {
        [LabelText("NPC 이름")]
        public string _name;

        [LabelText("NPC 설명"), Multiline]
        public string _dec;

        [LabelText("NPC 식별번호")]
        public int _index;

        [Title("사용하고 싶은 컴퍼넌트"), LabelText("대화내용관련")]
        public ComponentDialogueShow componentDialogueShow;
        [LabelText("이동 관련")]
        public Component_NPCMOVE componentMove;
        
        

    }


}
