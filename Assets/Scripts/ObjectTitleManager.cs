using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization; // опционально, IsEmpty уже доступен

public class ObjectTitleManager : MonoBehaviour
{
    public TextMeshProUGUI TextName;
    public TextMeshProUGUI TextDescription;

    public float rayDistance;

    private void FixedUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            var interaction = hit.collider.GetComponent<Interaction>();

            // Проверяем, что компонент есть и оба локализованных поля заполнены
            if (interaction != null && !interaction.Name.IsEmpty && !interaction.Description.IsEmpty)
            {
                string nameText = interaction.Name.GetLocalizedString().Replace("\\n", "\n");
                string descText = interaction.Description.GetLocalizedString().Replace("\\n", "\n");

                TextName.text = nameText;
                TextDescription.text = descText;

                TextName.ForceMeshUpdate();
                TextDescription.ForceMeshUpdate();
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