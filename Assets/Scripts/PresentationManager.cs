using UnityEngine;
using UnityEngine.Tilemaps;

public class PresentationManager : MonoBehaviour
{
    public static PresentationManager main;

    public Tilemap wallTilemap;
    public Tile wallTile;
    public Tile floorTile;

    public GameObject barrelPrefab;

    public Level loadedLevel;

    private void Awake()
    {
        main = this;
    }

    public void LoadLevel(Level level)
    {
        UnloadLevel();
        loadedLevel = level;
        for (int x = 0; x < level.width; x++)
        {
            for (int y = 0; y < level.height; y++)
            {
                if (level.wallMap[y, x] == WallType.Floor)
                {
                    wallTilemap.SetTile(new Vector3Int(x, y, 0), floorTile);
                }
                else if (level.wallMap[y, x] == WallType.Wall)
                {
                    wallTilemap.SetTile(new Vector3Int(x, y, 0), wallTile);
                }
            }
        }

        Camera.main.transform.position = new Vector3(level.width / 2f, level.height / 2f, -10f);

        foreach (Possessable poss in level.possessables)
        {
            if (poss is Barrel b)
            {
                Instantiate(barrelPrefab, new Vector3(b.pos.x + 0.5f, b.pos.y + 0.5f, 0), Quaternion.identity);
            }
        }
    }

    public void UnloadLevel()
    {
        loadedLevel = null;
        wallTilemap.ClearAllTiles();
    }
}
