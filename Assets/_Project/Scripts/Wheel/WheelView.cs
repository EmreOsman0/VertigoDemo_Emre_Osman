using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class WheelView : MonoBehaviour
{
    [SerializeField] private Image wheelImage;
    [SerializeField] private RectTransform sliceContainer;
    [SerializeField] private WheelSliceView slicePrefab;
    [SerializeField] private float radius = 150f; // The radius of the wheel where slices will be positioned
    [SerializeField] private float spinDuration = 3f;
    [SerializeField] private int spinRounds = 5;
    [SerializeField] private int maxExtraRounds = 2;

    private WheelConfig currentConfig;
    private bool isSpinning;

    public bool IsSpinning { get { return isSpinning; } }

    public void Build(WheelConfig wheelConfig)
    {
        currentConfig = wheelConfig;
        wheelImage.sprite = wheelConfig.WheelSprite;

        foreach (Transform child in sliceContainer)
            Destroy(child.gameObject);

        float step = 360f / wheelConfig.Slices.Count;

        for (int i = 0; i < wheelConfig.Slices.Count; i++)
        {
            float angle = i * step;
            float rad = angle * Mathf.Deg2Rad;

            WheelSliceView slice = Instantiate(slicePrefab, sliceContainer);
            RectTransform rect = (RectTransform)slice.transform;
            rect.anchoredPosition = new Vector2(Mathf.Sin(rad), Mathf.Cos(rad)) * radius;
            rect.localEulerAngles = new Vector3(0, 0, -angle);
            slice.Setup(wheelConfig.Slices[i]);
        }
    }

    public void Spin(System.Action<WheelSlice> onComplete)
    {
        if (isSpinning) return;
        isSpinning = true;

        int index = Random.Range(0, currentConfig.Slices.Count);
        float step = 360f / currentConfig.Slices.Count;
        int extraRounds = Random.Range(0, maxExtraRounds + 1);
        float targetAngle = (spinRounds + extraRounds) * 360f + index * step;

        wheelImage.rectTransform
            .DOLocalRotate(new Vector3(0f, 0f, targetAngle), spinDuration, RotateMode.FastBeyond360)
            .SetEase(Ease.OutQuart)
            .OnComplete(delegate
            {
                isSpinning = false;
                onComplete(currentConfig.Slices[index]);
            });
    }
}
