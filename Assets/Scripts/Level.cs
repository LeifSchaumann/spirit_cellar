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
    public List<Possessable> possessables;

    public Level(TextAsset textFile)
    {
        LevelData data = JsonUtility.FromJson<LevelData>(textFile.text);
        width = data.walls[0].Length;
        height = data.walls.Length;
        wallMap = new WallType[width, height];
        possessables = new List<Possessable>();
        fireRequirement = data.fireRequirement;

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

                if (data.possessables[y][x] == 'B')
                {
                    possessables.Add(new Barrel(new Vector2Int(x, y)));
                }
            }
        }
    }
}

[System.Serializable]
public struct LevelData
{
    public int fireRequirement;
    public string[] walls;
    public string[] possessables;
}
