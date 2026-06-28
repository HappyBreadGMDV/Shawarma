using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class Button3D : MonoBehaviour
{
    public UnityEvent OnEnter;
    public UnityEvent OnExit;
    public UnityEvent OnClick;

    private void OnMouseDown()
    {
        if (OnClick != null)
        {
            OnClick.Invoke();
        }
    }

    private void OnMouseEnter()
    {
        if (OnEnter != null)
        {
            OnEnter.Invoke();
        }
    }

    private void OnMouseExit()
    {
        if (OnExit != null)
        {
            OnExit.Invoke();
        }
    }
}
