using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

public class Interaction : MonoBehaviour
{
    public LocalizedString Name;
    public LocalizedString Description;

    public virtual void AnotherClick()
    {
        Debug.Log("interaction F");
    }

    public virtual void Click()
    {
        Debug.Log("interaction");
    }
}
