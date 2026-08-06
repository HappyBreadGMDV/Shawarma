using UnityEngine;

public class SauseContainer : MonoBehaviour
{
    public float GrabSubstract = 0.01f;
    public float MaxSouse;
    public float Souse;
    public Transform SouseTransform;
    public Transform SouseStart;
    public Transform SouseEnd;

    public void UpdateSousePosition()
    {
        if (Souse < GrabSubstract)
            return;

        Souse -= GrabSubstract;

        if (SouseTransform == null || SouseStart == null || SouseEnd == null)
            return;

        float progress = Mathf.Clamp01(Souse / MaxSouse);
        SouseTransform.localPosition = Vector3.Lerp(SouseStart.localPosition, SouseEnd.localPosition, progress);
    }


}