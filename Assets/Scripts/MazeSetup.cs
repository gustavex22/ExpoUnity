using UnityEngine;

/// <summary>
/// Utilidad para generar un laberinto simple de paredes usando primitivas Cubos.
/// Asigna este script a un GameObject vacío llamado "MazeSetup" y pulsa
/// el engranaje del inspector > "Generar Laberinto", o llámalo desde código.
/// </summary>
public class MazeSetup : MonoBehaviour
{
    [Header("Configuración del laberinto")]
    [Tooltip("Prefab opcional para las paredes. Si se deja null se usará un Cube primitivo.")]
    public GameObject wallPrefab;

    [Tooltip("Tamaño de cada celda del laberinto.")]
    public float cellSize = 2f;

    [Tooltip("Ancho del laberinto en celdas.")]
    public int width = 10;

    [Tooltip("Alto del laberinto en celdas.")]
    public int height = 10;

    [Tooltip("Altura de las paredes.")]
    public float wallHeight = 3f;

    [Tooltip("Grosor de las paredes.")]
    public float wallThickness = 0.2f;

    [Tooltip("Objeto padre bajo el cual se agruparán las paredes generadas.")]
    public Transform mazeParent;

    [ContextMenu("Generar Laberinto")]
    public void GenerateMaze()
    {
        if (mazeParent == null)
        {
            GameObject parent = new GameObject("Laberinto");
            mazeParent = parent.transform;
        }

        // Patrón simple: bordes exteriores + algunas paredes internas
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                // Paredes de borde exterior (forma un rectángulo cerrado)
                if (x == 0 || x == width - 1 || z == 0 || z == height - 1)
                {
                    CreateWall(new Vector3(x * cellSize, wallHeight / 2f, z * cellSize));
                }
            }
        }

        // Paredes internas de ejemplo para formar pasillos (edita este patrón a tu gusto)
        CreateWall(new Vector3(3 * cellSize, wallHeight / 2f, 3 * cellSize));
        CreateWall(new Vector3(4 * cellSize, wallHeight / 2f, 3 * cellSize));
        CreateWall(new Vector3(5 * cellSize, wallHeight / 2f, 5 * cellSize));
        CreateWall(new Vector3(5 * cellSize, wallHeight / 2f, 6 * cellSize));
        CreateWall(new Vector3(7 * cellSize, wallHeight / 2f, 2 * cellSize));
        CreateWall(new Vector3(7 * cellSize, wallHeight / 2f, 8 * cellSize));

        Debug.Log($"Laberinto generado: {width}x{height} celdas.");
    }

    private void CreateWall(Vector3 position)
    {
        GameObject wall;
        if (wallPrefab != null)
        {
            wall = Instantiate(wallPrefab, position, Quaternion.identity, mazeParent);
        }
        else
        {
            wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
            wall.transform.position = position;
            wall.transform.SetParent(mazeParent);
        }

        // Escalar al grosor y altura deseados (Cube mide 1x1x1 por defecto)
        wall.transform.localScale = new Vector3(wallThickness, wallHeight, wallThickness);

        // Asegurar BoxCollider y marcar como estática para optimizar física
        BoxCollider collider = wall.GetComponent<BoxCollider>();
        if (collider == null)
        {
            collider = wall.AddComponent<BoxCollider>();
        }

        wall.isStatic = true;
        wall.name = "Pared";
        wall.tag = "Untagged";
    }
}
