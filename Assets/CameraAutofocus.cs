using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class CameraAutofocus : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var Vuforia = VuforiaARController.Instance;
        Vuforia.RegisterVuforiaStartedCallback(OnVuforiaStarted);
        Vuforia.RegisterOnPauseCallback(OnPaused);
    }
    private void OnVuforiaStarted()
    {
        CameraDevice.Instance.SetFocusMode(
            CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
    }

    private void OnPaused(bool paused)
    {
        if (!paused) // resumed
        {
            // Set again autofocus mode when app is resumed
            CameraDevice.Instance.SetFocusMode(
                CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
        }
    }
}
