using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class WheelView : MonoBehaviour
{
    [SerializeField] private Image wheelImage;
    [SerializeField] private Image indicatorImage;
    [SerializeField] private RectTransform sliceContainer;
    [SerializeField] private WheelSliceView slicePrefab;
    [SerializeField] private float radius = 150f; // The radius of the wheel where slices will be positioned
    [SerializeField] private float spinDuration = 3f;
    [SerializeField] private int spinRounds = 5;
    [SerializeField] private int maxExtraRounds = 2;

    private readonly List<WheelSliceView> slices = new List<WheelSliceView>();
    private WheelConfig currentConfig;
    private bool isSpinning;

    public bool IsSpinning { get { return isSpinning; } }
    public WheelConfig CurrentConfig { get { return currentConfig; } }

    public void Build(WheelConfig wheelConfig, int multiplier)
    {
        currentConfig = wheelConfig;
        wheelImage.sprite = wheelConfig.WheelSprite;
        indicatorImage.sprite = wheelConfig.IndicatorSprite;

        float step = 360f / wheelConfig.Slices.Count;

        for (int i = 0; i < wheelConfig.Slices.Count; i++)
        {
            if (i >= slices.Count)
                slices.Add(Instantiate(slicePrefab, sliceContainer));

            WheelSliceView slice = slices[i];
            slice.gameObject.SetActive(true);

            float angle = i * step;
            float rad = angle * Mathf.Deg2Rad;

            RectTransform rect = (RectTransform)slice.transform;
            rect.anchoredPosition = new Vector2(Mathf.Sin(rad), Mathf.Cos(rad)) * radius;
            rect.localEulerAngles = new Vector3(0, 0, -angle);
            slice.Setup(wheelConfig.Slices[i], multiplier);
        }

        for (int i = wheelConfig.Slices.Count; i < slices.Count; i++)
            slices[i].gameObject.SetActive(false);
    }

    public void Spin(System.Action<WheelSlice> onComplete)
    {
        if (isSpinning) return;

        int index = Random.Range(0, currentConfig.Slices.Count);
        SpinToIndex(index, onComplete);
    }

#if UNITY_EDITOR
    public void DebugSpinToSlice(int index, System.Action<WheelSlice> onComplete)
    {
        if (index < 0 || index >= currentConfig.Slices.Count)
            return;

        if (isSpinning)
            return;

        SpinToIndex(index, onComplete);
    }
#endif

    private void SpinToIndex(int index, System.Action<WheelSlice> onComplete)
    {
        isSpinning = true;

        float step = 360f / currentConfig.Slices.Count;
        int extraRounds = Random.Range(0, maxExtraRounds + 1);
        float targetAngle = (spinRounds + extraRounds) * 360f + index * step;

        wheelImage.rectTransform
            .DOLocalRotate(new Vector3(0f, 0f, targetAngle), spinDuration, RotateMode.FastBeyond360)
            .SetEase(Ease.OutQuart)
            .OnComplete(delegate
            {
                isSpinning = false;
                indicatorImage.rectTransform.DOComplete();
                indicatorImage.rectTransform.DOPunchRotation(new Vector3(0f, 0f, 15f), 0.4f, 8);
                slices[index].PlayWinAnimation();
                onComplete(currentConfig.Slices[index]);
            });
    }
}
