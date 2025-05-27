
using UnityEditor;
using UnityEngine;
using System.IO;

public class ProjectStructureCreator
{
    [MenuItem("Tools/Create Project Structure")]
    public static void CreateFolders()
    {
        string[] folders = {
            "Assets/Scenes",
            "Assets/Scripts",
            "Assets/Models",
            "Assets/Animations",
            "Assets/Prefabs",
            "Assets/Materials",
            "Assets/UI",
            "Assets/Audio"
        };

        foreach (string folder in folders)
        {
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
        }

        AssetDatabase.Refresh();
        Debug.Log("✅ 폴더 구조 생성 완료!");
    }
}
