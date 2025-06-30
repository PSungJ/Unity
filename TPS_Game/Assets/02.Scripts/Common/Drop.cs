using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DataInfo;
public class Drop : MonoBehaviour,IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {      //자식 오브젝트가 없어야  드랍됨
        Debug.Log("OnDrop called.");
        if (transform.childCount == 0)
        {
            Debug.Log("OnDrop ifcalled.");
            Drag.draggingItem.transform.SetParent(this.transform);
            Item item = Drag.draggingItem.GetComponent<ItemInfo>().itemData;
            GameManager.Instance.AddItem(item); // 아이템을 게임 매니저에 추가
        }
    }
}
