using UnityEngine;

namespace TaliyahPottruff.ConversationSystem.Triggers
{
    [RequireComponent(typeof(Collider2D))]
    public class EnterTrigger2D : Trigger
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Invoke();
        }
    }
}