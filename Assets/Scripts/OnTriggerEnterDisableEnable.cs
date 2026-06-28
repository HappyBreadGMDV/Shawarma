using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerEnterDisableEnable : MonoBehaviour
{
    public GameObject[] Enable;
    public GameObject[] Disable;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (var item in Enable)
            {
                item.SetActive(true);
            }

            foreach (var item in Disable)
            {
                item.SetActive(false);
            }
        }
    }
}
