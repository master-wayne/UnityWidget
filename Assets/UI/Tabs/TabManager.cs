using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DCG_UI
{
    public class TabManager : MonoBehaviour
    {
        [SerializeField]
        public float m_UIWidth = 500;

        [SerializeField]
        public float m_UIHeight = 70;

        [SerializeField]
        public ANCHOR_POSITION m_AnchorPosition = ANCHOR_POSITION.MIDDLE_RIGHT;

        [SerializeField]
        public Canvas m_canvas;

        [SerializeField]
        public Color m_tabIdleColor, m_tabHoverColor, m_tabActiveColor;
        public LayerMask m_UILayer;

        private Tab m_activeTab;
        private Tab m_highlightedTab;

        private TabArea m_activeTabArea;

        [SerializeField]
        public List<GameObject> m_tabHeaders;

        private List<Tab> m_tabs;

        [SerializeField]
        public List<GameObject> m_tabAreas;
        private List<TabArea> m_areas;

        private RectTransform m_rectTransform;
        private GraphicRaycaster m_graphicRaycaster;

        private delegate void TabEnter(Tab tab);
        private delegate void TabExit(Tab tab);
        private delegate void TabSelect(Tab tab);

        private event TabEnter OnTabEnter;
        private event TabExit OnTabExit;
        private event TabSelect OnTabSelect;

        void OnEnable()
        {
            OnTabEnter += Enter;
            OnTabExit += Exit;
            OnTabSelect += Select;
        }

        void OnDisable()
        {
            OnTabEnter -= Enter;
            OnTabExit -= Exit;
            OnTabSelect -= Select;
        }

        void Start()
        {
            PositionUI();
            InitTabs();
            m_graphicRaycaster = m_canvas.GetComponent<GraphicRaycaster>();
        }

        private void PositionUI()
        {
            m_rectTransform = GetComponent<RectTransform>();
            Vector2 UISizeDelta = new Vector2(m_UIWidth, m_UIHeight);
            Utilities.PositionUI(m_rectTransform, UISizeDelta, m_AnchorPosition);
        }

        private void InitTabs()
        {
            if (m_tabs == null)
            {
                m_tabs = new List<Tab>();
            }
            if (m_areas == null)
            {
                m_areas = new List<TabArea>();
            }

            foreach (GameObject header in m_tabHeaders)
            {
                Tab tab = header.AddComponent<Tab>();
                tab.m_background = header.GetComponent<Image>();
                tab.m_tabManager = this;
                m_tabs.Add(tab);
            }

            foreach (GameObject tabArea in m_tabAreas)
            {
                TabArea area = tabArea.AddComponent<TabArea>();
                m_areas.Add(area);
            }

            m_activeTab = m_tabs[0];
            m_activeTabArea = m_areas[0];
            OnTabSelect?.Invoke(m_activeTab);
        }

        public void Enter(Tab tab)
        {
            tab.m_background.color = m_tabHoverColor;

        }
        public void Exit(Tab tab)
        {
            tab.m_background.color = m_tabIdleColor;
        }
        public void Select(Tab tab)
        {
            ResetTabs();
            tab.m_background.color = m_tabActiveColor;

            int tabIndex = m_tabs.IndexOf(tab);
            foreach (TabArea area in m_areas)
            {
                area.gameObject.SetActive(false);
            }
            m_areas[tabIndex].gameObject.SetActive(true);
        }
        public void ResetTabs()
        {
            foreach (Tab tab in m_tabs)
            {
                tab.m_background.color = m_tabIdleColor;
            }
        }

        void Update()
        {
            CheckForMouseInteraction();
        }
        private void CheckForMouseInteraction()
        {
            CheckForTabSelection();
            CheckForTabHighlight();
        }

        private void CheckForTabSelection()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Tab tab = UIRayCast();
                if (tab != null)
                {
                    if (tab != m_activeTab)
                    {
                        OnTabExit.Invoke(m_activeTab);
                    }
                    m_activeTab = tab;
                    m_highlightedTab = null;
                    OnTabSelect.Invoke(m_activeTab);
                }
            }
        }

        private void CheckForTabHighlight()
        {
            Tab tab = UIRayCast();

            if (tab != null && tab != m_activeTab)
            {
                if (m_highlightedTab != null)
                {
                    OnTabExit?.Invoke(m_highlightedTab);
                }
                m_highlightedTab = tab;
                OnTabEnter?.Invoke(m_highlightedTab);
            }
            else
            {
                if (m_highlightedTab != null)
                {
                    OnTabExit?.Invoke(m_highlightedTab);
                    m_highlightedTab = null;
                }
            }
        }

        Tab UIRayCast()
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            pointerData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            m_graphicRaycaster.Raycast(pointerData, results);

            Tab tab = null;
            foreach (RaycastResult result in results)
            {
                Tab hit = result.gameObject.GetComponent<Tab>();
                if (hit != null)
                {
                    tab = hit;
                    break;
                }
            }
            return tab;
        }
    }
}
