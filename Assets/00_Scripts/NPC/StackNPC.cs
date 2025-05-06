using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackNPC : MessageTrigger
{
    public override void OnClickSpaceBar()
    {
        UIManager.Instance.ChangeState(UIState.StackUI);
    }
}
