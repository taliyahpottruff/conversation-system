using TMPro;
using UnityEngine;

namespace ConversationSystem.UI {
    public class ConversationUI : MonoBehaviour {
        [SerializeField]
        private TextMeshProUGUI nametag, text;

        private void Start() {
            Debug.Log("Conversation started!");
        }
    }
}