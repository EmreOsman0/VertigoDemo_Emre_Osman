using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Wheel/Wheel Config")]
public class WheelConfig : ScriptableObject
{
    [SerializeField] private Sprite wheelSprite;
    [SerializeField] private WheelSlice[] slices;
    public Sprite WheelSprite => wheelSprite;
    public IReadOnlyList<WheelSlice> Slices => slices;
}
