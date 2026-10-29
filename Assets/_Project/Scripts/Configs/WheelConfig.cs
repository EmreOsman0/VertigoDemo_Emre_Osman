using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Wheel/Wheel Config")]
public class WheelConfig : ScriptableObject
{
    public enum WheelType { Bronze, Silver, Golden }

    [SerializeField] private WheelType type;
    [SerializeField] private Sprite wheelSprite;
    [SerializeField] private Sprite indicatorSprite;
    [SerializeField] private WheelSlice[] slices;
    public Sprite WheelSprite { get { return wheelSprite; } }
    public Sprite IndicatorSprite { get { return indicatorSprite; } }
    public IReadOnlyList<WheelSlice> Slices { get { return slices; } }
    public WheelType Type { get { return type; } }

    public static WheelType TypeForZone(int zone)
    {
        if (GameModel.IsSuper(zone)) return WheelType.Golden;
        if (GameModel.IsSafe(zone)) return WheelType.Silver;
        return WheelType.Bronze;
    }

    public bool HasBomb()
    {
        for (int i = 0; i < slices.Length; i++)
            if (slices[i].reward != null && slices[i].reward.IsBomb) return true;
        return false;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (slices == null) return;

        for (int i = 0; i < slices.Length; i++)
            slices[i].Label = $"Slice {i + 1}";

        if (type != WheelType.Bronze && HasBomb())
            Debug.LogError(name + ": " + type + " wheel cannot have a bomb.", this);
    }
#endif
}
