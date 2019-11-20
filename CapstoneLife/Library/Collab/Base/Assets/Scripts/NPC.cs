using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using HJ.Dialogue;
public class NPC : MonoBehaviour
{
    [LabelText("NPC 번호")]
    public int _NPCNumber = 0;

    [LabelText("NPC 이름")]
    public string _NPCName;

    [LabelText("NPC Dialogue Box")]
    public GameObject _NPCBox;

    [LabelText("NPC 작동여부")]
    public bool is_Npc = true;

    [LabelText("기본 대화내용(순서)")]
    public Dialogue _Description = null;

    

    public Queue<string> QueueMssage{
        get{
            if(_Description == null) return null;
            Queue<string> temp = new Queue<string>();
            
            foreach(Message dec in _Description.setting)
            {
                temp.Enqueue(dec.description);
            }

            return temp;
        }
    }
    public Queue<Sprite> QueueImage{
        get{
            if(_Description == null) return null;
            Queue<Sprite> temp = new Queue<Sprite>();
            
            foreach(Message dec in _Description.setting)
            {
                temp.Enqueue(dec.description_Image);
            }

            return temp;
        }
    }

    private GameObject GetBox{
        get {
            if(_NPCBox == null) return null;
            return _NPCBox;
        }
    }


    public Text ChangeImage{
        get{
            Text temp = GetBox.GetComponentInParent<Text>();

            temp.text = "";

            return temp;
        }
    }

}
