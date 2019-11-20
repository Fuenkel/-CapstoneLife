using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using HJ.Dialogue;
using UniRx;
using DG.Tweening;
using Sirenix.OdinInspector;
using System.Linq;
public class DialogueManager : MonoBehaviour
{
    #region 싱글턴
    private static DialogueManager instance;
    public static DialogueManager Instance{
        get{return instance;}
        set{instance = value;}
    }
    
    void Awake(){
        if(instance == null) instance = this;
        else if(instance != this) Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
    #endregion

    #region 유틸리티
    private string tap1 = "GameObject";
    private string tap2 = "TEXT & IMAGE";
    #endregion

    #region 변수선언
    [TabGroup("$tap1")]
    public GameObject _MessagePanel;
    
    [TabGroup("$tap1")]
    public GameObject[] _PlayerBox;

    [TabGroup("$tap1")]
    public GameObject _BackGround;


    [TabGroup("$tap2")]
    public Text _MessageText;
    
    [SerializeField]
    private Image[] _PlayerImage;
    #endregion

    private Queue<string> _MsgQueue;
    private Queue<Sprite> _ImageQueue;


    void Start(){
        InitGame();
    }

    void InitGame(){
        //? Null 값이 아닌 Box 찾기
        var _ImageTemp = 
            from  box in _PlayerBox
            where box != null
            select box;

        var subject = new Subject<int>();
        
        //? 이미지 생성
        _PlayerImage = new Image[_ImageTemp.Count()];
        

        //? 비동기적 서브젝트 작동
        subject.Subscribe(x => {
            _PlayerImage[x] = _ImageTemp.ElementAt(x).GetComponentInChildren<Image>();
        },
        _ => Debug.LogError("Subject ERROR"));
        
        
        for(int i = 0; i < _ImageTemp.Count(); i++)
        {
            subject.OnNext(i);
        }
        
        subject.OnCompleted();

        Debug.Log(_ImageTemp.Count());

        StartCoroutine(HideObject());
    }

    public void LoadDialogue(Dialogue db){
        if(db == null) return;
        _MsgQueue = new Queue<string>();
        _ImageQueue = new Queue<Sprite>();

        _MsgQueue.Clear();
        _ImageQueue.Clear();
        
        //? Null 값이 아닌 Dialogue안의  Setting 클래스 찾기
        var msg = 
        from n in db.setting
        where n != null
        select n;

        foreach(var temp in msg){
            _MsgQueue.Enqueue(temp.description);
            _ImageQueue.Enqueue(temp.description_Image);
        }

        Debug.Log(_MsgQueue.Count);

        
    }

    IEnumerator HideObject(){
        
        WaitForSeconds timer = new WaitForSeconds(1f);

        _BackGround.GetComponent<Image>().DOFade(0f,0.5f);
        _MessagePanel.GetComponent<Image>().DOFade(0f,0.5f);
        _MessageText.text ="";
        yield return timer;

        _BackGround.SetActive(false);
        _MessagePanel.SetActive(false);
        
        for(int i = 0 ; i < _PlayerBox.Length; i++)
            if(_PlayerBox[i] != null)
                _PlayerBox[i].SetActive(false);
    }

    IEnumerator ShowObject(){
        WaitForSeconds timer = new WaitForSeconds(1f);

        _BackGround.SetActive(true);
        _MessagePanel.SetActive(true);
        _BackGround.GetComponent<Image>().DOFade(1f,0.5f);
        _MessagePanel.GetComponent<Image>().DOFade(1f,0.5f);
        yield return timer;

        
    }

    void ShowText(string _msg ="", Sprite _img = null, int _petton = 0)
    {
        
    }
    
}
