using UnityEngine;
using DG.Tweening;

public class ShakeManager : MonoBehaviour
{
    Vector3 originalPos;
    Quaternion originalRot;
    public void Shake()
    {
        Camera mainCamera = Camera.main;

        mainCamera.transform.DOKill(true);
        originalPos = mainCamera.transform.localPosition;
        originalRot = mainCamera.transform.localRotation;

        DOTween.Sequence()
            .Append(mainCamera.transform.DOShakePosition(0.5f, 0.5f))
            .Join(mainCamera.transform.DOShakeRotation(0.5f, 0.5f))
            .OnComplete(() =>
            {
                mainCamera.transform.localPosition = originalPos;
                mainCamera.transform.localRotation = originalRot;
            });
    }
}
