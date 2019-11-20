using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using HJ.Manager;
//? 02.MainMap Scene에서 사용하는 스크립트
public class GameLoding : MonoBehaviour
{
    public Slider LodingSlider;
    public Text textbox;

    public MapManager manager;

    public GameObject tutorial;

    //? 씬들 로딩
    string[] sceneName =
    {
        "03.SubMap_School", "04.SubMap_Home", "05.SubMap_PCRoom", "06.SubMap_Coffee 1", "07.SubMap_Campus",
        "08.SubMap_Book", "09.SubMap_bar"
    };

    private void Start()
    {
        LodingSlider.value = 0;
        InitGame();
    }


    void InitGame()
    {
        StartCoroutine(Loading());


       
    }

    IEnumerator Loading()
    {
        AsyncOperation async;
        for(int i = 0; i < sceneName.Length; i++)
        {
            async = SceneManager.LoadSceneAsync(sceneName[i], LoadSceneMode.Additive);

            while(!async.isDone)
            {
                yield return new WaitForEndOfFrame();
                LodingSlider.value = async.progress;
            }
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName[i]));
            textbox.text += string.Format("{0} Scene 로딩 완료\n",SceneManager.GetActiveScene().name);
        }
        yield return new WaitForSeconds(2f);
        GameObject[] obj = GameObject.FindGameObjectsWithTag("MapID");

        for (int i = 0; i < obj.Length; i++)
            manager.mapBoundBoxCollider[i] = obj[i].GetComponent<BoxCollider2D>();

        LodingSlider.gameObject.SetActive(false);

        // Main Scene 설정
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("02.MainMap"));

        if (!PlayerState.is_tutorial)
        {
            tutorial.gameObject.GetComponent<HJ.NPC.Component_NPCEVENT>().PlayEvent();
        }
        

        Destroy(gameObject, 10f);
        
    }
}
