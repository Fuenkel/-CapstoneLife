using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
public class NPCManager : Singleton<NPCManager>
{
    [LabelText("NPC 목록")]
    [SerializeField] NPCLIST[] npclist;

    [LabelText("스위치 조건 문")]
    public IFLIST[] npcifList;

}
[System.Serializable]
public class NPCLIST
{
    public string name;
    public GameObject npc_object;

    public IEnumerator HideObejct()
    {
        npc_object.GetComponent<SpriteRenderer>().DOFade(0f, 1.0f);
        yield return new WaitForSeconds(1.5f);
        npc_object.SetActive(false);
    }
}

[System.Serializable]
public class IFLIST
{
    public string ifname;

    public bool is_bool;
}