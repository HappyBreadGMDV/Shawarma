using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnStartDisable : MonoBehaviour
{
    void Start()
    {
        gameObject.SetActive(false);
    }
}
