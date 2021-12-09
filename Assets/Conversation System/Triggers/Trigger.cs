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

        protected void Invoke()
        {
            StartCoroutine(StartConversation_Coroutine());
        }

        private IEnumerator StartConversation_Coroutine()
        {
            yield return new WaitForSeconds(delay);
            Debug.Log("Start Conversation...");
            var obj = Instantiate(Resources.Load<GameObject>("Prefabs/Conversation"));
            var ui = obj.GetComponentInChildren<ConversationUI>();
            ui.Init(conversation);
        }
    }
}