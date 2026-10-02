using UnityEngine;
using UnityEngine.InputSystem;

public class BindsController : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;
    [SerializeField] private UIController canvas;

    private InputAction action;

    public InputAction Action => action;

    private void Awake()
    {
        action = inputActions.FindAction("TossButton");
    }

    public void RebindButton()
    {
        Debug.Log("Нажмите кнопку, на которую хотите назначить действие");
        action.Disable();
        action.PerformInteractiveRebinding(0)
              .WithCancelingThrough("<Keyboard>/escape")
              .OnComplete(callback =>
              {
                  callback.Dispose();
                  action.Enable();
                  canvas.UpdateBindedButtonText($"{action.GetBindingDisplayString(0)}");
              })
              .OnCancel(callback =>
              {
                  callback.Dispose();
                  action.Enable();
                  Debug.Log("Назначение отменено");
              })
              .Start();
    }
}