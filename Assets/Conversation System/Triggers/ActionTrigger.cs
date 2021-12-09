using System;
using TaliyahPottruff.ConversationSystem.Triggers;
using UnityEngine;
using UnityEngine.InputSystem;

public class ActionTrigger : Trigger
{
    [SerializeField]
    private InputActionAsset inputActionsAsset;
    [SerializeField]
    private string actionMap, action;

    private void Start()
    {
        try
        {
            // Set up event handler to invoke when a button is pressed
            inputActionsAsset.FindActionMap(actionMap).FindAction(action).performed += AreaActionTrigger_performed;
            inputActionsAsset.Enable();
        }
        catch (NullReferenceException e)
        {
            throw new Exception("Conversation System: No input action asset has been set on your area action trigger!");
        }
    }

    private void AreaActionTrigger_performed(InputAction.CallbackContext obj)
    {
        Invoke();
    }
}