// Copyright 2019. LCH. All rights reserved.

using System;
using System.Diagnostics;
using UnityEngine;
using UnityEditor;
/*
namespace LCH
{
	[DisallowMultipleComponent]
	public class FontsubsetMakerSettingWindow : EditorWindow
    {
		private const string TAG = nameof(FontsubsetMakerSettingWindow);
        private const float WIDTH = 115;



        [MenuItem("Tools/Fontsubset Maker/Settings")]
        private static void ShowWindow()
        {
            GetWindow(typeof(FontsubsetMakerSettingWindow));
        }



        private string pyftsubsetFolderPath =
#if UNITY_EDITOR_WIN
        "C:\\Users/LCH/AppData/Local/Programs/Python/Python38-32/Scripts";
#elif UNITY_EDITOR_OSX
        "/Library/Frameworks/Python.framework/Versions/3.8/bin";
#endif



        private void OnGUI()
        {
#if UNITY_EDITOR_WIN
            InstantiateFileBrosweGUI("Pytfsubset.exe path: ", pyftsubsetFolderPath, "exe", ref FontsubsetMaker.pyftsubsetFilePath);
#elif UNITY_EDITOR_OSX
            InstantiateFileBrosweGUI("Pytfsubset path: ", FontsubsetMaker.pyftsubsetFilePath, () => FontsubsetMaker.pyftsubsetFilePath = EditorUtility.OpenFilePanel("", pyftsubsetFolderPath, ""));
#endif
            InstantiateFileBrosweGUI("glyphs.txt path: ", FontsubsetMaker.glyphsTxtFilePath, () => FontsubsetMaker.glyphsTxtFilePath = EditorUtility.OpenFilePanel("", Application.dataPath, "txt"));

            for (int i = 0; i <= FontsubsetMaker.scenePathsIncludedText.Count; i++)
            {
                if (i < FontsubsetMaker.scenePathsIncludedText.Count)
                {
                    InstantiateFileBrosweGUI("scene.unity path included text: ", FontsubsetMaker.scenePathsIncludedText[i], () => FontsubsetMaker.scenePathsIncludedText[i] = EditorUtility.OpenFilePanel("", Application.dataPath, "unity"));
                }
                else if (i == FontsubsetMaker.scenePathsIncludedText.Count)
                {
                    InstantiateFileBrosweGUI("scene.unity path included text: ", "", () => FontsubsetMaker.scenePathsIncludedText.Add(EditorUtility.OpenFilePanel("", Application.dataPath, "unity")));
                }
            }
        }



        private void InstantiateFileBrosweGUI(string label, string returnFilePath, Action onClickButton)
        {
            GUILayout.BeginHorizontal(GUILayout.MinWidth(WIDTH * 4));
            GUILayout.Label(label, GUILayout.Width(WIDTH));
            GUILayout.TextField(returnFilePath, GUILayout.MinWidth(WIDTH));

            if (GUILayout.Button("Browse", GUILayout.Width(WIDTH))) onClickButton?.Invoke();

            GUILayout.EndHorizontal();
        }
    }
}
*/