using UnityEngine;

[CreateAssetMenu(menuName = "Wheel/Reward")]
public class RewardData : ScriptableObject
{
    [SerializeField] private string rewardName;
    [SerializeField] private Sprite icon;
    [SerializeField] private bool isBomb;

    public string RewardName => rewardName;
    public Sprite Icon => icon;
    public bool IsBomb => isBomb;
}