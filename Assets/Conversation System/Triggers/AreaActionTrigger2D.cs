using System;
using TaliyahPottruff.ConversationSystem.Triggers;
using UnityEngine;
using UnityEngine.InputSystem;

public class AreaActionTrigger2D : Trigger
{
    [SerializeField]
    private bool playerIn;
    [SerializeField]
    private string needsTag;
    [SerializeField]
    private InputActionAsset inputActionsAsset;
    [SerializeField]
    private string actionMap, action;

    private void Start()
    {
        try
        {
            inputActionsAsset.FindAction($"{actionMap}/{action}").performed += AreaActionTrigger_performed;
            inputActionsAsset.Enable();
        }
        catch (NullReferenceException e)
        {
            throw new Exception("Conversation System: No input action asset has been set on your area action trigger!");
        }
    }

    private void AreaActionTrigger_performed(InputAction.CallbackContext obj)
    {
        if (playerIn)
        {
            Invoke();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!needsTag.Equals(""))
        {
            if (collision.tag.Equals(needsTag))
            {
                playerIn = true;
            }
        }
        else
        {
            playerIn = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!needsTag.Equals(""))
        {
            if (collision.tag.Equals(needsTag))
            {
                playerIn = false;
            }
        }
        else
        {
            playerIn = false;
        }
    }
}