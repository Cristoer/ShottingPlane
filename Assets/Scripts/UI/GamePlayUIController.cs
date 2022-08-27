using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayUIController : MonoBehaviour
{
    [SerializeField] PlayerInput playerInput;
    [SerializeField] Canvas hUDCanvas;
    [SerializeField] Canvas menuCanvas;
    private void OnEnable()
    {
        playerInput.onPause += Pause;
        playerInput.onUnPause += Unpause;
    }
    private void OnDisable()
    {
        playerInput.onPause -= Pause;
        playerInput.onUnPause -= Unpause;
    }
    void Pause()
    {
        Time.timeScale = 0f;
        hUDCanvas.enabled = false;
        menuCanvas.enabled = true;
        playerInput.EnablePauseMenuInput();
        playerInput.SwichToDynamicUpdateMode();
    }
    void Unpause()
    {
        Time.timeScale = 1f;
        hUDCanvas.enabled = true;
        menuCanvas.enabled = false;
        playerInput.EnableGameplayInput();
        playerInput.SwichToFixedUpdateMode();
    }
}
