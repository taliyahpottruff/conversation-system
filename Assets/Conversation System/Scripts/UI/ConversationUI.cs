using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TaliyahPottruff.ConversationSystem.UI
{
    public class ConversationUI : MonoBehaviour
    {
        public static Conversation CURRENT_CONVERSATION;

        [Range(0.1f, 50f)]
        public float charactersPerSecond = 25f;

        [SerializeField]
        private TextMeshProUGUI nametag, text;
        [SerializeField]
        private GameObject optionsHolder, optionButton, canvas;
        [SerializeField]
        private InputActionAsset inputActions;
        [SerializeField]
        private AudioSource audioSource;

        private int currentNode;
        private bool typing;

        private void Start()
        {
            Debug.Log("Conversation UI initialized...");

            try
            {
                inputActions.FindActionMap("Conversation").FindAction("Next", true).performed += NextControl_performed;
                inputActions.Enable();
            }
            catch (NullReferenceException)
            {
                Debug.LogError("Conversation System: No InputActionAsset is set in the conversation UI prefab. Please set it!");
            }
        }

        private void NextControl_performed(InputAction.CallbackContext obj)
        {
            if (!typing && CURRENT_CONVERSATION != null && CURRENT_CONVERSATION.nodes[currentNode].next.Count < 2)
            {
                NextLine(0);
            }
        }

        public void Init(Conversation conversation)
        {
            CURRENT_CONVERSATION = conversation;
            currentNode = 0;
            conversation.onStart.Invoke();
            StartCoroutine(Typing_Coroutine(conversation.nodes[currentNode]));
        }

        public void NextLine(int responseNumber)
        {
            // Hide the options if shown
            optionsHolder.SetActive(false);

            if (CURRENT_CONVERSATION.nodes[currentNode].next.Count > 0)
            {
                currentNode = CURRENT_CONVERSATION.nodes[currentNode].next[responseNumber];
                StartCoroutine(Typing_Coroutine(CURRENT_CONVERSATION.nodes[currentNode]));
            }
            else
            {
                // End of conversation
                EndConversation();
            }
        }

        public void EndConversation()
        {
            CURRENT_CONVERSATION.onFinish.Invoke();
            inputActions.FindActionMap("Conversation").FindAction("Next", true).performed -= NextControl_performed;
            CURRENT_CONVERSATION = null;
            Destroy(canvas);
        }

        private IEnumerator Typing_Coroutine(Node toType)
        {
            // Setup
            typing = true;
            nametag.text = (toType.participant >= 0) ? CURRENT_CONVERSATION.participants[toType.participant].name : "Player";
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
                    tmp.text = CURRENT_CONVERSATION.nodes[option].text;
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
                if (CURRENT_CONVERSATION.typeSound != null)
                {
                    audioSource.PlayOneShot(CURRENT_CONVERSATION.typeSound);
                }
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