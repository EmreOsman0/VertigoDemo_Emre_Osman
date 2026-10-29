using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WheelSliceView : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI amountText;

    public void Setup(WheelSlice slice, int multiplier)
    {
        icon.sprite = slice.reward.Icon;
        amountText.text = "x" + (slice.amount * multiplier);
        amountText.gameObject.SetActive(!slice.reward.IsBomb);
    }

    public void PlayWinAnimation()
    {
        icon.rectTransform.DOComplete();
        icon.rectTransform.DOPunchScale(Vector3.one * 0.4f, 0.5f, 6);
    }
}
