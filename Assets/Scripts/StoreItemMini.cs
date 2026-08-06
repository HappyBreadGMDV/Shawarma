using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreItemMini : MonoBehaviour
{
    public TMP_Text Name;
    public Button ButtonDelete;
    public Image Image;
    // Ссылка на продукт, который этот элемент представляет
    public Store.Product ProductData { get; private set; }

    private Store store;

    /// <summary>Инициализация мини-иконки товара в корзине</summary>
    public void Setup(Store.Product product, Store storeScript, Sprite icon)
    {
        ProductData = product;
        store = storeScript;
        // Здесь можно обновить текст, иконку и т.д.
        // Например, если есть TextMeshProUGUI поле nameText:
        Image.sprite = icon;
        Name.text = product.Name.GetLocalizedString();
        ButtonDelete.onClick.RemoveAllListeners();
        ButtonDelete.onClick.AddListener(RemoveFromCart);
    }

    /// <summary>Вызывается при нажатии на кнопку удаления товара</summary>
    public void RemoveFromCart()
    {
        if (store != null && ProductData != null)
            store.Delete(ProductData);
    }
}