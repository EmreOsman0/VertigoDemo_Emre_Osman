using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardItemView : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI amountText;

    public void Setup(RewardData reward, int amount)
    {
        icon.sprite = reward.Icon;
        amountText.text = "x" + amount;
    }
}
