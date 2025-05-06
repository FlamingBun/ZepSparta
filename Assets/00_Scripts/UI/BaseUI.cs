using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseUI : MonoBehaviour
{
    protected abstract UIState GetUIState();

    public virtual void Init()
    {
    }

    public virtual void SetActive(UIState state)
    {
        gameObject.SetActive(GetUIState() == state);
    }

    protected void CloseUIs()
    {
        // Debug.Log("CloseUIs 호출됨\n" + System.Environment.StackTrace);
        UIManager.Instance.ChangeState(UIState.None);
    }
}
