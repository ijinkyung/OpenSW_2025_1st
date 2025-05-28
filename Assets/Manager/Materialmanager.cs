using UnityEngine;
using System.Collections.Generic;

public class MaterialManager : MonoBehaviour
{
    public static MaterialManager Instance;

    [Header("Material Dictionary")]
    public List<MaterialEntry> materialList = new List<MaterialEntry>();
    private Dictionary<string, Material> materialDict = new Dictionary<string, Material>();

    [System.Serializable]
    public class MaterialEntry
    {
        public string materialName;
        public Material material;
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        foreach (var entry in materialList)
        {
            if (!materialDict.ContainsKey(entry.materialName) && entry.material != null)
                materialDict.Add(entry.materialName, entry.material);
        }
    }

    /// <summary>
    /// 기본 머티리얼 적용: 단일 렌더러 대상
    /// </summary>
    public void ApplyMaterial(GameObject target, string materialName)
    {
        if (materialDict.ContainsKey(materialName))
        {
            Renderer renderer = target.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = materialDict[materialName];
                Debug.Log($"Material '{materialName}' applied to '{target.name}'");
            }
            else
            {
                Debug.LogWarning("Renderer not found on " + target.name);
            }
        }
        else
        {
            Debug.LogWarning("Material not found: " + materialName);
        }
    }

    /// <summary>
    /// Skybox 머티리얼 변경
    /// </summary>
    public void ApplySkybox(string materialName)
    {
        if (materialDict.ContainsKey(materialName))
        {
            RenderSettings.skybox = materialDict[materialName];
            Debug.Log("Skybox material applied: " + materialName);
        }
        else
        {
            Debug.LogWarning("Skybox material not found: " + materialName);
        }
    }

    /// <summary>
    /// Ground (바닥) 머티리얼 적용
    /// </summary>
    public void ApplyGroundMaterial(GameObject groundObject, string materialName)
    {
        ApplyMaterial(groundObject, materialName);
    }

    /// <summary>
    /// Wall (벽) 머티리얼 적용
    /// </summary>
    public void ApplyWallMaterial(GameObject wallObject, string materialName)
    {
        ApplyMaterial(wallObject, materialName);
    }

    /// <summary>
    /// Player 모델의 모든 Renderer에 머티리얼 적용
    /// </summary>
    public void ApplyPlayerMaterials(GameObject playerObject, string materialName)
    {
        if (!materialDict.ContainsKey(materialName))
        {
            Debug.LogWarning("Material not found: " + materialName);
            return;
        }

        Renderer[] renderers = playerObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in renderers)
        {
            r.material = materialDict[materialName];
        }
        Debug.Log("Applied player material to all parts: " + materialName);
    }

    /// <summary>
    /// 외부에서 직접 Material 반환
    /// </summary>
    public Material GetMaterial(string materialName)
    {
        if (materialDict.ContainsKey(materialName))
            return materialDict[materialName];
        else
            return null;
    }
}
