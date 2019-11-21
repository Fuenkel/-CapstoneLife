using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace HJ.NPC
{
    /// <summary>
    /// 이 스크립트는 NPC의 조건문을 설정하는 시스템입니다.
    /// </summary>
    public class ComponentIF : MonoBehaviour
    {
        [LabelText("NPCManager IF 번호"),Space(10)]
        public int _IFNode = -1;

        //? 설명
        [LabelText("설명")]
        public string descrption;

        [LabelText("NPC 활성화 하시겠습니까 ?")]
        public bool is_start;

        [LabelText("EVENT")]
        public HJ.Manager.MessageCommend[] eventmessage;
        void Update()
        {
            if (!is_start || _IFNode <= -1) return;
            if (NPCManager.Instance.npcifList.Length <= 0 || NPCManager.Instance.npcifList.Length <= _IFNode) return;

            if(NPCManager.Instance.npcifList[_IFNode].is_bool)
            {
                is_start = false;

                EVENTPLAY();
            }
        }

        public void EVENTPLAY()
        {
            if (eventmessage.Length > 0)
                foreach (var n in eventmessage) 
                    HJ.Manager.EventPageManager.Instance.OnPlay(n);
        }
    }

}
