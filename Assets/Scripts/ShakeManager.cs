using UnityEngine;
using DG.Tweening;

public class ShakeManager : MonoBehaviour
{
    Transform originalCamPos;
    public void Shake()
    {
        Camera mainCamera = Camera.main;
        originalCamPos = mainCamera.transform;
        DOTween.Sequence()
            .Append(mainCamera.transform.DOShakePosition(0.25f, new Vector3(0.5f, 0.5f, 0), 10, 90, false))
            .Append(mainCamera.transform.DOShakeRotation(0.25f, new Vector3(0, 0, 10), 10, 90, false))
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                mainCamera.transform.localPosition = originalCamPos.localPosition;
                mainCamera.transform.localRotation = Quaternion.identity;
            });
    }
}
