using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TaliyahPottruff.ConversationSystem
{
    [CreateAssetMenu(fileName = "New Character", menuName = "Conversation System/Character")]
    public class Character : ScriptableObject
    {
        public new string name = "Name";
    }
}