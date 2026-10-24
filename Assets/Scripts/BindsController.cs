using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class BindsController : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;

    private InputAction action;

    public InputAction Action => action;

    public UnityEvent<string> OnButtonChange;

    private void Awake()
    {
        action = inputActions.FindAction("TossButton");
    }

    public void RebindButton()
    {
        action.Disable();
        action.PerformInteractiveRebinding(0)
              .WithCancelingThrough("<Keyboard>/escape")
              .OnComplete(callback =>
              {
                  callback.Dispose();
                  action.Enable();
                  OnButtonChange?.Invoke(action.GetBindingDisplayString(0));
              })
              .OnCancel(callback =>
              {
                  callback.Dispose();
                  action.Enable();
              })
              .Start();
    }
}