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
            if (curAction == action) return;    // PlayerCtrl에서 반환되는 값이 있는지 확인
            if (curAction != null)  // 반환 값이 없으면 Movement 혹은 Combat 취소
            {
                curAction.Cancel();
            }
            curAction = action;
        }
    }
}
