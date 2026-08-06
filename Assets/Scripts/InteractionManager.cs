using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public float rayDistance;

    public KeyCode interactionKey = KeyCode.E;
    public KeyCode fKey = KeyCode.F;
    private bool isCliked;

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
            Debug.LogError("Main Camera не найдена!");
    }

    void Update()
    {
        if (Input.GetKey(interactionKey))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;


            if (Physics.Raycast(ray, out hit, rayDistance) && !isCliked)
            {
                if (hit.collider.gameObject.GetComponent<Interaction>() != null)
                {
                    hit.collider.gameObject.GetComponent<Interaction>().Click();
                }
                isCliked = true;
            }
        }
        else if (Input.GetKey(fKey))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;


            if (Physics.Raycast(ray, out hit, rayDistance) && !isCliked)
            {
                if (hit.collider.gameObject.GetComponent<Interaction>() != null)
                {
                    hit.collider.gameObject.GetComponent<Interaction>().AnotherClick();
                }
                isCliked = true;
            }
        }
        else
        {
            isCliked = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (Camera.main == null) return;

        Gizmos.color = Color.red;
        Vector3 direction = Camera.main.transform.forward;
        Gizmos.DrawLine(transform.position, transform.position + direction * rayDistance);
    }
}
