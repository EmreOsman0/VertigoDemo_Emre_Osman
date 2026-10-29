using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardItemView : MonoBehaviour
{
    [SerializeField] private Image box;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI amountText;

    public void Setup(RewardData reward, int amount, Sprite boxSprite)
    {
        box.sprite = boxSprite;
        icon.sprite = reward.Icon;
        amountText.text = "x" + amount;
    }
}
