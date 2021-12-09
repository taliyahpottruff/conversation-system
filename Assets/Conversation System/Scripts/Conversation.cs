using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TaliyahPottruff.ConversationSystem
{
    public class Conversation : MonoBehaviour
    {
        public Character[] participants;
        public List<Node> nodes = new() { new Node("") };
        public UnityEvent onStart = new();
        public UnityEvent onFinish = new();
    }
}