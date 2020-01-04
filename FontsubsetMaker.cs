// Copyright 2019. LCH. All rights reserved.

using TMPro;
using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.UI;

namespace LCH
{
#if UNITY_EDITOR
    public class FontsubsetMaker : LCHMonoBehaviour
    {
        private const string TAG = nameof(FontsubsetMaker);



        private static string pyftsubsetPath = Path.Combine("C:\\", "Users", "LCH", "AppData", "Local", "Programs", "Python", "Python38-32", "Scripts", "pyftsubset.exe");
        private static string collectedText;
        private static bool isCollected;

        // Not subset font paths, Save fontsubset path.
        private static FontsubsetPath[] fontsubsetPaths = {
            new FontsubsetPath(Path.Combine(Application.dataPath, "LCHFramework", "Fonts", "NotoSans", "Original"), Path.Combine(Application.dataPath, "LCHFramework", "Fonts", "NotoSans", "Subset")),
            new FontsubsetPath(Path.Combine(Application.dataPath, "LCHFramework", "Fonts", "NotoSansMono", "Original"), Path.Combine(Application.dataPath, "LCHFramework", "Fonts", "NotoSansMono", "Subset")),
        };


        private static string SubsetTxtPath
        {
            get
            {
                return Path.Combine(fontsubsetPaths[0].outputPath, "subset.txt");
            }
        }



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
            ConsoleWindow.Clear();

            Debug.Log($"{TAG}, CollectTexts(): Begin.");

            if (!EditorApplication.isPlaying)
            {
                string beginScenePath = null;
                string[] activeScenePaths = EditorBuildSettingsScene.GetActiveSceneList(EditorBuildSettings.scenes);

                collectedText = null;

                foreach(string activeScenePath in activeScenePaths)
                {
                    if (activeScenePath == UnityEngine.SceneManagement.SceneManager.GetActiveScene().path)
                    {
                        beginScenePath = UnityEngine.SceneManagement.SceneManager.GetActiveScene().path;

                        break;
                    }
                }

                foreach(string activeScenePath in activeScenePaths)
                {
                    EditorSceneManager.OpenScene(activeScenePath);

                    foreach (GameObject rootGameObject in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects())
                    {
                        foreach (Text text in rootGameObject.GetComponentsInChildren<Text>())
                        {
                            collectedText += text.text + " ";
                        }

                        foreach (TMP_Text tmp_text in rootGameObject.GetComponentsInChildren<TMP_Text>())
                        {
                            collectedText += tmp_text.text + " ";
                        }
                    }
                }

                EditorSceneManager.OpenScene(beginScenePath);
                isCollected = true;
            }
            else
            {
                Debug.Log($"{TAG}, 플레이 모드에서는 사용할 수 없습니다.");
            }
        }

        private static void ExportSubsetTxt()
        {
            File.WriteAllText(SubsetTxtPath, collectedText);

            Debug.Log($"{TAG}, {SubsetTxtPath} 을(를) 생성했습니다.");
        }

        [MenuItem("Tools/Fontsubset Maker/Make Fontsubset", false, 20)]
        private static void MakeFontsubset()
        {
            if (File.Exists(pyftsubsetPath))
            {
                if (File.Exists(SubsetTxtPath))
                {
                    foreach (FontsubsetPath fontsubsetPath in fontsubsetPaths)
                    {
                        if (Directory.Exists(fontsubsetPath.originalPath))
                        {
                            string[] originalFontPaths = (new string[] { "*.ttf", "*.otf" }).SelectMany(searchPattern => Directory.GetFiles(fontsubsetPath.originalPath, searchPattern)).ToArray();

                            if (originalFontPaths == null || originalFontPaths.Length == 0)
                            {
                                Debug.LogError($"{TAG}, {fontsubsetPath.originalPath} 에 폰트가 " + (originalFontPaths == null ? "0" : originalFontPaths.Length.ToString()) + " 개 있습니다.");
                            }
                            else
                            {
                                foreach (string originalFontPath in originalFontPaths)
                                {
                                    string arguments = $"{originalFontPath} --output-file={fontsubsetPath.outputPath + originalFontPath.Replace(fontsubsetPath.originalPath, "")} --text-file={SubsetTxtPath}";
                                    System.Diagnostics.Process.Start(pyftsubsetPath, arguments);
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
                    Debug.LogError($"{TAG}, {SubsetTxtPath} 이 없습니다.");
                }
            }
            else
            {
                Debug.LogError($"{TAG}, {pyftsubsetPath} 을(를) 찾을 수 없습니다. 파이썬은 설치하셨나요?");
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
#endif
}
