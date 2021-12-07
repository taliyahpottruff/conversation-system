using System.Collections;
using TMPro;
using UnityEngine;

namespace TaliyahPottruff.ConversationSystem.UI
{
    public class ConversationUI : MonoBehaviour
    {
        private Conversation m_conversation;
        public Conversation Conversation { get => m_conversation; }

        [Range(0.1f, 50f)]
        public float charactersPerSecond = 25f;

        [SerializeField]
        private TextMeshProUGUI nametag, text;
        [SerializeField]
        private GameObject optionsHolder, optionButton, canvas;

        private Node currentNode;
        private bool typing;

        private void Start()
        {
            Debug.Log("Conversation UI initialized...");
        }

        public void Init(Conversation conversation)
        {
            this.m_conversation = conversation;
            currentNode = conversation.entryNode;
            conversation.onStart.Invoke();
            StartCoroutine(Typing_Coroutine(currentNode));
        }

        private void Update()
        {
            // TODO: Temporary, please use new input system
            if (Input.GetKeyDown(KeyCode.Space) && !typing && m_conversation != null && currentNode != null)
            {
                // Hide the options if shown
                optionsHolder.SetActive(false);

                // TODO: Only do this if 1 or 0 options
                if (currentNode.next.Count > 0)
                {
                    currentNode = currentNode.next[0];
                    StartCoroutine(Typing_Coroutine(currentNode));
                }
                else
                {
                    // End of conversation
                    Destroy(canvas);
                }
            }
        }

        private IEnumerator Typing_Coroutine(Node toType)
        {
            // Setup
            typing = true;
            nametag.text = toType.participant.ToString(); // TODO: This is just an ID right now, needs to be a name
            text.text = "";
            toType.lineStart.Invoke();

            // Show options if multiple branches exist
            if (toType.next.Count > 1)
            {
                optionsHolder.SetActive(true);
                for (int i = 0; i < optionsHolder.transform.childCount; i++)
                {
                    var child = optionsHolder.transform.GetChild(i);
                    Destroy(child.gameObject);
                }
                foreach (var option in toType.next)
                {
                    var obj = Instantiate<GameObject>(optionButton, optionsHolder.transform);
                    var tmp = obj.GetComponentInChildren<TextMeshProUGUI>();
                    tmp.text = option.text;
                }
            }

            // Typing loop
            var characters = toType.text.Length;
            for (int i = 0; i < characters; i++)
            {
                text.text = toType.text.Substring(0, i + 1);
                yield return new WaitForSeconds(1f / charactersPerSecond);
            }

            // When finished
            typing = false;
            toType.lineEnd.Invoke();
        }
    }
}