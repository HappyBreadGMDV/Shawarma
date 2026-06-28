using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneBox : MonoBehaviour
{
    public GameObject[] Objects;

    private void OnCollisionEnter(Collision collision)
    {
        foreach (var item in Objects)
        {
            var inst = Instantiate(item, transform.position, Quaternion.identity);
        }

        Destroy(this.gameObject);
    }
}
