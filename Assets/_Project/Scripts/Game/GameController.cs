using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] private WheelView wheelView;
    [SerializeField] private Button spinButton;
    [SerializeField] private Button leaveButton;
    [SerializeField] private TextMeshProUGUI zoneText;
    [SerializeField] private WheelConfig bronzeConfig;
    [SerializeField] private WheelConfig silverConfig;
    [SerializeField] private WheelConfig goldenConfig;

    private GameModel model = new GameModel();

#if UNITY_EDITOR
    private void OnValidate()
    {
        Transform spinButtonTransform = transform.Find("ui_wheel_buttons/ui_button_spin");
        if (spinButtonTransform != null)
            spinButton = spinButtonTransform.GetComponent<Button>();

        Transform leaveButtonTransform = transform.Find("ui_wheel_buttons/ui_button_leave");
        if (leaveButtonTransform != null)
            leaveButton = leaveButtonTransform.GetComponent<Button>();

        Transform zoneTextTransform = transform.Find("ui_text_zone_value");
        if (zoneTextTransform != null)
            zoneText = zoneTextTransform.GetComponent<TextMeshProUGUI>();
    }
#endif

    private void OnEnable()
    {
        spinButton.onClick.AddListener(OnSpinClicked);
        leaveButton.onClick.AddListener(OnLeaveClicked);
    }

    private void OnDisable()
    {
        spinButton.onClick.RemoveListener(OnSpinClicked);
        leaveButton.onClick.RemoveListener(OnLeaveClicked);
    }

    private void Start()
    {
        wheelView.Build(GetConfigForZone());
        UpdateUI();
    }

    private WheelConfig GetConfigForZone()
    {
        if (model.IsSuperZone) return goldenConfig;
        if (model.IsSafeZone) return silverConfig;
        return bronzeConfig;
    }

#if UNITY_EDITOR
    public void DebugSetZone(int zone)
    {
        if (wheelView.IsSpinning) return;
        model.SetZone(zone);
        wheelView.Build(GetConfigForZone());
        UpdateUI();
    }
#endif

    private void OnSpinClicked()
    {
        spinButton.interactable = false;
        wheelView.Spin(OnSpinFinished);
        UpdateUI();
    }

    private void OnSpinFinished(WheelSlice slice)
    {
        spinButton.interactable = true;

        if (slice.reward.IsBomb)
        {
            Debug.Log("Bomb! Lost everything.");
            model.Reset();
        }
        else
        {
            model.AddReward(slice);
            Debug.Log("Won: " + slice.reward.RewardName + " x" + slice.amount + " | Zone: " + model.Zone);
        }

        wheelView.Build(GetConfigForZone());
        UpdateUI();
    }

    private void OnLeaveClicked()
    {
        foreach (KeyValuePair<RewardData, int> reward in model.Rewards)
            Debug.Log("Collected: " + reward.Key.RewardName + " x" + reward.Value);

        model.Reset();
        wheelView.Build(GetConfigForZone());
        UpdateUI();
    }

    private void UpdateUI()
    {
        leaveButton.interactable = model.CanLeave && !wheelView.IsSpinning;
        zoneText.text = "Zone " + model.Zone;
    }
}
