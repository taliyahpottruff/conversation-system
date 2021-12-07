using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace TaliyahPottruff.ConversationSystem.Editor {
    public class ConversationEditorWindow : EditorWindow
    {
        Conversation target;

        List<Node> nodes = new List<Node>();
        List<ConversationEditorConnection> connections = new List<ConversationEditorConnection>();

        [MenuItem("Window/Conversation Editor")]
        static void ShowWindow()
        {
            var window = (ConversationEditorWindow)EditorWindow.GetWindow(typeof(ConversationEditorWindow));
            window.Init();
            window.Show();
        }

        public void Init()
        {
            titleContent = new GUIContent("Conversation Editor");

            OnSelectionChange();
        }

        private void RefreshNodes()
        {
            nodes.Clear();
            connections.Clear();

            if (target != null)
            {
                // Add nodes and connections
                SetupConnections(target);
                Debug.Log(nodes.Count);
            }
        }

        private void SetupConnections(Conversation target)
        {
            for (int i = 0; i < target.nodes.Count; i++)
            {
                var node = target.nodes[i];
                nodes.Add(node);

                // If any next nodes exist, add connections
                foreach (var child in node.next)
                {
                    connections.Add(new ConversationEditorConnection(i, child));
                }
            }
        }

        private void OnSelectionChange()
        {
            var selected = Selection.activeObject;
            target = null;
            if (selected != null)
            {
                Debug.Log(selected.GetType());
                if (selected.GetType() == typeof(GameObject))
                {
                    var go = selected as GameObject;
                    var conversation = go.GetComponent<Conversation>();

                    if (conversation != null)
                    {
                        Debug.Log(conversation.nodes.Count);
                        target = conversation;
                    }
                }
            }
            RefreshNodes();
        }

        private void OnGUI()
        {
            if (target != null && connections != null && nodes != null)
            {
                // Node Area
                GUILayout.BeginArea(new Rect(0, 0, position.width - 250, position.height));
                //DrawNodeCurve(window1, window2); // Here the curve is drawn under the windows
                // Draw curves
                foreach (var connection in connections)
                {
                    DrawNodeCurve(nodes[connection.from].position, nodes[connection.to].position);
                }

                // Draw windows
                BeginWindows();
                //target.entryNode.position = GUI.Window(1, target.entryNode.position, DrawNodeWindow, target.entryNode.text);   // Updates the Rect's when these are dragged
                for (int i = 0; i < nodes.Count; i++)
                {
                    var node = nodes[i];
                    var previousPosition = node.position;
                    node.position = GUI.Window(i, node.position, DrawNodeWindow, "");
                    if (!node.position.Equals(previousPosition))
                    {
                        EditorUtility.SetDirty(target);
                    }
                }
                EndWindows();
                GUILayout.EndArea();

                // Sidebar Area
                GUILayout.BeginArea(new Rect(position.width - 250, 0, 250, position.height));
                GUILayout.Label(target.name);
                GUILayout.Space(10);
                GUILayout.Label("Participants");
                foreach (var participant in target.participants)
                {
                    GUILayout.Label(participant.name);
                }
                GUILayout.EndArea();
            }
        }

        private void DrawNodeWindow(int id)
        {
            var participant = (nodes[id].participant < 0) ? "Player" : target.participants[nodes[id].participant].name;
            // TODO: Make this not hardcoded
            EditorGUILayout.Popup(0, new string[] { "Player", "John Smith" });

            var previousText = nodes[id].text;
            nodes[id].text = GUILayout.TextArea(nodes[id].text);
            if (!previousText.Equals(nodes[id].text))
            {
                EditorUtility.SetDirty(target);
            }

            if (GUILayout.Button("New Child"))
            {
                Debug.Log("Creating a new child node...");
                var newPosition = nodes[id].position;
                newPosition.x += newPosition.width * 2;
                var newNode = new Node("New Node", -1, newPosition);
                target.nodes.Add(newNode);
                var newID = target.nodes.Count - 1;
                nodes[id].next.Add(newID);
                nodes.Add(newNode);
                connections.Add(new ConversationEditorConnection(id, newID));
                EditorUtility.SetDirty(target);
            }

            if (GUILayout.Button("Delete Node"))
            {
                var nodeToDelete = nodes[id];

                // Remove all the connections associated with the node
                var connectionsToBeDeleted = connections.FindAll(delegate (ConversationEditorConnection conn) { return conn.from == id || conn.to == id; });
                foreach (var connection in connectionsToBeDeleted)
                {
                    if (connection.from == id)
                    {
                        connections.Remove(connection);
                    }
                    else if (connection.to == id)
                    {
                        nodes[connection.from].next.Remove(id);
                        connections.Remove(connection);
                    }
                }

                // Remove the node
                nodes.Remove(nodeToDelete);
                target.nodes.Remove(nodeToDelete);

                EditorUtility.SetDirty(target);
            }

            SerializedObject serializedObject = new SerializedObject(target);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("nodes").GetArrayElementAtIndex(id).FindPropertyRelative("lineStart"));
            serializedObject.ApplyModifiedProperties();

            GUI.DragWindow();
        }

        private void DrawNodeCurve(Rect start, Rect end)
        {
            Vector3 startPos = new Vector3(start.x + start.width, start.y + start.height / 2, 0);
            Vector3 endPos = new Vector3(end.x, end.y + end.height / 2, 0);
            Vector3 startTan = startPos + Vector3.right * 50;
            Vector3 endTan = endPos + Vector3.left * 50;
            Color shadowCol = new Color(0, 0, 0, 0.06f);
            for (int i = 0; i < 3; i++) // Draw a shadow
                Handles.DrawBezier(startPos, endPos, startTan, endTan, shadowCol, null, (i + 1) * 5);
            Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.white, null, 1);
        }
    }
    
    public struct ConversationEditorConnection
    {
        public int from, to;

        public ConversationEditorConnection(int from, int to)
        {
            this.from = from;
            this.to = to;
        }
    }
}