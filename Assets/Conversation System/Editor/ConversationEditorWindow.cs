using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace TaliyahPottruff.ConversationSystem.Editor
{
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

            _zoomArea = new(0, 0, position.width - 250, position.height);

            OnSelectionChange();
        }

        private void RefreshNodes()
        {
            nodes.Clear();
            connections.Clear();

            if (target != null)
            {
                // Add nodes and connections
                SetupNodesAndConnections(target);
                Debug.Log(nodes.Count);
            }
        }

        private void SetupNodesAndConnections(Conversation target)
        {
            for (int i = 0; i < target.nodes.Count; i++)
            {
                var node = target.nodes[i];
                nodes.Add(node);

                // If any next nodes exist, add connections

            }

            SetupConnections();
        }

        private void SetupConnections()
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                var node = nodes[i];
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

        private const float kZoomMin = 0.1f;
        private const float kZoomMax = 10.0f;

        private Rect _zoomArea = Rect.zero;
        private float _zoom = 1.0f;
        private Vector2 _zoomCoordsOrigin = Vector2.zero;

        private Vector2 ConvertScreenCoordsToZoomCoords(Vector2 screenCoords)
        {
            return (screenCoords - _zoomArea.TopLeft()) / _zoom + _zoomCoordsOrigin;
        }

        private void OnGUI()
        {
            // Sidebar Area
            GUIUtility.ScaleAroundPivot(Vector2.one, Vector2.zero);
            GUILayout.BeginArea(new Rect(position.width - 250, 0, 250, position.height));
            GUILayout.Label(target.name);
            GUILayout.Space(10);
            GUILayout.Label("Participants");
            foreach (var participant in target.participants)
            {
                GUILayout.Label(participant.name);
            }
            GUILayout.EndArea();

            // Node Area
            var area = EditorZoomArea.Begin(_zoom, _zoomArea);
            if (target != null && connections != null && nodes != null)
            {
                HandleEvents();

                // Node Area
                GUILayout.BeginArea(area);
                //DrawNodeCurve(window1, window2); // Here the curve is drawn under the windows
                // Draw curves
                foreach (var connection in connections)
                {
                    DrawNodeCurve(nodes[connection.from].position.Offset(_zoomCoordsOrigin), nodes[connection.to].position.Offset(_zoomCoordsOrigin));
                }

                // Draw windows
                BeginWindows();
                //target.entryNode.position = GUI.Window(1, target.entryNode.position, DrawNodeWindow, target.entryNode.text);   // Updates the Rect's when these are dragged
                for (int i = 0; i < nodes.Count; i++)
                {
                    var node = nodes[i];
                    var previousPosition = node.position.Offset(_zoomCoordsOrigin);
                    node.position = GUI.Window(i, node.position.Offset(_zoomCoordsOrigin), DrawNodeWindow, "").Offset(_zoomCoordsOrigin * -1);
                    if (!node.position.Equals(previousPosition.Offset(_zoomCoordsOrigin * -1)))
                    {
                        EditorUtility.SetDirty(target);
                    }
                }
                EndWindows();
                GUILayout.EndArea();
                EditorZoomArea.End();
            }
        }

        private void HandleEvents()
        {
            // Allow adjusting the zoom with the mouse wheel as well. In this case, use the mouse coordinates
            // as the zoom center instead of the top left corner of the zoom area. This is achieved by
            // maintaining an origin that is used as offset when drawing any GUI elements in the zoom area.
            if (Event.current.type == EventType.ScrollWheel)
            {
                Vector2 screenCoordsMousePos = Event.current.mousePosition;
                Vector2 delta = Event.current.delta;
                Vector2 zoomCoordsMousePos = ConvertScreenCoordsToZoomCoords(screenCoordsMousePos);
                float zoomDelta = -delta.y / 150.0f;
                float oldZoom = _zoom;
                _zoom += zoomDelta;
                _zoom = Mathf.Clamp(_zoom, kZoomMin, kZoomMax);
                _zoomCoordsOrigin += (zoomCoordsMousePos - _zoomCoordsOrigin) - (oldZoom / _zoom) * (zoomCoordsMousePos - _zoomCoordsOrigin);

                Event.current.Use();
            }

            // Allow moving the zoom area's origin by dragging with the middle mouse button or dragging
            // with the left mouse button with Alt pressed.
            if (Event.current.type == EventType.MouseDrag &&
                Event.current.button == 1)
            {
                Vector2 delta = Event.current.delta;
                delta /= _zoom;
                _zoomCoordsOrigin += delta;

                Event.current.Use();
            }
        }

        private void DrawNodeWindow(int id)
        {
            var participant = (nodes[id].participant < 0) ? "Player" : target.participants[nodes[id].participant].name;
            // TODO: Make this not hardcoded
            var lastParticipant = nodes[id].participant;
            nodes[id].participant = EditorGUILayout.Popup(nodes[id].participant + 1, new string[] { "Player", "John Smith" }) - 1;
            if (lastParticipant != nodes[id].participant)
            {
                EditorUtility.SetDirty(target);
            }

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
                        continue;
                    }
                    else if (connection.to == id)
                    {
                        nodes[connection.from].next.Remove(id);
                        connections.Remove(connection);
                        continue;
                    }
                }

                // Decrease any ID references above current
                for (int i = 0; i < nodes.Count; i++)
                {
                    for (int j = 0; j < nodes[i].next.Count; j++)
                    {
                        var next = nodes[i].next[j];

                        if (next == id)
                        {
                            nodes[i].next.Remove(id);
                        }
                        else if (next > id)
                        {
                            nodes[i].next[j]--;
                        }
                    }
                }

                // Remove the node
                nodes.Remove(nodeToDelete);
                target.nodes.Remove(nodeToDelete);

                connections.Clear();
                SetupConnections();
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