using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

public interface ILevelObjectPresenter
{
    void DestroySelf();
}

public interface IPossessablePresenter : ILevelObjectPresenter
{
    void Possess();
    void Depossess();
}

public class PresentationManager : MonoBehaviour
{
    public static PresentationManager main;

    public Tilemap wallTilemap;
    public Tile wallTile;
    public Tile floorTile;

    public PlayerPresenter playerPresenter;
    public Dictionary<LevelObject, ILevelObjectPresenter> presenters;

    public GameObject barrelPrefab;
    public GameObject candlesPrefab;
    public GameObject firePrefab;

    public Level loadedLevel;

    private void Awake()
    {
        main = this;

        presenters = new Dictionary<LevelObject, ILevelObjectPresenter>();
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
                    wallTilemap.SetTile(new Vector3Int(x, -y-1, 0), floorTile);
                }
                else if (level.wallMap[y, x] == WallType.Wall)
                {
                    wallTilemap.SetTile(new Vector3Int(x, -y-1, 0), wallTile);
                }
            }
        }

        foreach (LevelObject obj in level.levelObjects)
        {
            if (obj is Barrel b)
            {
                BarrelPresenter barrelPresenter;
                if (b.rollsVertically)
                {
                    barrelPresenter = Instantiate(barrelPrefab, GetWorldCoords(b.pos), Quaternion.Euler(new Vector3(0f, 0f, 90f))).GetComponent<BarrelPresenter>();
                } else
                {
                    barrelPresenter = Instantiate(barrelPrefab, GetWorldCoords(b.pos), Quaternion.identity).GetComponent<BarrelPresenter>();
                }
                barrelPresenter.barrel = b;
                presenters.Add(b, barrelPresenter);
            } else if (obj is Candles c)
            {
                CandlesPresenter candlesPresenter = Instantiate(candlesPrefab, GetWorldCoords(c.pos), Quaternion.identity).GetComponent<CandlesPresenter>();
                
                candlesPresenter.candles = c;
                presenters.Add(c, candlesPresenter);
                if (!c.isLit)
                {
                    candlesPresenter.Extinguish();
                }
            }
        }

        if (presenters[level.possesedObject] is IPossessablePresenter pp)
        {
            pp.Possess();
        }

        Camera.main.transform.position = new Vector3(level.width / 2f, -level.height / 2f, -10f);

        playerPresenter.transform.position = GetWorldCoords(level.possesedObject.pos);
    }

    public void UnloadLevel()
    {
        loadedLevel = null;
        wallTilemap.ClearAllTiles();

        foreach (ILevelObjectPresenter presenter in presenters.Values)
        {
            presenter.DestroySelf();
        }

        presenters.Clear();
    }

    public Vector3 GetWorldCoords(Vector2Int pos)
    {
        return new Vector3(pos.x + 0.5f, -pos.y - 0.5f, 0f);
    }

    public async Task PresentEvents(List<LevelEvent> events)
    {
        for (int i = 0; i < events.Count; i++)
        {
            if (events[i] is BarrelRoll br)
            {
                if (presenters[br.barrel] is BarrelPresenter bp)
                {
                    await bp.Roll(br);
                }
            } else if (events[i] is Possession p)
            {
                if (presenters[p.depossessed] is IPossessablePresenter pp2)
                {
                    pp2.Depossess();
                }
                await playerPresenter.Possess(p);
                if (presenters[p.possessed] is IPossessablePresenter pp1)
                {
                    pp1.Possess();
                }
            } else if (events[i] is FireStart f)
            {
                FirePresenter firePresenter = Instantiate(firePrefab, GetWorldCoords(f.fire.pos), Quaternion.identity).GetComponent<FirePresenter>();
                presenters.Add(f.fire, firePresenter);
            } else if (events[i] is Extingish e)
            {
                if(presenters[e.candles] is CandlesPresenter cp)
                {
                    cp.Extinguish();
                }
            }
        }
    }
}
