using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WheelSliceView : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI amountText;

    public void Setup(WheelSlice slice)
    {
        icon.sprite = slice.reward.Icon;
        amountText.text = "x" + slice.amount;
        amountText.gameObject.SetActive(!slice.reward.IsBomb);
    }
}
