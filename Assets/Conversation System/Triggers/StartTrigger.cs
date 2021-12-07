using TaliyahPottruff.ConversationSystem.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TaliyahPottruff.ConversationSystem.Triggers
{
    public class StartTrigger : MonoBehaviour
    {
        public float delay;
        public Conversation conversation;

        private ConversationUI conversationBox;

        private void Start()
        {
            StartCoroutine(StartConversation_Coroutine());
        }

        private IEnumerator StartConversation_Coroutine()
        {
            yield return new WaitForSeconds(delay);
            Debug.Log("Start Conversation...");
            var obj = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Conversation"));
            var ui = obj.GetComponentInChildren<ConversationUI>();
            ui.Init(conversation);
        }
    }
}