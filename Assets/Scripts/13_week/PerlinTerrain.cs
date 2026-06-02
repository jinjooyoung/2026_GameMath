using UnityEngine;

public class PerlinTerrain : MonoBehaviour
{
    public int width = 30;
    public int depth = 30;
    public float scale = 0.1f;
    public float heightMultiplier = 8f;
    public GameObject cubePrefab;

    void Start()
    {
        Generate();
    }

    public void Generate()
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                float noise = Mathf.PerlinNoise(x * scale, z * scale);
                int height = Mathf.RoundToInt(noise * heightMultiplier);
                CreateCube(x, z, height);
            }
        }
    }

    void CreateCube(int x, int z, int height)
    {
        for (int y = 0; y <= height; y++)
        {
            Vector3 position = new Vector3(x, y, z);

            Instantiate(cubePrefab, position, Quaternion.identity, transform);
        }
    }

    Vector2 GetGradient(int x, int z, int seed)
    {
        int hash = x * 17 + z * 31 + seed;
        hash = Mathf.Abs(hash);

        int index = hash % gradients.Length;

        return gradients[index];
    }

    Vector2[] gradients =
    {
        new Vector2(1,0),
        new Vector2(-1,0),
        new Vector2(0,1),
        new Vector2(0,-1),
        new Vector2(1,1).normalized,
        new Vector2(-1,1).normalized,
        new Vector2(1,-1).normalized,
        new Vector2(-1,-1).normalized,
    };

    float Fade(float t)
    {
        return t * t * t * (t * (t * 6f - 15f) + 10f);
    }
}
