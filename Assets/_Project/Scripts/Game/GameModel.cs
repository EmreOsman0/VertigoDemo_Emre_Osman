using System.Collections.Generic;

public class GameModel
{
    private const int SafeZoneInterval = 5;
    private const int SuperZoneInterval = 30;

    private int zone = 1;
    private readonly Dictionary<RewardData, int> rewards = new Dictionary<RewardData, int>();

    public static bool IsSuper(int zone) { return zone % SuperZoneInterval == 0; }
    public static bool IsSafe(int zone) { return !IsSuper(zone) && zone % SafeZoneInterval == 0; }

    public int Zone { get { return zone; } }
    public IReadOnlyDictionary<RewardData, int> Rewards { get { return rewards; } }
    public bool IsSuperZone { get { return IsSuper(zone); } }
    public bool IsSafeZone { get { return IsSafe(zone); } }
    public bool CanLeave { get { return IsSafeZone || IsSuperZone; } }

    public void AddReward(WheelSlice slice, int multiplier)
    {
        rewards.TryGetValue(slice.reward, out int current);
        rewards[slice.reward] = current + slice.amount * multiplier;
        zone++;
    }

    public void Reset()
    {
        zone = 1;
        rewards.Clear();
    }

#if UNITY_EDITOR
    public void SetZone(int value)
    {
        if (value < 1) value = 1;
        zone = value;
    }
#endif
}
