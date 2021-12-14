using UnityEngine;

namespace TaliyahPottruff.ConversationSystem.Triggers
{
    public class EnterTrigger : Trigger
    {
        private void OnTriggerEnter(Collider collision)
        {
            Invoke();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Invoke();
        }
    }
}