using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MenuItem
{
    public string m_text = String.Empty;

    public TMPro.TextAlignmentOptions m_menuTextAlignment = TMPro.TextAlignmentOptions.Center;
    public Color m_defaultColor = Color.gray;
    public Color m_onHoverColor = Color.blue;
    public int m_fontSize = 14;
    public Color m_textColor = Color.white;
    public GameObject m_scriptObject = null;
    public List<string> m_functions = new List<string>();
    public int m_selectedFunctionIndex = 0;
    public string m_functionToCall = String.Empty;
    public bool m_IsHidden = true;
}
