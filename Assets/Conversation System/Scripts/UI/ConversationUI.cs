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

        private int currentNode;
        private bool typing;

        private void Start()
        {
            Debug.Log("Conversation UI initialized...");
        }

        public void Init(Conversation conversation)
        {
            this.m_conversation = conversation;
            currentNode = 0;
            conversation.onStart.Invoke();
            StartCoroutine(Typing_Coroutine(conversation.nodes[currentNode]));
        }

        private void Update()
        {
            // TODO: Temporary, please use new input system
            if (Input.GetKeyDown(KeyCode.Space) && !typing && m_conversation != null && m_conversation.nodes[currentNode].next.Count < 2)
            {
                NextLine(0);
            }
        }

        public void NextLine(int responseNumber)
        {
            // Hide the options if shown
            optionsHolder.SetActive(false);

            if (m_conversation.nodes[currentNode].next.Count > 0)
            {
                currentNode = m_conversation.nodes[currentNode].next[responseNumber];
                StartCoroutine(Typing_Coroutine(m_conversation.nodes[currentNode]));
            }
            else
            {
                // End of conversation
                Destroy(canvas);
            }
        }

        private IEnumerator Typing_Coroutine(Node toType)
        {
            // Setup
            typing = true;
            nametag.text = (toType.participant >= 0) ? m_conversation.participants[toType.participant].name : "Player"; // TODO: This is just an ID right now, needs to be a name
            text.text = "";
            toType.lineStart.Invoke();

            // Show options if multiple branches exist
            if (toType.next.Count > 1)
            {
                // Clear previous options
                for (int i = 0; i < optionsHolder.transform.childCount; i++)
                {
                    var child = optionsHolder.transform.GetChild(i);
                    Destroy(child.gameObject);
                }
                // Show options
                for (int i = 0; i < toType.next.Count; i++)
                {
                    var option = toType.next[i];
                    var obj = Instantiate<GameObject>(optionButton, optionsHolder.transform);
                    var tmp = obj.GetComponentInChildren<TextMeshProUGUI>();
                    tmp.text = m_conversation.nodes[option].text;
                    var rb = obj.GetComponentInChildren<ResponseButton>();
                    rb.Init(this);
                    rb.responseNumber = i;
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
            if (toType.next.Count > 1)
            {
                optionsHolder.SetActive(true);
            }
            typing = false;
            toType.lineEnd.Invoke();
        }
    }
}