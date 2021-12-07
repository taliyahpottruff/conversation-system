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
        public string text;
        public Rect position;
        public List<int> next;
        public UnityEvent lineStart;
        public UnityEvent lineEnd;

        public Node() : this("", -1, new Rect(0, 0, 150, 200))
        {

        }

        public Node(string text) : this(text, -1, new Rect(0, 0, 150, 200))
        {

        }

        public Node(string text, int participant) : this(text, participant, new Rect(0, 0, 150, 200))
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