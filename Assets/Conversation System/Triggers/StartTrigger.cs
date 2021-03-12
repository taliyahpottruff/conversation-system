using ConversationSystem.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ConversationSystem.Triggers {
    public class StartTrigger : MonoBehaviour {
        public float delay;

        private ConversationUI conversationBox;

        private void Start() {
            StartCoroutine(StartConversation_Coroutine());
        }

        private IEnumerator StartConversation_Coroutine() {
            yield return new WaitForSeconds(delay);
            Debug.Log("Start Conversation...");
            Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Conversation"));
        }
    }
}