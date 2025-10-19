using UnityEngine;

public abstract class LevelEvent
{
    
}

public class BarrelRoll : LevelEvent
{
    public Barrel barrel;
    public Vector2Int dir;
    public Vector2Int startPos;
    public Vector2Int endPos;

    public BarrelRoll(Barrel barrel, Vector2Int dir, Vector2Int startPos, Vector2Int endPos)
    {
        this.barrel = barrel;
        this.dir = dir;
        this.startPos = startPos;
        this.endPos = endPos;
    }
}

public class Possession : LevelEvent
{
    public Possessable possessed;
    public Possessable depossessed;

    public Possession(Possessable possessed, Possessable depossessed)
    {
        this.possessed = possessed;
        this.depossessed = depossessed;
    }
}
