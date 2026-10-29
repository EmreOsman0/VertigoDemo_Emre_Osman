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
    [SerializeField] private GameObject rewardsPopup;
    [SerializeField] private RewardsView popupRewardsView;
    [SerializeField] private Button playAgainButton;
    [SerializeField] private Button rewardsQuitButton;
    [SerializeField] private ZoneWheelTable zoneWheelTable;

    private GameModel model = new GameModel();
    private int multiplier = 1;

#if UNITY_EDITOR
    private void OnValidate()
    {
        spinButton = FindChild<Button>("ui_wheel_buttons/ui_button_spin");
        leaveButton = FindChild<Button>("ui_wheel_buttons/ui_button_leave");
        zoneText = FindChild<TextMeshProUGUI>("ui_text_zone_value");
        rewardsView = FindChild<RewardsView>("ui_rewards");
        bombPopup = FindObject("ui_popups/ui_popup_bomb");
        restartButton = FindChild<Button>("ui_popups/ui_popup_bomb/ui_button_popup_bomb_restart");
        quitButton = FindChild<Button>("ui_popups/ui_popup_bomb/ui_button_popup_bomb_quit");
        rewardsPopup = FindObject("ui_popups/ui_popup_reward");
        popupRewardsView = FindChild<RewardsView>("ui_popups/ui_popup_reward/ui_popup_rewards");
        playAgainButton = FindChild<Button>("ui_popups/ui_popup_reward/ui_button_popup_rewards_play_again");
        rewardsQuitButton = FindChild<Button>("ui_popups/ui_popup_reward/ui_button_popup_rewards_quit");
    }

    private T FindChild<T>(string path) where T : Component
    {
        Transform child = transform.Find(path);
        return child != null ? child.GetComponent<T>() : null;
    }

    private GameObject FindObject(string path)
    {
        Transform child = transform.Find(path);
        return child != null ? child.gameObject : null;
    }
#endif

    private void OnEnable()
    {
        spinButton.onClick.AddListener(OnSpinClicked);
        leaveButton.onClick.AddListener(OnLeaveClicked);
        restartButton.onClick.AddListener(OnRestartClicked);
        quitButton.onClick.AddListener(OnQuitClicked);
        playAgainButton.onClick.AddListener(OnPlayAgainClicked);
        rewardsQuitButton.onClick.AddListener(OnQuitClicked);
    }

    private void OnDisable()
    {
        spinButton.onClick.RemoveListener(OnSpinClicked);
        leaveButton.onClick.RemoveListener(OnLeaveClicked);
        restartButton.onClick.RemoveListener(OnRestartClicked);
        quitButton.onClick.RemoveListener(OnQuitClicked);
        playAgainButton.onClick.RemoveListener(OnPlayAgainClicked);
        rewardsQuitButton.onClick.RemoveListener(OnQuitClicked);
    }

    private void Start()
    {
        BuildWheel();
        UpdateUI();
    }

    private void BuildWheel()
    {
        WheelConfig wheel = zoneWheelTable.GetWheel(model.Zone, out multiplier);
        wheelView.Build(wheel, multiplier);
    }

#if UNITY_EDITOR
    public void DebugSetZone(int zone)
    {
        if (wheelView.IsSpinning) return;
        model.SetZone(zone);
        BuildWheel();
        UpdateUI();
    }

    public void DebugSpinToBomb()
    {
        if (wheelView.IsSpinning)
            return;

        int bombIndex = wheelView.CurrentConfig.IndexOfBomb();
        if (bombIndex < 0)
        {
            Debug.LogWarning("The current wheel zone has no bomb slice.", this);
            return;
        }

        spinButton.interactable = false;
        wheelView.DebugSpinToSlice(bombIndex, OnSpinFinished);
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
        if (slice.reward.IsBomb)
        {
            bombPopup.SetActive(true);
            return;
        }

        spinButton.interactable = true;
        model.AddReward(slice, multiplier);

        BuildWheel();
        UpdateUI();
    }

    private void OnRestartClicked()
    {
        bombPopup.SetActive(false);
        StartNewRun();
    }

    private void OnPlayAgainClicked()
    {
        rewardsPopup.SetActive(false);
        StartNewRun();
    }

    private void StartNewRun()
    {
        spinButton.interactable = true;
        model.Reset();
        BuildWheel();
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
        spinButton.interactable = false;
        leaveButton.interactable = false;
        popupRewardsView.Show(model.Rewards, true);
        rewardsPopup.SetActive(true);
    }

    private void UpdateUI()
    {
        leaveButton.interactable = model.CanLeave && !wheelView.IsSpinning;
        zoneText.text = "Zone " + model.Zone;
        rewardsView.Show(model.Rewards, model.CanLeave);
    }
}
