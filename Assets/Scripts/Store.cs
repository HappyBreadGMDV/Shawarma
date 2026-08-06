using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;

public class Store : MonoBehaviour
{
    public TMP_Text priceText;

    [Header("Scripts")]
    public Drone DroneScript;
    public PauseMenu PauseScript;
    public FirstPersonController FirstPersonControllerScript;
    public MoneyManager MoneyManagerScript;

    [Header("Prefabs")]
    public GameObject ProductPrefab;
    public GameObject MiniProductPrefab;

    [Header("Parents")]
    public Transform ProductsListParent;
    public Transform MiniProductsListParent;

    [Header("Lists/Debug")]
    public List<Product> Products = new List<Product>();
    public List<Product> ProductsBuy = new List<Product>();

    [Serializable]
    public class Product
    {
        public LocalizedString Name;
        public int Price;
        public GameObject Object;
        public Sprite Icon;
        [HideInInspector] public GameObject IconPlane;
    }

    private void OnEnable()
    {
        PauseScript.enabled = false;
        FirstPersonControllerScript.cameraCanMove = false;
        FirstPersonControllerScript.playerCanMove = false;
        Cursor.lockState = CursorLockMode.None;
    }

    private void OnDisable()
    {
        PauseScript.enabled = true;
        FirstPersonControllerScript.cameraCanMove = true;
        FirstPersonControllerScript.playerCanMove = true;
        Cursor.lockState = CursorLockMode.Locked;
        PauseScript.Resume();
    }

    private void Start()
    {
        foreach (var product in Products)
        {
            GameObject item = Instantiate(ProductPrefab, ProductsListParent);
            var display = item.GetComponent<StoreItem>();
            if (display != null)
            {
                display.StoreScript = this;
                display.Setup(product.Name.GetLocalizedString(), product.Price, product.Object, product.Icon);
            }
            else
            {
                Debug.LogWarning("На префабе товара нет компонента StoreItem!");
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }

    public void Add(string productName)
    {
        Product product = Products.Find(p => p.Name.GetLocalizedString() == productName);
        if (product == null) return;

        Product miniProduct = new Product
        {
            Name = product.Name,
            Price = product.Price,
            Object = product.Object,
            Icon = product.Icon
        };

        GameObject miniGO = Instantiate(MiniProductPrefab, MiniProductsListParent);
        var miniScript = miniGO.GetComponent<StoreItemMini>();
        if (miniScript != null)
        {
            miniScript.Setup(miniProduct, this, miniProduct.Icon);
        }

        miniProduct.IconPlane = miniGO;
        ProductsBuy.Add(miniProduct);

        UpdateTotalPrice();
    }

    public void Delete(Product product)
    {
        if (product == null || !ProductsBuy.Contains(product)) return;

        if (product.IconPlane != null)
            Destroy(product.IconPlane);

        ProductsBuy.Remove(product);
        UpdateTotalPrice();
    }

    public void Buy()
    {
        // Дрон занят
        if (DroneScript != null && DroneScript.playableDirector != null &&
            DroneScript.playableDirector.state == UnityEngine.Playables.PlayState.Playing)
            return;

        int totalPrice = ProductsBuy.Sum(p => p.Price);

        // Недостаточно денег
        if (MoneyManagerScript.Money < totalPrice)
        {
            Debug.Log("Недостаточно денег!");
            return;
        }

        // Отправляем объекты дрону
        DroneScript.StartDrone(ProductsBuy.Select(p => p.Object).ToArray());
        MoneyManagerScript.AddMoney(-totalPrice);

        // Очищаем корзину
        for (int i = ProductsBuy.Count - 1; i >= 0; i--)
        {
            Delete(ProductsBuy[i]);
        }
    }

    private void UpdateTotalPrice()
    {
        int total = ProductsBuy.Sum(p => p.Price);
        if (priceText != null)
            priceText.text = $"${total}";
    }
}