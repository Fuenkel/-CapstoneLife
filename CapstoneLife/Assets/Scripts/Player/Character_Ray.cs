using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HJ.NPC;

public class Character_Ray : MonoBehaviour
{
    public GameObject btn_select;
    

    private Vector2 locate;

    private Collider2D collider2d;

    //  Vector2 _PlayerVector;
    //  public int walkCount = 2;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {  /*
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        // RaycastHit2D hit;
        locate.Set(horizontal, vertical);

        Vector2 _start = transform.position;
        Vector2 _end = _start + new Vector2(locate.x * 0.5f, locate.y * 0.5f);
        
        RaycastHit2D hit = Physics2D.Raycast(_start, _end, 10.0f);
        Debug.DrawRay(_start, _end, Color.red, 0.3f);
        if (hit)
        {
         //   Debug.Log("good");
       }            
       */
    
}

     void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "NPC" )
        {
            btn_select.SetActive(true);
            collider2d = collision;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "NPC")
        {
            btn_select.SetActive(false);
            collider2d = null;
        }
    }

    /// <summary>
    /// 상호작용 버튼
    /// </summary>
    public void Speechup()
    {
        if (HJ.Manager.DialogueManager.is_nextmsg && HJ.Manager.DialogueManager.is_Msg) return;

        NPCInformation info = collider2d.GetComponent<NPCInformation>();
        if(info.componentDialogueShow != null)
        {
            switch(info.componentDialogueShow._dialogueenum)
            {
                case Utility_Dialogue.DialoguePettonEnum.NORMALMSG: //? 말풍선 대화
                    info.componentDialogueShow.StartNormalMsg();
                    break;
                case Utility_Dialogue.DialoguePettonEnum.STORYMSG:  //? 일반 대화
                    if(info.componentDialogueShow._msgDialogue != null && !HJ.Manager.DialogueManager.is_nextmsg)
                        info.componentDialogueShow.StartDialogueMsg(info.componentDialogueShow._msgDialogue);
                    break;
                case Utility_Dialogue.DialoguePettonEnum.CHOICEMSG: //? 선택 대화
                    if (info.componentDialogueShow._msgChoice != null && !HJ.Manager.DialogueManager.is_nextmsg)
                        info.componentDialogueShow.StartChoiceDialogueMsg(info.componentDialogueShow._msgChoice);
                    break;
            }
            
        }

        
        Debug.Log(info._name);
    }

}