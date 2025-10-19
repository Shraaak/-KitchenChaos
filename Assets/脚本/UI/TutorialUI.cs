using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI KeyMoveUpText;
    [SerializeField] private TextMeshProUGUI KeyMoveDownText;
    [SerializeField] private TextMeshProUGUI KeyMoveLeftText;
    [SerializeField] private TextMeshProUGUI KeyMoveRightText;
    [SerializeField] private TextMeshProUGUI KeyMoveInteractText;
    [SerializeField] private TextMeshProUGUI KeyMoveInteractAlternateText;
    [SerializeField] private TextMeshProUGUI KeyMovePauseText;

    void Start()
    {
        InputGame.Instance.OnBindingRebind += InputGame_OnBindingRebind;
        KitchenGameManager.Instance.OnStateChanged += KitchenGameManager_OnStateChanged;

        UpdateVisual();

        Show();
    }


    private void UpdateVisual()
    {
        KeyMoveUpText.text = InputGame.Instance.GetBindingText(InputGame.Binding.Move_Up);
        KeyMoveDownText.text = InputGame.Instance.GetBindingText(InputGame.Binding.Move_Down);
        KeyMoveLeftText.text = InputGame.Instance.GetBindingText(InputGame.Binding.Move_Left);
        KeyMoveRightText.text = InputGame.Instance.GetBindingText(InputGame.Binding.Move_Right);
        KeyMoveInteractText.text = InputGame.Instance.GetBindingText(InputGame.Binding.Interact);
        KeyMoveInteractAlternateText.text = InputGame.Instance.GetBindingText(InputGame.Binding.InteractAlternate);
        KeyMovePauseText.text = InputGame.Instance.GetBindingText(InputGame.Binding.Pause);
    }

    private void InputGame_OnBindingRebind(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }

    private void KitchenGameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (KitchenGameManager.Instance.IsCountdownToStartActive())
        {
            Hide();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
