using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

public static class LightmapUVMenu
{
    [MenuItem("Tools/Generate Lightmap UVs for All Models")]
    private static void SetAllModelsLightmapUVs()
    {
        if (!EditorUtility.DisplayDialog("Batch Set Lightmap UVs",
            "Включить 'Generate Lightmap UVs' для ВСЕХ 3D-моделей в проекте?",
            "Да", "Отмена"))
            return;

        string[] guids = AssetDatabase.FindAssets("t:Model");
        int total = guids.Length;
        int changed = 0;

        try
        {
            for (int i = 0; i < total; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                ModelImporter importer = AssetImporter.GetAtPath(path) as ModelImporter;

                EditorUtility.DisplayProgressBar("Setting Lightmap UVs",
                    $"Обработка: {path}", (float)i / total);

                if (importer != null && !importer.generateSecondaryUV)
                {
                    importer.generateSecondaryUV = true;
                    importer.SaveAndReimport();
                    changed++;
                }
            }
        }
        finally
        {
            EditorUtility.ClearProgressBar();
        }

        AssetDatabase.Refresh();
        Debug.Log($"Готово! 'Generate Lightmap UVs' включено у {changed} из {total} моделей.");
    }
}
#endif