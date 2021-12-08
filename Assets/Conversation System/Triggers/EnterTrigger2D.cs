using System.Collections;
using TaliyahPottruff.ConversationSystem.UI;
using UnityEngine;

namespace TaliyahPottruff.ConversationSystem.Triggers
{
    [RequireComponent(typeof(Collider2D))]
    public class EnterTrigger2D : MonoBehaviour
    {
        public float delay;
        public Conversation conversation;

        private void OnTriggerEnter2D(Collider2D collision)
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