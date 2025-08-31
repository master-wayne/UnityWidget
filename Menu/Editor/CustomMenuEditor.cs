using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using UnityEngine.UI;
using Unity.VisualScripting;
using TMPro;
using UnityEditor.Rendering;

namespace DCG_UI
{
    [CustomEditor(typeof(MenuManager))]
    public class CustomMenuEditor : Editor
    {
        private MenuManager m_menuManager;
        private Canvas m_canvas;
        private GameObject m_menuRoot = null;

        private enum ROW_TYPE
        {
            TOP,
            CHILD_1,
            CHILD_2
        }

        public override void OnInspectorGUI()
        {
            m_menuManager = (MenuManager)target;
            if (m_menuManager != null)
            {
                SetMenuItemCount();

                int x = m_menuManager.m_menuDepth.x;
                int y = m_menuManager.m_menuDepth.y;
                int z = m_menuManager.m_menuDepth.z;

                DrawDefaultInspector();
                GenerateInputFields(x, y, z);

                if (GUILayout.Button("Generate Menu"))
                {
                    GenerateMenu(x, y, z);
                }

                if (GUI.changed)
                {
                    EditorUtility.SetDirty(m_menuManager);
                }
            }
        }
        public void SetMenuItemCount()
        {
            int x = m_menuManager.m_menuDepth.x;
            int y = m_menuManager.m_menuDepth.y;
            int z = m_menuManager.m_menuDepth.z;
            int maxDepth = m_menuManager.m_maxMenuDepth;
            int menuItemCount;

            if (x < 0) x = 0; if (x > maxDepth) x = maxDepth;
            if (y < 0) y = 0; if (y > maxDepth) y = maxDepth;
            if (z < 0) z = 0; if (z > maxDepth) z = maxDepth;

            if (z == 0)
            {
                menuItemCount = x * y;
            }
            if (y == 0)
            {
                menuItemCount = x;
            }
            else
            {
                menuItemCount = x + x * y + x * y * z;
            }

            if (m_menuManager.m_menuItems == null || m_menuManager.m_menuItems.Count != menuItemCount)
            {
                m_menuManager.m_menuItems = new List<MenuItem>();
                for (int i = 0; i < menuItemCount; i++)
                {
                    MenuItem item = new MenuItem();
                    m_menuManager.m_menuItems.Add(item);
                }
            }
            m_menuManager.m_menuDepth.x = x;
            m_menuManager.m_menuDepth.y = y;
            m_menuManager.m_menuDepth.z = z;
        }

        private void GenerateMenu(int x, int y, int z)
        {
            InitMenuData();
            SetCanvas();
            SetMenuRoot();
            SetMenuAlignment();
            PopulateMenu(x, y, z);
        }

        private void InitMenuData()
        {
            foreach (MenuItem item in m_menuManager.m_menuItems)
            {
                item.m_defaultColor = m_menuManager.m_menuItemDefaultColor;
                item.m_onHoverColor = m_menuManager.m_menuItemOnHoverColor;
                item.m_fontSize = m_menuManager.m_fontSize;
                item.m_textColor = m_menuManager.m_textColor;
                item.m_menuTextAlignment = m_menuManager.m_menuTextAlignment;
            }
        }

        private void SetCanvas()
        {
            m_canvas = FindObjectOfType<Canvas>();
            if (m_canvas == null)
            {
                GameObject o = new GameObject("Canvas", typeof(Canvas));
                m_canvas = o.GetComponent<Canvas>();
                m_canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                o.AddComponent<CanvasScaler>();
                o.AddComponent<GraphicRaycaster>();
            }
        }
        private void SetMenuRoot()
        {
            int childCount = m_canvas.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                if (m_canvas.transform.GetChild(i).name == "DCG_Menu")
                {
                    m_menuRoot = m_canvas.transform.GetChild(i).gameObject;
                    GameObject.DestroyImmediate(m_menuRoot);
                    break;
                }
            }
            m_menuRoot = new GameObject("DCG_Menu");
            m_menuRoot.transform.SetParent(m_canvas.transform);
        }

        private void SetMenuAlignment()
        {
            if (m_menuManager.m_menuLocation == MENU_LOCATION.TOP || m_menuManager.m_menuLocation == MENU_LOCATION.BOTTOM)
            {
                HorizontalLayoutGroup hgl = m_menuRoot.AddComponent<HorizontalLayoutGroup>();
                hgl.spacing = m_menuManager.m_menuSpacing;
                hgl.childAlignment = TextAnchor.MiddleLeft;
                hgl.childControlHeight = true;
                hgl.childControlWidth = true;
            }
            if (m_menuManager.m_menuLocation == MENU_LOCATION.LEFT || m_menuManager.m_menuLocation == MENU_LOCATION.RIGHT)
            {
                VerticalLayoutGroup vgl = m_menuRoot.AddComponent<VerticalLayoutGroup>();
                vgl.spacing = m_menuManager.m_menuSpacing;
                vgl.childAlignment = TextAnchor.UpperCenter;
                vgl.childControlHeight = true;
                vgl.childControlWidth = true;
            }

            RectTransform rect = m_menuRoot.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(m_menuManager.m_menuSize.x, m_menuManager.m_menuSize.y);

            switch (m_menuManager.m_menuLocation)
            {
                case MENU_LOCATION.LEFT:
                    rect.anchorMin = new Vector2(0, 0.5f);
                    rect.anchorMax = new Vector2(0, 0.5f);
                    rect.pivot = new Vector2(0, 0.5f);
                    rect.anchoredPosition = new Vector2(10, 0); // 10 units from left
                    break;

                case MENU_LOCATION.RIGHT:
                    rect.anchorMin = new Vector2(1, 0.5f);
                    rect.anchorMax = new Vector2(1, 0.5f);
                    rect.pivot = new Vector2(1, 0.5f);
                    rect.anchoredPosition = new Vector2(-10, 0); // 10 units from right
                    break;

                case MENU_LOCATION.TOP:
                    rect.anchorMin = new Vector2(0.5f, 1);
                    rect.anchorMax = new Vector2(0.5f, 1);
                    rect.pivot = new Vector2(0.5f, 1);
                    rect.anchoredPosition = new Vector2(0, -10); // 10 units from top
                    break;

                case MENU_LOCATION.BOTTOM:
                    rect.anchorMin = new Vector2(0.5f, 0);
                    rect.anchorMax = new Vector2(0.5f, 0);
                    rect.pivot = new Vector2(0.5f, 0);
                    rect.anchoredPosition = new Vector2(0, 10); // 10 units from bottom
                    break;
            }
        }
        private void PopulateMenu(int x, int y, int z)
        {
            int childIndex1 = x;
            int childIndex2 = x + x * y;

            //m_menuRoot.GetComponent<RectTransform>().position = 

            for (int i = 0; i < x; i++)
            {
                if (m_menuManager.m_menuItems[i].m_text != "")
                {
                    Transform m1 = CreateMenuButton(m_menuManager.m_menuItems[i], m_menuRoot.transform, false);

                    for (int j = 0; j < y; j++)
                    {
                        if (m_menuManager.m_menuItems[childIndex1].m_text != "")
                        {
                            Transform m2 = CreateMenuButton(m_menuManager.m_menuItems[childIndex1], m1, false);

                            for (int k = 0; k < z; k++)
                            {
                                if (m_menuManager.m_menuItems[childIndex2].m_text != "")
                                {
                                    CreateMenuButton(m_menuManager.m_menuItems[childIndex2], m2, false);
                                }
                                childIndex2 += 1;
                            }
                        }
                        childIndex1 += 1;
                    }
                }
            }
        }

        private Transform CreateMenuButton(MenuItem item, Transform parent, bool isHidden)
        {
            GameObject btn = new GameObject(item.m_text, typeof(RectTransform), typeof(Button), typeof(Image));
            btn.transform.SetParent(parent.transform);

            Image image = btn.GetComponent<Image>();
            image.color = item.m_defaultColor; // Custom background color

            // Set button size and position
            //RectTransform rect = btn.GetComponent<RectTransform>();
            //rect.sizeDelta = new Vector2(160, 40);
            //rect.anchoredPosition = new Vector2(0, 0);

            // Create TextMeshProUGUI child
            GameObject textGO = new GameObject("TMP_Text", typeof(RectTransform), typeof(TextMeshProUGUI));
            textGO.transform.SetParent(btn.transform, false);

            TextMeshProUGUI tmpText = textGO.GetComponent<TextMeshProUGUI>();
            tmpText.text = item.m_text;
            tmpText.fontSize = item.m_fontSize;
            tmpText.alignment = item.m_menuTextAlignment;
            tmpText.color = item.m_textColor;

            // Stretch text to fill button
            RectTransform textRect = textGO.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = Vector2.zero;
            textRect.offsetMax = Vector2.zero;

            // Add click listener
            //Button button = buttonGO.GetComponent<Button>();
            //button.onClick.AddListener(() => Debug.Log("TMP Button clicked!"));
            return btn.transform;
        }
        private void GenerateInputFields(int x, int y, int z)
        {
            int childIndex1 = x;
            int childIndex2 = x + x * y;

            if (m_menuManager.m_menuItems.Count > 0)
            {
                EditorGUILayout.LabelField("Enter menu contents", EditorStyles.boldLabel);

                for (int i = 0; i < x; i++)
                {
                    CreateRow(m_menuManager.m_menuItems[i], ROW_TYPE.TOP);

                    for (int j = 0; j < y; j++)
                    {
                        CreateRow(m_menuManager.m_menuItems[childIndex1], ROW_TYPE.CHILD_1);
                        childIndex1 += 1;

                        for (int k = 0; k < z; k++)
                        {
                            CreateRow(m_menuManager.m_menuItems[childIndex2], ROW_TYPE.CHILD_2);
                            childIndex2 += 1;
                        }
                    }
                    EditorGUILayout.Separator();
                }
            }
        }
        private void CreateRow(MenuItem item, ROW_TYPE rowType)
        {
            float totalWidth = EditorGUIUtility.currentViewWidth - 40;
            float labelWidth = 100;
            float spacing = 5;
            float remainingWidth = totalWidth - labelWidth - spacing * 3;
            float textWidth = remainingWidth * 0.4f;
            float objectWidth = remainingWidth * 0.3f;
            float dropDownWidth = remainingWidth * 0.3f;

            Rect rowRect = GUILayoutUtility.GetRect(1, EditorGUIUtility.singleLineHeight);
            float x = rowRect.x;
            float y = rowRect.y;

            Rect r1 = new Rect(x, y, 100, rowRect.height);
            GUIStyle labelStyle = new GUIStyle(EditorStyles.label);
            labelStyle.fontSize = 14;
            labelStyle.normal.textColor = Color.cyan;
            labelStyle.alignment = TextAnchor.MiddleLeft;
            EditorGUI.LabelField(r1, GetLabelText(rowType), labelStyle);

            Rect r2 = new Rect(x + labelWidth + spacing, y, textWidth, rowRect.height);
            GUIStyle textFieldStyle = new GUIStyle(EditorStyles.textField);
            textFieldStyle.fontSize = 14;
            textFieldStyle.normal.textColor = new Color(0.9f, 0.9f, 0.9f);
            textFieldStyle.alignment = TextAnchor.MiddleLeft;
            item.m_text = EditorGUI.TextField(r2, item.m_text);

            Rect r3 = new Rect(x + labelWidth + textWidth + spacing * 2, y, objectWidth, rowRect.height);
            item.m_scriptObject = (GameObject)EditorGUI.ObjectField(r3, item.m_scriptObject, typeof(GameObject), true);

            Rect r4 = new Rect(x + labelWidth + textWidth + objectWidth + spacing * 3, y, dropDownWidth, rowRect.height);
            GetFunctionList(item.m_functions, item.m_scriptObject);
            item.m_selectedFunctionIndex = EditorGUI.Popup(r4, item.m_selectedFunctionIndex, item.m_functions.ToArray());
            if (item.m_selectedFunctionIndex >= 0)
            {
                item.m_functionToCall = item.m_functions[item.m_selectedFunctionIndex];
            }
        }
        private void GetFunctionList(List<string> functionList, GameObject obj)
        {
            functionList.Clear();
            functionList.Add("None");

            if (obj != null)
            {
                MonoBehaviour[] attachedScripts = obj.GetComponents<MonoBehaviour>();
                foreach (MonoBehaviour script in attachedScripts)
                {
                    Type type = script.GetType();
                    MethodInfo[] methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);

                    foreach (var method in methods)
                    {
                        if (!method.IsSpecialName)
                        {
                            functionList.Add($"{type.Name}.{method.Name}");
                        }
                    }
                }
            }
        }
        private string GetLabelText(ROW_TYPE rowType)
        {
            string s = string.Empty;
            if (rowType == ROW_TYPE.TOP)
            {
                s = "";
            }
            if (rowType == ROW_TYPE.CHILD_1)
            {
                s = "|----";
            }
            if (rowType == ROW_TYPE.CHILD_2)
            {
                s = "|----------";
            }
            return s + "text";
        }
    }
}
