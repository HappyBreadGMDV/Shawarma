//using UnityEngine;

//public class ProceduralSkybox : MonoBehaviour
//{
//    public Material MaterialDay;
//    public Material MaterialNight;
//    public float TimeChangeSkybox = 2f;
//    public bool isDay = true;

//    private float DayAlpha = 1;
//    private float NightAlpha = 0;
//    private float timer;

//    void Update()
//    {
//        timer += Time.deltaTime;

//        MaterialDay.SetFloat("Transparent", DayAlpha);
//        MaterialNight.SetFloat("Transparent", NightAlpha);

//        if (isDay)   // сейчас день, переходим к ночи
//        {
//            DayAlpha = Mathf.Clamp01(timer / TimeChangeSkybox);
//            NightAlpha = 1 - DayAlpha;
//            RenderSettings.skybox.Lerp(MaterialDay, MaterialNight, NightAlpha);
//        }
//        else        // сейчас ночь, переходим ко дню
//        {
//            NightAlpha = Mathf.Clamp01(timer / TimeChangeSkybox);
//            DayAlpha = 1 - NightAlpha;
//            RenderSettings.skybox.Lerp(MaterialNight, MaterialDay, DayAlpha);
//        }

//        // Когда время перехода истекло – переключаем режим и сбрасываем таймер
//        if (timer >= TimeChangeSkybox)
//        {
//            isDay = !isDay;
//            timer = 0f;
//        }
//    }
//}

using Unity.VisualScripting;
using UnityEngine;

public class ProceduralSkybox : MonoBehaviour
{
    public float TimeChangeSkybox = 2f;
    public float DelayChangeSkybox = 2f;
    public bool isDay = true;
    public bool delay = false;

    public GameObject DayModel;    // перетащи дневную сферу
    public GameObject NightModel;  // перетащи ночную сферу

    public Light DayLight;    // перетащи дневную сферу
    public Light NightLight;  // перетащи ночную сферу

    private float DayAlpha = 1;
    private float NightAlpha = 0;
    private float timer;

    private Renderer dayRenderer;
    private Renderer nightRenderer;

    void Start()
    {
        if (DayModel) dayRenderer = DayModel.GetComponent<Renderer>();
        if (NightModel) nightRenderer = NightModel.GetComponent<Renderer>();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if(timer >= DelayChangeSkybox && delay == true)
        {
            delay = false;
            timer = 0f;
        }

        if(delay == true) { return; }
        // Пересчитываем альфы в зависимости от направления перехода
        if (isDay)   // день -> ночь
        {
            DayAlpha = Mathf.Clamp01(timer / TimeChangeSkybox);
            NightAlpha = 1 - DayAlpha;
        }
        else        // ночь -> день
        {
            NightAlpha = Mathf.Clamp01(timer / TimeChangeSkybox);
            DayAlpha = 1 - NightAlpha;
        }

        // Применяем прозрачность к материалам моделей
        if (dayRenderer) dayRenderer.material.SetFloat("_Transparent", DayAlpha);
        if (nightRenderer) nightRenderer.material.SetFloat("_Transparent", NightAlpha);

        NightLight.intensity = NightAlpha;
        DayLight.intensity = DayAlpha;

        // Смена дня/ночи по истечении времени
        if (timer >= TimeChangeSkybox)
        {
            isDay = !isDay;
            delay = true;
            timer = 0f;
        }
    }
}