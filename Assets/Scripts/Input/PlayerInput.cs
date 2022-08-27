using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[CreateAssetMenu(menuName ="Player Input")]
public class PlayerInput :ScriptableObject,InputActions.IGameplayActions,InputActions.IPauseMenuActions
{
    public event UnityAction<Vector2> onMove=delegate { };
    public event UnityAction onStopMove=delegate { };
    public event UnityAction onFire = delegate { };
    public event UnityAction onStopFire=delegate { };
    public event UnityAction onDodge = delegate { };
    public event UnityAction onOverDrive = delegate { };
    public event UnityAction onPause = delegate { };
    public event UnityAction onUnPause = delegate { };
    InputActions inputActions;
    void OnEnable()
    {
        inputActions = new InputActions();
        inputActions.Gameplay.SetCallbacks(this);
        inputActions.PauseMenu.SetCallbacks(this);
    }
    void SwitchActionMap(InputActionMap actionMap,bool isUIInput)
    {
        inputActions.Disable();
        actionMap.Enable();
        if (isUIInput)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    public void DisableAllInputs()
    {
        
        inputActions.Gameplay. Disable();
        inputActions.PauseMenu.Disable();
    }
    public void SwichToDynamicUpdateMode() => InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInDynamicUpdate;
    public void SwichToFixedUpdateMode() => InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInFixedUpdate;
    public void EnableGameplayInput() => SwitchActionMap(inputActions.Gameplay, false);
    public void EnablePauseMenuInput() => SwitchActionMap(inputActions.PauseMenu, true);
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onMove.Invoke(context.ReadValue<Vector2>());
        }
        if(context.canceled)
        {
            onStopMove.Invoke();
        }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onFire.Invoke();
        }
        if (context.canceled)
        {
            onStopFire.Invoke();
        }
    }

    public void OnDodge(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onDodge.Invoke();
        }
    }

    public void OnOverdrive(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onOverDrive.Invoke();
        }
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onPause.Invoke();
        }
    }

    public void OnUnpause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onUnPause.Invoke();
        }
    }
}
