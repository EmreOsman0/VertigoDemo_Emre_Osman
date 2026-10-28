using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Wheel/Wheel Config")]
public class WheelConfig : ScriptableObject
{
    [SerializeField] private Sprite wheelSprite;
    [SerializeField] private WheelSlice[] slices;
    public Sprite WheelSprite { get { return wheelSprite; } }
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
