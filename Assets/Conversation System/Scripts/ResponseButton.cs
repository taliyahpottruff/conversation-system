using TaliyahPottruff.ConversationSystem.UI;
using UnityEngine;

namespace TaliyahPottruff.ConversationSystem
{
    public class ResponseButton : MonoBehaviour
    {
        public int responseNumber;

        private ConversationUI ui;

        public void Init(ConversationUI ui)
        {
            this.ui = ui;
        }

        public void ChooseResponse()
        {
            if (ui != null)
            {
                ui.NextLine(responseNumber);
            }
        }
    }
}