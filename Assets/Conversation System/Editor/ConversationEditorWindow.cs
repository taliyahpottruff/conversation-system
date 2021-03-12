using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ConversationSystem.Editor {
    public class ConversationEditorWindow : EditorWindow {
        Conversation target;

        List<Node> nodes;
        List<ConversationEditorConnection> connections;

        [MenuItem("Window/Conversation Editor")]
        static void ShowWindow() {
            var window = (ConversationEditorWindow)EditorWindow.GetWindow(typeof(ConversationEditorWindow));
            window.Init();
            window.Show();
        }

        public void Init() {
            titleContent = new GUIContent("Conversation Editor");

            nodes = new List<Node>();
            connections = new List<ConversationEditorConnection>();

            OnSelectionChange();
        }

        private void RefreshNodes() {
            nodes.Clear();
            connections.Clear();

            // Add some testing data
            target.entryNode.next.Add(new Node("Text 2", 0, new Rect(250, 250, 100, 100)));

            // Add to editor
            nodes.Add(target.entryNode);
            nodes.Add(target.entryNode.next[0]);

            connections.Add(new ConversationEditorConnection(nodes[0], nodes[1]));
        }

        private void OnSelectionChange() {
            var selected = Selection.activeObject;
            if (selected.GetType() != typeof(DefaultAsset)) {
                Debug.Log(selected.GetType());
                target = selected as Conversation;
                RefreshNodes();
            }
        }

        private void OnGUI() {
            if (target != null) {
                // Node Area
                GUILayout.BeginArea(new Rect(0, 0, position.width - 250, position.height));
                //DrawNodeCurve(window1, window2); // Here the curve is drawn under the windows
                // Draw curves
                foreach (var connection in connections) {
                    DrawNodeCurve(connection.from.position, connection.to.position);
                }

                // Draw windows
                BeginWindows();
                //target.entryNode.position = GUI.Window(1, target.entryNode.position, DrawNodeWindow, target.entryNode.text);   // Updates the Rect's when these are dragged
                for (int i = 0; i < nodes.Count; i++) {
                    var node = nodes[i];
                    node.position = GUI.Window(i, node.position, DrawNodeWindow, node.text);
                }
                EndWindows();
                GUILayout.EndArea();

                // Sidebar Area
                GUILayout.BeginArea(new Rect(position.width - 250, 0, 250, position.height));
                GUILayout.Label(target.name);
                GUILayout.Button("Do Nothing");
                GUILayout.EndArea();
            }
        }

        private void DrawNodeWindow(int id) {
            GUI.DragWindow();
        }

        private void DrawNodeCurve(Rect start, Rect end) {
            Vector3 startPos = new Vector3(start.x + start.width, start.y + start.height / 2, 0);
            Vector3 endPos = new Vector3(end.x, end.y + end.height / 2, 0);
            Vector3 startTan = startPos + Vector3.right * 50;
            Vector3 endTan = endPos + Vector3.left * 50;
            Color shadowCol = new Color(0, 0, 0, 0.06f);
            for (int i = 0; i < 3; i++) // Draw a shadow
                Handles.DrawBezier(startPos, endPos, startTan, endTan, shadowCol, null, (i + 1) * 5);
            Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.black, null, 1);
        }
    }
    
    public struct ConversationEditorConnection {
        public Node from, to;

        public ConversationEditorConnection(Node from, Node to) {
            this.from = from;
            this.to = to;
        }
    }
}