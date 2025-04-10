using UnityEngine;

public class SheepManager : MonoBehaviour
{
    public GameObject sheepPrefab;
    public Transform waterSurface;

    public int sheepCount = 300;

    public Terrain terrain;

    void Start()
    {
        for (int i = 0; i < sheepCount; i++)
        {
            Vector3 spawnPosition = GetRandomPositionOnTerrain();
            Instantiate(sheepPrefab, spawnPosition, Quaternion.identity);
        }
        UIManager.Instance.UpdateSheepCount(sheepCount);
    }

    Vector3 GetRandomPositionOnTerrain()
    {
        float terrainWidth = terrain.terrainData.size.x;
        float terrainLength = terrain.terrainData.size.z;
        float waterHeight = waterSurface.position.y;

        for (int i = 0; i < 300; i++) // 最大300回試す
        {
            float randomX = Random.Range(0, terrainWidth);
            float randomZ = Random.Range(0, terrainLength);
            float y = terrain.SampleHeight(new Vector3(randomX, 0, randomZ));

            if (y > waterHeight)
            {
                return new Vector3(randomX, y + 1.0f, randomZ); // 足元を1浮かせる
            }
        }

        // 条件を満たさなかった場合、中央に出す
        return new Vector3(terrainWidth / 2, terrain.SampleHeight(Vector3.zero) + 1.0f, terrainLength / 2);
    }
}
