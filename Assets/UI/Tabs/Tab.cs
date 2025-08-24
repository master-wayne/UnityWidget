using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DCG_UI
{
    [RequireComponent(typeof(Image))]
    public class Tab : MonoBehaviour
    {
        public TabManager m_tabManager;

        [HideInInspector]
        public Image m_background;
    }
}
