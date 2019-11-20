using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class GameManager : Singleton<GameManager>
{
    public Transform _Player;                   //? 플레이어
    public Transform _MainCamera;           //? 메인카메라

    public Image _FadeOutImage;        //? FadeOut
   
    

    //? 캐릭터가 죽었는가?
    public static bool isDead = false;
  

    /// <summary>
    /// 페이드 아웃
    /// </summary>
    /// <param name="t">시간</param>
    public void Fade(float t)
    {
        StartCoroutine(FadeOutCorutine(t));
    }





    private IEnumerator FadeOutCorutine(float t)
    {
        if(_FadeOutImage == null) { yield break; }
        _FadeOutImage.DOFade(1f, t);

        yield return new WaitForSeconds(t);

        yield return StartCoroutine(FadeInCorutine(t));
    }
    private IEnumerator FadeInCorutine(float t)
    {
        if (_FadeOutImage == null) { yield break; }
        _FadeOutImage.DOFade(0f, t);

        yield return null;
    }
}
