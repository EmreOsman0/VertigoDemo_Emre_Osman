using System.Collections.Generic;

public class GameModel
{
    private const int SafeZoneInterval = 5;
    private const int SuperZoneInterval = 30;

    private int zone = 1;
    private readonly Dictionary<RewardData, int> rewards = new Dictionary<RewardData, int>();

    public int Zone { get { return zone; } }
    public IReadOnlyDictionary<RewardData, int> Rewards { get { return rewards; } }
    public bool IsSuperZone { get { return zone % SuperZoneInterval == 0; } }
    public bool IsSafeZone { get { return !IsSuperZone && zone % SafeZoneInterval == 0; } }
    public bool CanLeave { get { return IsSafeZone || IsSuperZone; } }

    public void AddReward(WheelSlice slice)
    {
        rewards.TryGetValue(slice.reward, out int current);
        rewards[slice.reward] = current + slice.amount;
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
