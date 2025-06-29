using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;     //�巡�� �ڵ巯 ,�巡�� ����     �巡�װ� ������ ��       
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
    //�巡�� �̺�Ʈ 
    public void OnDrag(PointerEventData eventData)
    {
        itemTr.position = Input.mousePosition;
        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //�巡�װ� ���� �Ǿ��� �� �θ� Inventory�� �Ѵ�
       this.transform.SetParent(inventroyTr);

        //�巡�װ� ���۵Ǹ� �巡�� �Ǵ� ������ ������ ������ 
        draggingItem = this.gameObject;
        //�巡�װ�  ���۵Ǹ� �ٸ� UI�̺�Ʈ�� ���� �ʵ��� ����
        canvasGroup.blocksRaycasts = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //�巡�װ� ���� �Ǿ��� �� null ������ �־��ش�
        draggingItem = null;
        canvasGroup.blocksRaycasts = true; // �巡�װ� ������ UI �̺�Ʈ�� �޾ƾ� ��

        //���Կ� �巡�� ���� �ʾ��� �� ������� ItemList�� ������.
        if(itemTr.parent ==inventroyTr)
        {
            itemTr.SetParent(itemListTr.transform);
            GameManager.Instance.RemoveItem(GetComponent<ItemInfo>().itemData); // ���Կ� �߰��� �������� ������ �˸�
        }
    }
}
