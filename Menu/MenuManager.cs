using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UI;

public enum MENU_LOCATION
{
    TOP,
    RIGHT,
    LEFT,
    BOTTOM
}

namespace DCG_UI
{
    [ExecuteAlways]
    public class MenuManager : MonoBehaviour
    {
        [HideInInspector]
        public List<MenuItem> m_menuItems;

        [SerializeField]
        public MENU_LOCATION m_menuLocation;

        [SerializeField]
        public Vector2 m_menuSize;

        [SerializeField]
        public float m_menuSpacing = 1;

        [SerializeField]
        public Color m_menuItemDefaultColor = Color.gray;

        [SerializeField]
        public Color m_menuItemOnHoverColor = Color.blue;

        [SerializeField]
        public int m_fontSize = 14;

        [SerializeField]
        public Color m_textColor = Color.white;

        [SerializeField]
        public TMPro.TextAlignmentOptions m_menuTextAlignment = TMPro.TextAlignmentOptions.Center;

        [HideInInspector]
        public int m_maxMenuDepth = 5;

        [SerializeField]
        public Vector3Int m_menuDepth;
    }
}
