//using System;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Events;

//public class ShwarmaCooking : MonoBehaviour
//{
//    [Serializable]
//    public class CookingData
//    {
//        public GameObject Object;         // префаб, чьё имя будет сравниваться
//        public bool DestroyObject;
//        public bool oneshot = true;       // сработать только один раз
//        public bool NoCollider = false;
//        public UnityEvent Event;

//        [HideInInspector] public bool used;   // внутренний флаг
//    }

//    public List<CookingData> cookingData = new List<CookingData>();

//    private void OnTriggerEnter(Collider other)
//    {
//        foreach (var item in cookingData)
//        {
//            // Если элемент не настроен – пропускаем
//            if (item.Object == null) continue;

//            // Сравниваем имена (Contains позволяет игнорировать "(Clone)")
//            if (other.gameObject.name.Contains(item.Object.name))
//            {
//                // Если одноразовое событие и оно уже было – пропускаем
//                if (item.oneshot && item.used) continue;
//                if (item.NoCollider == false) continue;

//                item.Event.Invoke();
//                item.used = true;

//                if (item.DestroyObject)
//                {
//                    Destroy(other.gameObject);
//                    // После уничтожения объекта нет смысла проверять другие элементы
//                    break;
//                }
//            }
//        }
//    }

//    private void OnCollisionEnter(Collision collision)
//    {
//        foreach (var item in cookingData)
//        {
//            // Если элемент не настроен – пропускаем
//            if (item.Object == null) continue;

//            // Сравниваем имена (Contains позволяет игнорировать "(Clone)")
//            if (collision.gameObject.name.Contains(item.Object.name))
//            {
//                // Если одноразовое событие и оно уже было – пропускаем
//                if (item.oneshot && item.used) continue;
//                if (item.NoCollider == false) continue;

//                item.Event.Invoke();
//                item.used = true;

//                if (item.DestroyObject)
//                {
//                    Destroy(collision.gameObject);
//                    // После уничтожения объекта нет смысла проверять другие элементы
//                    break;
//                }
//            }
//        }
//    }

//    public void AddIngredient(GameObject Object)
//    {
//        foreach (var item in cookingData)
//        {
//            // Если элемент не настроен – пропускаем
//            if (item.Object == null) continue;

//            // Сравниваем имена (Contains позволяет игнорировать "(Clone)")
//            if (Object.name.Contains(item.Object.name))
//            {
//                // Если одноразовое событие и оно уже было – пропускаем
//                if (item.oneshot && item.used) continue;

//                item.Event.Invoke();
//                item.used = true;

//                if (item.DestroyObject)
//                {
//                    Destroy(Object);
//                    // После уничтожения объекта нет смысла проверять другие элементы
//                    break;
//                }
//            }
//        }
//    }
//}

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShwarmaCooking : Interaction
{
    [Serializable]
    public class CookingData
    {
        public GameObject Object;         // префаб, чьё имя будет сравниваться
        public bool DestroyObject;
        public bool oneshot = true;       // сработать только один раз
        public bool NoCollider = false;   // если true – НЕ вызывать событие при коллизиях/триггерах
        public UnityEvent Event;

        [HideInInspector] public bool used;   // внутренний флаг
    }

    public GameObject ShawarmaModel;

    public List<CookingData> cookingData = new List<CookingData>();

    private void OnTriggerEnter(Collider other)
    {
        foreach (var item in cookingData)
        {
            if (item.Object == null) continue;

            if (other.gameObject.name.Contains(item.Object.name))
            {
                if (item.oneshot && item.used) continue;
                if (item.NoCollider) continue;   // исправлено: пропускаем, если NoCollider включено

                item.Event.Invoke();
                item.used = true;

                if (item.DestroyObject)
                {
                    Destroy(other.gameObject);
                    break;
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        foreach (var item in cookingData)
        {
            if (item.Object == null) continue;

            if (collision.gameObject.name.Contains(item.Object.name))
            {
                if (item.oneshot && item.used) continue;
                if (item.NoCollider) continue;   // исправлено

                item.Event.Invoke();
                item.used = true;

                if (item.DestroyObject)
                {
                    Destroy(collision.gameObject);
                    break;
                }
            }
        }
    }

    public void AddIngredient(GameObject Object)
    {
        foreach (var item in cookingData)
        {
            if (item.Object == null) continue;

            if (Object.name.Contains(item.Object.name))
            {
                if (item.oneshot && item.used) continue;

                item.Event.Invoke();
                item.used = true;

                if (item.DestroyObject)
                {
                    Destroy(Object);
                    break;
                }
            }
        }
    }

    public override void AnotherClick()
    {
        var rotation = Quaternion.Euler(transform.rotation.x + 90f, transform.rotation.y, transform.rotation.z);

        var inst = Instantiate(ShawarmaModel, transform.position, rotation);
        ShwarmaCooked newCooking = inst.AddComponent<ShwarmaCooked>();

        newCooking.cookingData = new List<CookingData>();
        foreach (var item in this.cookingData)
        {
            if (item.used && item.Object != null)
            {
                CookingData newItem = new CookingData();
                newItem.Object = item.Object;
                newItem.DestroyObject = item.DestroyObject;
                newItem.oneshot = item.oneshot;
                newItem.NoCollider = item.NoCollider;
                newItem.used = false; // в готовой шаурме сбрасываем
                newItem.Event = item.Event; // если нужно
                newCooking.cookingData.Add(newItem);
            }
        }
        Destroy(this.gameObject);
    }
}