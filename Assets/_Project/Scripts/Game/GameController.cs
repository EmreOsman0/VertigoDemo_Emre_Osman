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
    [SerializeField] private RewardsView rewardsView;
    [SerializeField] private GameObject bombPopup;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button quitButton;
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

        Transform rewardsViewTransform = transform.Find("ui_rewards");
        if (rewardsViewTransform != null)
            rewardsView = rewardsViewTransform.GetComponent<RewardsView>();

        Transform bombPopupTransform = transform.Find("ui_popups/ui_popup_bomb");
        if (bombPopupTransform != null)
            bombPopup = bombPopupTransform.gameObject;

        Transform restartButtonTransform = transform.Find("ui_popups/ui_popup_bomb/ui_button_popup_bomb_restart");
        if (restartButtonTransform != null)
            restartButton = restartButtonTransform.GetComponent<Button>();

        Transform quitButtonTransform = transform.Find("ui_popups/ui_popup_bomb/ui_button_popup_bomb_quit");
        if (quitButtonTransform != null)
            quitButton = quitButtonTransform.GetComponent<Button>();
    }
#endif

    private void OnEnable()
    {
        spinButton.onClick.AddListener(OnSpinClicked);
        leaveButton.onClick.AddListener(OnLeaveClicked);
        restartButton.onClick.AddListener(OnRestartClicked);
        quitButton.onClick.AddListener(OnQuitClicked);
    }

    private void OnDisable()
    {
        spinButton.onClick.RemoveListener(OnSpinClicked);
        leaveButton.onClick.RemoveListener(OnLeaveClicked);
        restartButton.onClick.RemoveListener(OnRestartClicked);
        quitButton.onClick.RemoveListener(OnQuitClicked);
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

    public void DebugSpinToBomb()
    {
        if (wheelView.IsSpinning)
            return;

        WheelConfig config = GetConfigForZone();
        for (int i = 0; i < config.Slices.Count; i++)
        {
            if (!config.Slices[i].reward.IsBomb)
                continue;

            spinButton.interactable = false;
            wheelView.DebugSpinToSlice(i, OnSpinFinished);
            UpdateUI();
            return;
        }

        Debug.LogWarning("The current wheel zone has no bomb slice.", this);
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
        if (slice.reward.IsBomb)
        {
            bombPopup.SetActive(true);
            return;
        }

        spinButton.interactable = true;
        model.AddReward(slice);
        Debug.Log("Won: " + slice.reward.RewardName + " x" + slice.amount + " | Zone: " + model.Zone);

        wheelView.Build(GetConfigForZone());
        UpdateUI();
    }

    private void OnRestartClicked()
    {
        bombPopup.SetActive(false);
        spinButton.interactable = true;
        model.Reset();
        wheelView.Build(GetConfigForZone());
        UpdateUI();
    }

    private void OnQuitClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
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
        rewardsView.Show(model.Rewards, model.CanLeave);
    }
}
