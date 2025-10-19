using UnityEngine;

// Models the level, objects in it, and interactions between those objects

public class Level
{
    public Level(string json)
    {

    }
}

[System.Serializable]
public struct LevelJSON
{
    public int fireRequirement;
    public string[] walls;
    public string[] possessables;
}
