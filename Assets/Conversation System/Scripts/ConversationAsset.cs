using UnityEngine;

namespace TaliyahPottruff.ConversationSystem
{
    [CreateAssetMenu(fileName = "New Conversation", menuName = "Conversation System/Conversation")]
    public class ConversationAsset : ScriptableObject
    {
        public Character[] participants;
        public Node entryNode = new Node("Testing Node");
    }
}