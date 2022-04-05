using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TaliyahPottruff.ConversationSystem
{
    [Serializable]
    public class Node
    {
        public int participant;
        [Multiline(3)]
        public string text;
        public Rect position;
        public List<int> next;
        public UnityEvent lineStart, lineEnd;
        public AudioClip typeSound;
        public bool skipLine, hideName;

        public Node() : this("", -1, new Rect(0, 0, 350, 405))
        {

        }

        public Node(string text) : this(text, -1, new Rect(0, 0, 350, 405))
        {

        }

        public Node(string text, int participant) : this(text, participant, new Rect(0, 0, 350, 405))
        {

        }

        public Node(string text, int participant, Rect position)
        {
            this.participant = participant;
            this.text = text;
            this.position = position;
            next = new List<int>();
        }
    }
}