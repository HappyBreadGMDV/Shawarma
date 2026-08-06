using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : Interaction
{
    public GameObject Open;
    public GameObject Close;

    public override void AnotherClick()
    {
        Open.SetActive(!Open.active);
        Close.SetActive(!Close.active);
    }
}
