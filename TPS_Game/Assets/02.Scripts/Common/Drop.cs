using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DataInfo;
public class Drop : MonoBehaviour,IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {      //�ڽ� ������Ʈ�� �����  �����
        Debug.Log("OnDrop called.");
        if (transform.childCount == 0)
        {
            Debug.Log("OnDrop ifcalled.");
            Drag.draggingItem.transform.SetParent(this.transform);
            Item item = Drag.draggingItem.GetComponent<ItemInfo>().itemData;
            GameManager.Instance.AddItem(item); // �������� ���� �Ŵ����� �߰�
        }
    }
}
