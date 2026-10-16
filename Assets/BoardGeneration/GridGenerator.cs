using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    [SerializeField] GameObject hexPrefab;

    public int width = 10;
    public int height = 10;
    public float radius = 1f; // Distance from center to vertex

    void Start()
    {
        float xStep = radius * Mathf.Sqrt(3f);
        float zStep = radius * 1.5f;

        for (int z = 0; z < height; z++)
        {
            // Every odd row is shifted right by half a horizontal step
            float xOffset = (z % 2 == 1) ? (xStep / 2f) : 0f;

            for (int x = 0; x < width; x++)
            {
                float xPos = (x * xStep) + xOffset;
                float zPos = z * zStep;

                Vector3 position = new Vector3(xPos, 0f, zPos);
                Quaternion spawnRotation = Quaternion.Euler(-90f, 0f, 0f);
                Instantiate(hexPrefab, position, spawnRotation, transform);
            }
        }
    }

    //first thing I want to do is be able to make a custom board.
    //I'll need inputs for the row width, so I can do this with an array of ints. first will be the bottom row, then next, then next, etc...
    //after I can figure out making the ocean area



}
