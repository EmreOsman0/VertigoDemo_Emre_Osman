using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] private WheelView wheelView;
    [SerializeField] private Button spinButton;

    private GameModel model = new GameModel();

#if UNITY_EDITOR
    private void OnValidate()
    {
        Transform spinButtonTransform = transform.Find("ui_button_spin");
        if (spinButtonTransform != null)
            spinButton = spinButtonTransform.GetComponent<Button>();
    }
#endif

    private void OnEnable()
    {
        spinButton.onClick.AddListener(OnSpinClicked);
    }

    private void OnDisable()
    {
        spinButton.onClick.RemoveListener(OnSpinClicked);
    }

    private void OnSpinClicked()
    {
        spinButton.interactable = false;
        wheelView.Spin(OnSpinFinished);
    }

    private void OnSpinFinished(WheelSlice slice)
    {
        spinButton.interactable = true;

        if (slice.reward.IsBomb)
        {
            Debug.Log("Bomb! Lost everything.");
            model.Reset();
            return;
        }

        model.AddReward(slice);
        Debug.Log("Won: " + slice.reward.RewardName + " x" + slice.amount + " | Zone: " + model.Zone);
    }
}
