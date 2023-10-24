using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Haptics;
using System;

public class HapticItem : MonoBehaviour
{

    public HapticClip clip1;
 
    HapticClipPlayer _playerLeft1;
    HapticClipPlayer _playerRight1;
    
    public bool activateInteractable;

    protected virtual void Start()
    {
        // We create two haptic clip players for each hand.
        _playerLeft1 = new HapticClipPlayer(clip1);
        _playerRight1 = new HapticClipPlayer(clip1);
        
    }

    public void ActiveSelect()
    {
        activateInteractable = true;
        Debug.Log("activated");
        
    }

    public void ActiveDeselect()
    {
        activateInteractable = false;
        Debug.Log("deactivated");
    }
    
    String GetControllerName(OVRInput.Controller controller)
    {
        if (controller == OVRInput.Controller.LTouch)
        {
            return "left controller";
        }
        else if (controller == OVRInput.Controller.RTouch)
        {
            return "right controller";
        }

        return "unknown controller";
    }

    // This section provides a series of interactions that showcase the playback and modulation capabilities of the
    // Haptics SDK.
    void HandleControllerInput(OVRInput.Controller controller, HapticClipPlayer clipPlayer1, Controller hand)
    {
        string controllerName = GetControllerName(controller);

        try
        {
            // Play first clip with default priority using the index trigger
            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, controller) && activateInteractable == true)
            {
                clipPlayer1.Play(hand);
                Debug.Log("Should feel vibration from clipPlayer1 on " + controllerName + ".");
            }

            // Stop first clip when releasing the index trigger
            if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger, controller) && activateInteractable == false)
            {
                clipPlayer1.Stop();
                Debug.Log("Vibration from clipPlayer1 on " + controllerName + " should stop.");
            }

            
        }

        // If any exceptions occur, we catch and log them here.
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    // We poll for controller interactions on every frame using the Update() loop
    protected virtual void Update()
    {
        HandleControllerInput(OVRInput.Controller.LTouch, _playerLeft1, Controller.Left);
        HandleControllerInput(OVRInput.Controller.RTouch, _playerRight1, Controller.Right);
    }

    
    

    protected virtual void OnDestroy()
    {
        _playerLeft1.Dispose();
        _playerRight1.Dispose();
        
    }

    // Upon exiting the application (or when playmode is stopped) we release the haptic clip players and uninitialize (dispose) the SDK.
    protected virtual void OnApplicationQuit()
    {
        Haptics.Instance.Dispose();
    }


}

