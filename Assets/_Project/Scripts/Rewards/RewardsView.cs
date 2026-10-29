using System.Collections.Generic;
using UnityEngine;

public class RewardsView : MonoBehaviour
{
    [SerializeField] private RectTransform content;
    [SerializeField] private RewardItemView itemPrefab;

    private readonly List<RewardItemView> items = new List<RewardItemView>();

    public void Show(IReadOnlyDictionary<RewardData, int> rewards)
    {
        int index = 0;
        foreach (KeyValuePair<RewardData, int> reward in rewards)
        {
            if (index >= items.Count)
                items.Add(Instantiate(itemPrefab, content));

            items[index].gameObject.SetActive(true);
            items[index].Setup(reward.Key, reward.Value);
            index++;
        }

        for (int i = index; i < items.Count; i++)
            items[i].gameObject.SetActive(false);
    }
}
