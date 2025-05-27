using UnityEngine;

public class WorldSettings : MonoBehaviour
{
    [Header("World Properties")]
    public float gravity = -9.81f;
    public Color skyboxColor = Color.black;
    public Color ambientLight = Color.white;
    public float ambientIntensity = 1f;

    [Header("Materials")]
    public MaterialManager materialManager;

    [Header("Spawn Points")]
    public Transform[] spawnPoints;

    private void Awake()
    {
        // Set physics
        Physics.gravity = new Vector3(0, gravity, 0);

        // Set lighting and environment
        RenderSettings.ambientLight = ambientLight;
        RenderSettings.ambientIntensity = ambientIntensity;

        if (materialManager != null)
        {
            // Apply materials to the world
            materialManager.ApplySkybox();
            
            // Apply materials to ground and walls
            GameObject ground = GameObject.Find("Ground");
            if (ground != null) materialManager.ApplyGroundMaterial(ground);

            GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");
            foreach (GameObject wall in walls)
            {
                materialManager.ApplyWallMaterial(wall);
            }

            // Apply materials to player
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) materialManager.ApplyPlayerMaterials(player);
        }
    }

    public Vector3 GetRandomSpawnPoint()
    {
        if (spawnPoints == null || spawnPoints.Length == 0)
            return Vector3.zero;

        int randomIndex = Random.Range(0, spawnPoints.Length);
        return spawnPoints[randomIndex].position;
    }
}
