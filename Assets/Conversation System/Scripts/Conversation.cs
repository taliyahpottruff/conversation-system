using UnityEngine;
using UnityEngine.Events;

namespace TaliyahPottruff.ConversationSystem
{
    public class Conversation : MonoBehaviour
    {
        public Character[] participants;
        public Node entryNode = new("Testing Node");
        public UnityEvent onStart = new();
    }
}