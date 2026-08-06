using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreItem : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text priceText;
    public Button buyButton;
    public Store StoreScript;
    public Image Image;

    private GameObject itemObject;   // объект, который будет создаваться при покупке

    public void Setup(string productName, int price, GameObject objectToSell, Sprite icon)
    {
        nameText.text = productName;
        priceText.text = $"${price}";
        itemObject = objectToSell;
        Image.sprite = icon;
        // Очищаем старые слушатели и добавляем новый
        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(Buy);
    }

    private void Buy()
    {
        // Здесь логика покупки: списать деньги, создать предмет и т.д.
        StoreScript.Add(nameText.text);
    }
}
