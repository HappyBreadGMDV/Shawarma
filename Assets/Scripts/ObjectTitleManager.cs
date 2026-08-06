using TMPro;
using UnityEngine;

public class ObjectTitleManager : MonoBehaviour
{
    public TextMeshProUGUI TextName;
    public TextMeshProUGUI TextDescription;
    public float rayDistance = 5f;

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
            Debug.LogError("Main Camera не найдена!");
    }

    private void Update()
    {
        if (mainCamera == null) return;

        // Луч из камеры через позицию мыши
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            var interaction = hit.collider.GetComponent<Interaction>();

            if (interaction != null && !interaction.Name.IsEmpty && !interaction.Description.IsEmpty)
            {
                TextName.text = interaction.Name.GetLocalizedString().Replace("\\n", "\n");
                TextDescription.text = interaction.Description.GetLocalizedString().Replace("\\n", "\n");
            }
            else
            {
                TextName.text = "";
                TextDescription.text = "";
            }
        }
        else
        {
            TextName.text = "";
            TextDescription.text = "";
        }
    }
}