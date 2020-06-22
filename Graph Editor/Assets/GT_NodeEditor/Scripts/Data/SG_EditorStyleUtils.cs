using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public static class SG_EditorStyleUtils
{
    
    public static GUIStyle LabelPlainWhite(float width = 0)
    {
        GUIStyle s = new GUIStyle(EditorStyles.label);
        s.normal.textColor = Color.white;
        s.fontStyle = FontStyle.Normal;
        s.margin.right = 10;
        s.margin.left = 10;
        s.padding.right = 0;
        s.padding.left = 0;

        if (width != 0)
            s.fixedWidth = width;

        return s;
    }

    public static GUIStyle LabelBoldWhite(float width = 0)
    {
        GUIStyle s = new GUIStyle(EditorStyles.label);
        s.normal.textColor = Color.white;
        s.fontStyle = FontStyle.Bold;
        s.margin.right = 10;
        s.margin.left = 10;
        s.padding.right = 0;
        s.padding.left = 0;

        if (width != 0)
            s.fixedWidth = width;

        return s;
    }

}
