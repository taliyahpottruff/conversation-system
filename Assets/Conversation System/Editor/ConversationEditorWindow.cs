using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ConversationSystem.Editor {
    public class ConversationEditorWindow : EditorWindow {
        Conversation target;

        Rect window1;
        Rect window2;

        [MenuItem("Window/Conversation Editor")]
        static void ShowWindow() {
            var window = (ConversationEditorWindow)EditorWindow.GetWindow(typeof(ConversationEditorWindow));
            window.Init();
            window.Show();
        }

        public void Init() {
            titleContent = new GUIContent("Conversation Editor");

            window1 = new Rect(10, 10, 100, 100);
            window2 = new Rect(210, 210, 100, 100);
        }

        private void OnSelectionChange() {
            var selected = Selection.activeObject;
            if (selected.GetType() != typeof(DefaultAsset)) {
                Debug.Log(selected.GetType());
                target = selected as Conversation;
            }
        }

        private void OnGUI() {
            if (target != null) {
                // Node Area
                GUILayout.BeginArea(new Rect(0, 0, position.width - 250, position.height));
                DrawNodeCurve(window1, window2); // Here the curve is drawn under the windows

                BeginWindows();
                window1 = GUI.Window(1, window1, DrawNodeWindow, "Window 1");   // Updates the Rect's when these are dragged
                window2 = GUI.Window(2, window2, DrawNodeWindow, "Window 2");
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
}