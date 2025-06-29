using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;     //드래그 핸드러 ,드래그 시작     드래그가 끝났을 때       
public class Drag : MonoBehaviour,IDragHandler,IBeginDragHandler,IEndDragHandler
{
    [SerializeField]
    private Transform itemTr;
    [SerializeField]
    private Transform inventroyTr;
    [SerializeField]
    private Transform itemListTr;
    public static GameObject draggingItem = null;
    private CanvasGroup canvasGroup;
    void Start()
    {
        itemTr = GetComponent<Transform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }
    //드래그 이벤트 
    public void OnDrag(PointerEventData eventData)
    {
        itemTr.position = Input.mousePosition;
        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //드래그가 시작 되었을 때 부모를 Inventory로 한다
       this.transform.SetParent(inventroyTr);

        //드래그가 시작되면 드래그 되는 아이템 정보를 저장함 
        draggingItem = this.gameObject;
        //드래그가  시작되면 다른 UI이벤트를 받지 않도록 설정
        canvasGroup.blocksRaycasts = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //드래그가 종료 되었을 때 null 정보를 넣어준다
        draggingItem = null;
        canvasGroup.blocksRaycasts = true; // 드래그가 끝나면 UI 이벤트를 받아야 함

        //슬롯에 드래그 되지 않았을 때 윈래대로 ItemList로 돌린다.
        if(itemTr.parent ==inventroyTr)
        {
            itemTr.SetParent(itemListTr.transform);
            GameManager.Instance.RemoveItem(GetComponent<ItemInfo>().itemData); // 슬롯에 추가된 아이템의 갱신을 알림
        }
    }
}
