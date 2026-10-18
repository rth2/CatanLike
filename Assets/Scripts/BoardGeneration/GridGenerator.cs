using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

public class GridGenerator : MonoBehaviour
{
    [SerializeField] GameObject hexPrefab;

    [SerializeField] int[] boardRows;

    public float radius = 1f; // Distance from center to vertex

    public void CreateBoard()
    {
        float xStep = radius * Mathf.Sqrt(3f);
        float zStep = radius * 1.5f;


        for (int row = 0; row < boardRows.Length; row++)
        {
            int tileCount = boardRows[row];

            float rowOffset = (row % 2 == 1) ? xStep / 2f : 0f;

            float rowWidth = (tileCount - 1) * xStep;
            float firstTileX = -rowWidth / 2f - rowOffset;

            for (int col = 0; col < tileCount; col++)
            {
                float xPos = firstTileX + col * xStep + rowOffset;
                float zPos = row * zStep;

                Vector3 position = new Vector3(xPos, 0f, zPos);
                Quaternion spawnRotation = Quaternion.Euler(-90f, 0f, 0f);

                #if UNITY_EDITOR
                if (!Application.isPlaying)
                {
                    GameObject tile = (GameObject)PrefabUtility.InstantiatePrefab(
                        hexPrefab, gameObject.scene);

                    Undo.RegisterCreatedObjectUndo(tile, "Generate Board Tile");
                    tile.transform.SetParent(transform, false);
                    tile.transform.localPosition = position;
                    tile.transform.localRotation = spawnRotation;
                    continue;
                }
                #endif

                Instantiate(hexPrefab, position, spawnRotation, transform);
            }

        }
        #if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            EditorSceneManager.MarkSceneDirty(gameObject.scene);
        }
        #endif
    }
}
