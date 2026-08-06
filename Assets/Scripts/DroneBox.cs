using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneBox : MonoBehaviour
{
    public GameObject[] Objects;
    public Vector3 OldVector3;
    public Transform OldParent;

    private void Start()
    {
        OldVector3 = transform.localPosition;
    }

    private void OnEnable()
    {
        this.transform.localPosition = OldVector3;
        this.transform.SetParent(OldParent);
    }

    private void OnCollisionEnter(Collision collision)
    {
        foreach (var item in Objects)
        {
            var inst = Instantiate(item, transform.position, Quaternion.identity);
        }

        this.transform.localPosition = OldVector3;
        this.transform.SetParent(OldParent);
        this.gameObject.SetActive(false);
    }
}
