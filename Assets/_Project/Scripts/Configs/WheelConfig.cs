using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Wheel/Wheel Config")]
public class WheelConfig : ScriptableObject
{
    [SerializeField] private Sprite wheelSprite;
    [SerializeField] private Sprite indicatorSprite;
    [SerializeField] private WheelSlice[] slices;
    public Sprite WheelSprite { get { return wheelSprite; } }
    public Sprite IndicatorSprite { get { return indicatorSprite; } }
    public IReadOnlyList<WheelSlice> Slices { get { return slices; } }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (slices == null) return;

        for (int i = 0; i < slices.Length; i++)
            slices[i].Label = $"Slice {i + 1}";
    }
#endif
}
