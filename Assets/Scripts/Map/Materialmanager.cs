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
        // Singleton 설정
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        // Dictionary로 변환
        foreach (var entry in materialList)
        {
            if (!materialDict.ContainsKey(entry.materialName))
                materialDict.Add(entry.materialName, entry.material);
        }
    }

    /// <summary>
    /// materialName으로 재질을 찾아 해당 오브젝트에 적용
    /// </summary>
    public void ApplyMaterial(GameObject target, string materialName)
    {
        if (materialDict.ContainsKey(materialName))
        {
            Renderer renderer = target.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = materialDict[materialName];
            }
            else
            {
                Debug.LogWarning("Renderer가 없습니다: " + target.name);
            }
        }
        else
        {
            Debug.LogWarning("등록되지 않은 머티리얼 이름: " + materialName);
        }
    }

    /// <summary>
    /// 외부에서 Material 객체를 직접 요청
    /// </summary>
    public Material GetMaterial(string materialName)
    {
        if (materialDict.ContainsKey(materialName))
            return materialDict[materialName];
        else
            return null;
    }
}
