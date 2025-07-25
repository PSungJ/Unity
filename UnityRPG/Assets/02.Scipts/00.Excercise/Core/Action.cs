using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class Actions : MonoBehaviour
    {
        IAction curAction;

        public void ActionStart(IAction action)
        {
            if (curAction == action) return;    // PlayerCtrl���� ��ȯ�Ǵ� ���� �ִ��� Ȯ��
            if (curAction != null)  // ��ȯ ���� ������ Movement Ȥ�� Combat ���
            {
                curAction.Cancel();
            }
            curAction = action;
        }
    }
}
