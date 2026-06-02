using UnityEngine;
using UnityEngine.UIElements;

public class SimplePerlinTerrain : MonoBehaviour
{
    public int width = 30;
    public int depth = 30;
    public int waterHeight = 3;
    public float scale = 0.1f;
    public float heightMultiplier = 8f;

    [Header("Prefabs")]
    public GameObject dirtPrefab;
    public GameObject grassPrefab;
    public GameObject waterPrefab;

    SimplePerlinNoise simpleNoise;

    int XOffset = 0;
    int ZOffset = 0;

    private void Start()
    {
        simpleNoise = GetComponent<SimplePerlinNoise>();

        XOffset = Random.Range(-9999, 9999);
        ZOffset = Random.Range(-9999, 9999);
        
        Generate();
    }

    public void Generate()
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                float xCoord = (x + XOffset) * scale;
                float zCoord = (z + ZOffset) * scale;

                float noise = simpleNoise.Noise(xCoord, zCoord);

                int height = Mathf.RoundToInt(noise * heightMultiplier);

                CreateCube(x, z, height);

                if (height < waterHeight)
                {
                    CreateWater(x, z, height+1);
                }
            }
        }
    }

    void CreateCube(int x, int z, int height)
    {
        for (int y = 0; y <= height; y++)
        {
            if (y == height)
            {
                Vector3 maxHeight = new Vector3(x, height, z);

                Instantiate(grassPrefab, maxHeight, Quaternion.identity, transform);
            }
            else
            {
                Vector3 position = new Vector3(x, y, z);

                Instantiate(dirtPrefab, position, Quaternion.identity, transform);
            }
        }
    }

    void CreateWater(int x, int z, int startHeight)
    {
        for (int y = startHeight; y <= waterHeight; y++)
        {
            Vector3 position = new Vector3(x, y, z);

            Instantiate(waterPrefab, position, Quaternion.identity, transform);
        }
    }
}
