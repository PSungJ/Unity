using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField]
    private Transform itemTr;
    [SerializeField]
    private Transform inventroyTr;
    [SerializeField]
    private Transform itemListTr;
    public static GameObject draggingItem = null;
    public CanvasGroup canvasGroup;

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
        this.transform.SetParent(inventroyTr);
        draggingItem = this.gameObject;
        canvasGroup.blocksRaycasts = false;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        draggingItem = null;
        canvasGroup.blocksRaycasts = true;

        if (itemTr.parent == inventroyTr)
        {
            itemTr.SetParent(itemListTr.transform);
            GameManager.Instance.RemoveItem(GetComponent<ItemInfo>().itemData);
        }
    }
}
