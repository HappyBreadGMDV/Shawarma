using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
            var objectTitle = hit.collider.GetComponent<Interaction>();

            // ѕровер€ем, что компонент найден и его локализаци€ настроена
            if (objectTitle != null && objectTitle.Name != null && objectTitle.Description != null)
            {
                // —начала получаем чистый string через .GetLocalizedString(), 
                // и только потом замен€ем символы переноса строки
                string nameText = objectTitle.Name.GetLocalizedString().Replace("\\n", "\n");
                string descText = objectTitle.Description.GetLocalizedString().Replace("\\n", "\n");

                TextName.text = nameText;
                TextDescription.text = descText;

                // ѕринудительно обновл€ем меш (на вс€кий случай)
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
