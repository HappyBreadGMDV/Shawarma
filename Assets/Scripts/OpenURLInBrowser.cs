using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenURLInBrowser : MonoBehaviour
{
    public void OpenURL(string path = "http://unity3d.com/")
    {
        Application.OpenURL(path);
    }
}
