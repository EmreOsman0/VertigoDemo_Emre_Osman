using UnityEngine;
using UnityEngine.UI;

public class WheelView : MonoBehaviour
{
    [SerializeField] private WheelConfig config;
    [SerializeField] private Image wheelImage;
    [SerializeField] private RectTransform sliceContainer;
    [SerializeField] private WheelSliceView slicePrefab;
    [SerializeField] private float radius = 150f;

    private void Start()
    {
        Build(config);
    }

    public void Build(WheelConfig wheelConfig)
    {
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
}
