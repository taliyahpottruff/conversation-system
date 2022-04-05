using TaliyahPottruff.ConversationSystem.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TaliyahPottruff.ConversationSystem.Triggers
{
    public class Trigger : MonoBehaviour
    {
        public float delay;
        public Conversation conversation;
        public bool disabled;

        public void Invoke()
        {
            if (!disabled)
            {
                StartCoroutine(StartConversation_Coroutine());
            }
        }

        private IEnumerator StartConversation_Coroutine()
        {

            yield return new WaitForSeconds(delay);
            if (ConversationUI.CURRENT_CONVERSATION == null)
            {
                disabled = true;
                Debug.Log("Start Conversation...");
                var obj = Instantiate(Resources.Load<GameObject>("Prefabs/Conversation"));
                var ui = obj.GetComponentInChildren<ConversationUI>();
                ui.Init(conversation);
            }
            else
            {
                Debug.LogWarning("Conversation System: Cannot start conversation as one is already active.");
            }
        }
    }
}