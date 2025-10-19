using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public enum WallType
{
    Floor,
    Wall,
    Pillar
}

// Models the level, objects in it, and interactions between those objects

public class Level
{
    public int fireRequirement;
    public int width;
    public int height;
    public WallType[,] wallMap;
    public List<LevelObject> levelObjects;
    public Possessable possesedObject;

    public Level(TextAsset textFile)
    {
        LevelData data = JsonUtility.FromJson<LevelData>(textFile.text);
        width = data.walls[0].Length;
        height = data.walls.Length;
        wallMap = new WallType[height, width];
        levelObjects = new List<LevelObject>();
        fireRequirement = data.fireRequirement;

        Vector2Int playerPos = new Vector2Int(-1, -1);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (data.walls[y][x] == ' ')
                {
                    wallMap[y, x] = WallType.Floor;
                } else
                {
                    wallMap[y, x] = WallType.Wall;
                }

                if (data.objects[y][x] == 'B')
                {
                    levelObjects.Add(new Barrel(new Vector2Int(x, y), false));
                }

                if (data.objects[y][x] == 'M')
                {
                    levelObjects.Add(new Barrel(new Vector2Int(x, y), true));
                }

                if (data.objects[y][x] == 'C')
                {
                    levelObjects.Add(new Candles(new Vector2Int(x, y), true));
                }

                if (data.objects[y][x] == 'U')
                {
                    levelObjects.Add(new Candles(new Vector2Int(x, y), false));
                }

                if (data.player[y][x] == 'P')
                {
                    playerPos = new Vector2Int(x, y);
                }
            }
        }

        if (playerPos.x == -1)
        {
            Debug.LogError("Level JSON does not give a player position!");
        }
        else if (!GetPossessable(playerPos, out possesedObject))
        {
            Debug.LogError("Level JSON does not put player above a possessable object!");
        }
    }

    private bool GetLevelObject(Vector2Int pos, out LevelObject obj)
    {
        foreach (LevelObject o in levelObjects)
        {
            if (o.CollidesWith(pos))
            {
                obj = o;
                return true;
            }
        }
        obj = null;
        return false;
    }

    private bool GetPossessable(Vector2Int pos, out Possessable poss)
    {
        if (GetLevelObject(pos, out LevelObject obj))
        {
            if (obj is Possessable)
            {
                poss = obj as Possessable;
                return true;
            }
        }
        poss = null;
        return false;
    }

    public List<LevelEvent> HandleInput(PlayerInput input)
    {
        List<LevelEvent> levelEvents = new List<LevelEvent>();

        if (input is MoveInput moveInput)
        {
            if (possesedObject is Barrel b)
            {
                if ((b.rollsVertically && (moveInput.dir == Vector2Int.up || moveInput.dir == Vector2Int.down)) || (!b.rollsVertically && (moveInput.dir == Vector2Int.left || moveInput.dir == Vector2Int.right)))
                {
                    Vector2Int endPos = MoveRaycast(b.pos, moveInput.dir);
                    if (endPos != b.pos)
                    {
                        levelEvents.Add(new BarrelRoll(b, moveInput.dir, b.pos, endPos));
                        b.pos = endPos;
                    }
                }
            } else if (possesedObject is Candles c)
            {
                if (c.isLit && !IsObstructed(c.pos + moveInput.dir))
                {
                    Fire fire = new Fire(c.pos + moveInput.dir);
                    levelEvents.Add(new FireStart(fire));
                    levelEvents.Add(new Extingish(c));
                    levelObjects.Add(fire);
                    c.isLit = false;
                }
            }
        } else if (input is PossessInput possessInput)
        {
            if (PossessRaycast(possesedObject.pos, possessInput.dir, out Possessable poss))
            {
                levelEvents.Add(new Possession(poss, possesedObject));
                possesedObject = poss;
            }
        }

        return levelEvents;
    }

    private Vector2Int MoveRaycast(Vector2Int origin, Vector2Int dir)
    {
        Vector2Int rayPos = origin;
        while (!IsObstructed(rayPos + dir))
        {
            rayPos = rayPos + dir;
        }
        return rayPos;
    }

    private bool PossessRaycast(Vector2Int origin, Vector2Int dir, out Possessable poss)
    {
        Vector2Int targetPos = MoveRaycast(origin, dir) + dir;
        
        if (GetPossessable(targetPos, out Possessable p))
        {
            poss = p;
            return true;
        } else
        {
            poss = null;
            return false;
        }
    }

    private bool InBounds(Vector2Int pos)
    {
        return !(pos.x < 0 || pos.x >= width || pos.y < 0 || pos.y >= height);
    }

    private bool IsObstructed(Vector2Int pos)
    {
        if (!InBounds(pos))
        {
            return true;
        }
        if (GetLevelObject(pos, out LevelObject obj))
        {
            return true;
        }
        if (wallMap[pos.y, pos.x] == WallType.Wall)
        {
            return true;
        }
        return false;
    }
}

[System.Serializable]
public struct LevelData
{
    public int fireRequirement;
    public string[] walls;
    public string[] objects;
    public string[] player;
}
