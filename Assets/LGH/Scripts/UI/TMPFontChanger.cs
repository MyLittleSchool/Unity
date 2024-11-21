using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class TMPFontChanger : MonoBehaviour
{
//    public TMP_FontAsset newFont; // 새로 적용할 폰트
//    public TMP_FontAsset targetFont; // 변경하고자 하는 현재 폰트

//    [ContextMenu("Change All TMP Fonts In Scene")]
//    public void ChangeAllFontsInScene()
//    {
//        // 현재 씬의 모든 TMP 텍스트 컴포넌트 찾기
//        TextMeshProUGUI[] sceneTexts = Resources.FindObjectsOfTypeAll<TextMeshProUGUI>();
//        TextMeshPro[] sceneTMP = Resources.FindObjectsOfTypeAll<TextMeshPro>();

//        int changedCount = 0;

//        // UI 텍스트(TextMeshProUGUI) 폰트 변경
//        foreach (TextMeshProUGUI text in sceneTexts)
//        {
//            if (targetFont == null || text.font == targetFont)
//            {
//                Undo.RecordObject(text, "Change Font");
//                text.font = newFont;
//                changedCount++;
//            }
//        }

//        // World 텍스트(TextMeshPro) 폰트 변경
//        foreach (TextMeshPro tmp in sceneTMP)
//        {
//            if (targetFont == null || tmp.font == targetFont)
//            {
//                Undo.RecordObject(tmp, "Change Font");
//                tmp.font = newFont;
//                changedCount++;
//            }
//        }

//        Debug.Log($"Changed {changedCount} texts to new font: {newFont.name}");
//    }

//    [ContextMenu("Change All TMP Fonts In Project")]
//    public void ChangeAllFontsInProject()
//    {
//        // 프로젝트 내의 모든 프리팹 찾기
//        string[] prefabPaths = AssetDatabase.FindAssets("t:Prefab");
//        int changedCount = 0;

//        foreach (string guid in prefabPaths)
//        {
//            //string path = AssetDatabase.GUIDToAssetPath(guid);
//            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

//            // 프리팹의 모든 TMP 컴포넌트 찾기
//            TextMeshProUGUI[] uiTexts = prefab.GetComponentsInChildren<TextMeshProUGUI>(true);
//            TextMeshPro[] tmpTexts = prefab.GetComponentsInChildren<TextMeshPro>(true);

//            bool prefabModified = false;

//            // UI 텍스트 변경
//            foreach (TextMeshProUGUI text in uiTexts)
//            {
//                if (targetFont == null || text.font == targetFont)
//                {
//                    text.font = newFont;
//                    prefabModified = true;
//                    changedCount++;
//                }
//            }

//            // World 텍스트 변경
//            foreach (TextMeshPro tmp in tmpTexts)
//            {
//                if (targetFont == null || tmp.font == targetFont)
//                {
//                    tmp.font = newFont;
//                    prefabModified = true;
//                    changedCount++;
//                }
//            }

//            // 변경사항이 있는 경우에만 프리팹 저장
//            if (prefabModified)
//            {
//                PrefabUtility.SavePrefabAsset(prefab);
//            }
//        }

//        Debug.Log($"Changed {changedCount} texts in project prefabs to new font: {newFont.name}");
//    }
}
