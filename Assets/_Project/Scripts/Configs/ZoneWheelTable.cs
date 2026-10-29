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

    [SerializeField] private ZoneWheel[] zoneWheels;

    public WheelConfig GetWheel(int zone)
    {
        for (int i = 0; i < zoneWheels.Length; i++)
        {
            if (zoneWheels[i].zone == zone)
                return zoneWheels[i].wheel;
        }
        return null;
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
