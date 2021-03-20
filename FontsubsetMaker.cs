// Copyright 2019. LCH. All rights reserved.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

namespace LCH
{
    public class FontsubsetMaker : MonoBehaviour
    {
        private const string TAG = nameof(FontsubsetMaker);



        public static string pyftsubsetFilePath;
        public static string glyphsTxtFilePath;
        public static List<string> scenePathsIncludedText = new List<string>();


        private static string collectedText;


        // Not subset font paths, Save fontsubset path.
        private static FontsubsetPath[] fontsubsetPaths = {
            new FontsubsetPath(Application.dataPath + "/LCHFramework/Fonts/NotoSans/Original", Application.dataPath + "/LCHFramework/Fonts/NotoSans/Subset"),
            new FontsubsetPath(Application.dataPath + "/LCHFramework/Fonts/NotoSansMono/Original", Application.dataPath +"/LCHFramework/Fonts/NotoSansMono/Subset")
        };



        [MenuItem("Tools/Fontsubset Maker/Collect Texts > Export subset.txt > Make Fontsubset", false, 10)]
        private static void CollectAndExportAndMakeFontsubset()
        {
            CollectTextsAndExportSubsetTxt();
            MakeFontsubset();
        }

        [MenuItem("Tools/Fontsubset Maker/Collect Texts > Export subset.txt", false, 30)]
        private static void CollectTextsAndExportSubsetTxt()
        {
            CollectTexts();
            ExportSubsetTxt();
        }

        private static void CollectTexts()
        {
            string beginScenePath = UnityEngine.SceneManagement.SceneManager.GetActiveScene().path;
            collectedText = null;

            foreach (string scene in scenePathsIncludedText)
            {
                EditorSceneManager.OpenScene(scene);

                foreach (GameObject rootGameObject in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects())
                {
                    foreach (Text text in rootGameObject.GetComponentsInChildren<Text>(true)) if (!collectedText.Contains(text.text)) collectedText += text.text + " ";
                    foreach (TMP_Text tmp_text in rootGameObject.GetComponentsInChildren<TMP_Text>(true)) if (!collectedText.Contains(tmp_text.text)) collectedText += tmp_text.text + " ";
                }
            }

            EditorSceneManager.OpenScene(beginScenePath);
        }

        private static void ExportSubsetTxt()
        {
            File.WriteAllText(glyphsTxtFilePath, collectedText);

            Debug.Log($"{TAG}, {glyphsTxtFilePath} 을(를) 생성했습니다.");
        }

        [MenuItem("Tools/Fontsubset Maker/Make Fontsubset", false, 20)]
        private static void MakeFontsubset()
        {
            if (File.Exists(pyftsubsetFilePath))
            {
                if (File.Exists(glyphsTxtFilePath))
                {
                    foreach (FontsubsetPath fontsubsetPath in fontsubsetPaths)
                    {
                        if (Directory.Exists(fontsubsetPath.originalPath))
                        {
                            string[] originalFontPaths = (new string[] { "*.ttf", "*.otf", "*.ttc" }).SelectMany(searchPattern => Directory.GetFiles(fontsubsetPath.originalPath, searchPattern)).ToArray();

                            if (originalFontPaths == null || originalFontPaths.Length == 0)
                            {
                                Debug.LogError($"{TAG}, {fontsubsetPath.originalPath} 에 폰트가 " + (originalFontPaths == null ? "0" : originalFontPaths.Length.ToString()) + " 개 있습니다.");
                            }
                            else
                            {
                                foreach (string originalFontPath in originalFontPaths)
                                {

                                    string arguments =
#if UNITY_EDITOR_WIN
                                        $"{originalFontPath}" +
                                        $" --text-file={glyphsTxtFilePath}" +
                                        $" --output-file={fontsubsetPath.outputPath + originalFontPath.Replace(fontsubsetPath.originalPath, "")}" +
#elif UNITY_EDITOR_OSX
                                        $"\"{originalFontPath}\"" +
                                        $" --text-file=\"{glyphsTxtFilePath}\"" +
                                        $" --output-file=\"{fontsubsetPath.outputPath + originalFontPath.Replace(fontsubsetPath.originalPath, "")}\"" +
#endif
                                        $" --layout-features='*'" +
                                        $" --glyph-names" +
                                        $" --symbol-cmap" +
                                        $" --legacy-cmap" +
                                        $" --notdef-glyph" +
                                        $" --notdef-outline" +
                                        $" --recommended-glyphs" +
                                        $" --name-legacy" +
                                        $" --drop-tables=" +
                                        $" --name-IDs='*'" +
                                        $" --name-languages='*'";

                                    try
                                    {
                                        System.Diagnostics.Process.Start(pyftsubsetFilePath, arguments);
                                    }
                                    catch (Exception e)
                                    {
                                        Debug.LogError(e);
#if UNITY_EDITOR_OSX
                                        Debug.Log(TAG + ", please execute in terminal \"chmod -R 777 {Target folder}\"");
#endif
                                    }
                                }
                            }
                        }
                        else
                        {
                            Debug.LogError($"{TAG}, {fontsubsetPath.originalPath} 을(를) 찾을 수 없습니다.");
                        }
                    }
                }
                else
                {
                    Debug.LogError($"{TAG}, {glyphsTxtFilePath} 이 없습니다.");
                }
            }
            else
            {
                Debug.LogError($"{TAG}, {pyftsubsetFilePath} 을(를) 찾을 수 없습니다. 파이썬은 설치하셨나요?");
            }
        }



        private struct FontsubsetPath
        {
            public string originalPath;
            public string outputPath;



            public FontsubsetPath(string originalPath, string outputPath)
            {
                this.originalPath = originalPath;
                this.outputPath = outputPath;
            }
        }
    }
}