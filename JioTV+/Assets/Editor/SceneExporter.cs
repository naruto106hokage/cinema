using UnityEditor;
using UnityEngine;
using System.IO;

public class SceneExporter : Editor
{
    [MenuItem("Tools/Export Scene to Folder")]
    public static void ExportSceneToFolder()
    {
        string scenePath = EditorUtility.OpenFilePanel("Select Scene", "Assets", "unity");
        if (string.IsNullOrEmpty(scenePath))
        {
            Debug.LogWarning("No scene selected.");
            return;
        }

        string sceneName = Path.GetFileNameWithoutExtension(scenePath);
        string exportFolder = EditorUtility.SaveFolderPanel("Select Export Folder", "", sceneName);
        if (string.IsNullOrEmpty(exportFolder))
        {
            Debug.LogWarning("No export folder selected.");
            return;
        }

        string relativeScenePath = "Assets" + scenePath.Substring(Application.dataPath.Length);
        var dependencies = AssetDatabase.GetDependencies(relativeScenePath, true);

        foreach (var dependency in dependencies)
        {
            if (!dependency.StartsWith("Assets"))
                continue; // Skip built-in assets

            string destinationPath = Path.Combine(exportFolder, dependency.Substring("Assets/".Length));
            string destinationDirectory = Path.GetDirectoryName(destinationPath);
            if (!Directory.Exists(destinationDirectory))
                Directory.CreateDirectory(destinationDirectory);

            File.Copy(dependency, destinationPath, true);
        }

        Debug.Log($"Scene and dependencies exported to {exportFolder}");
    }
}
