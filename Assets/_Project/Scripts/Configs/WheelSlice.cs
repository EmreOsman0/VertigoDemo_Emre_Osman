using UnityEngine;

[System.Serializable]
public struct WheelSlice
{
    [SerializeField, HideInInspector] private string label;
    public RewardData reward;
    public int amount;

    public string Label { get { return label; } set { label = value; } }
}
