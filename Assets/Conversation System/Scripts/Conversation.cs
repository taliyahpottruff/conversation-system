using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ConversationSystem {
    [CreateAssetMenu(fileName = "New Conversation", menuName = "Conversation System/Conversation")]
    public class Conversation : ScriptableObject {
        public Character[] participants;
        public Node entryNode = new Node("Testing Node");
    }
}