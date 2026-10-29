using UnityEngine;

[CreateAssetMenu(menuName = "Wheel/Zone Wheel Table")]
public class ZoneWheelTable : ScriptableObject
{
    [System.Serializable]
    public struct ZoneWheel
    {
        public int zone;
        public WheelConfig wheel;
    }

    [SerializeField] private WheelConfig bronzeWheel;
    [SerializeField] private WheelConfig silverWheel;
    [SerializeField] private WheelConfig goldenWheel;
    [SerializeField] private ZoneWheel[] zoneWheels;

    public WheelConfig GetWheel(int zone, out int multiplier)
    {
        for (int i = 0; i < zoneWheels.Length; i++)
        {
            if (zoneWheels[i].zone == zone && zoneWheels[i].wheel != null)
            {
                multiplier = 1;
                return zoneWheels[i].wheel;
            }
        }

        multiplier = zone;
        return GetDefaultWheel(zone);
    }

    private WheelConfig GetDefaultWheel(int zone)
    {
        WheelConfig.WheelType type = WheelConfig.TypeForZone(zone);
        if (type == WheelConfig.WheelType.Golden) return goldenWheel;
        if (type == WheelConfig.WheelType.Silver) return silverWheel;
        return bronzeWheel;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (zoneWheels == null) return;
        for (int i = 0; i < zoneWheels.Length; i++)
        {
            WheelConfig wheel = zoneWheels[i].wheel;
            if (wheel == null) continue;

            WheelConfig.WheelType expected = WheelConfig.TypeForZone(zoneWheels[i].zone);
            if (wheel.Type != expected)
                Debug.LogError("Zone " + zoneWheels[i].zone + " needs a " + expected + " wheel, but " + wheel.name + " is " + wheel.Type + ".", this);
        }
    }
#endif
}
