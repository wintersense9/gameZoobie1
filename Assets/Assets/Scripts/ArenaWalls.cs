using UnityEngine;

/// <summary>
/// Attach to any empty GameObject — auto-generates 4 invisible walls around the arena on Start.
/// </summary>
public class ArenaWalls : MonoBehaviour
{
    public float arenaSize = 18f;  // total width/length of the arena
    public float wallHeight = 4f;

    void Awake()
    {
        float half = arenaSize / 2f;
        float thickness = 1f;

        CreateWall(new Vector3(0, wallHeight / 2f, half),           new Vector3(arenaSize, wallHeight, thickness)); // North
        CreateWall(new Vector3(0, wallHeight / 2f, -half),          new Vector3(arenaSize, wallHeight, thickness)); // South
        CreateWall(new Vector3(half, wallHeight / 2f, 0),           new Vector3(thickness, wallHeight, arenaSize)); // East
        CreateWall(new Vector3(-half, wallHeight / 2f, 0),          new Vector3(thickness, wallHeight, arenaSize)); // West
    }

    void CreateWall(Vector3 position, Vector3 scale)
    {
        var wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        wall.name = "Wall";
        wall.transform.position = position;
        wall.transform.localScale = scale;

        // Grey so walls are visible in game
        wall.GetComponent<Renderer>().material.color = new Color(0.4f, 0.4f, 0.4f);
    }
}

