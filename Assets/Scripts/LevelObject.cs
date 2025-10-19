using UnityEngine;


public abstract class LevelObject
{
    public Vector2Int pos;

    public virtual bool CollidesWith(Vector2Int pos)
    {
        return this.pos == pos;
    }
}

public abstract class Possessable : LevelObject { }


public class Barrel : Possessable
{
    public bool rollsVertically;

    public Barrel(Vector2Int pos, bool rollsVertically)
    {
        this.pos = pos;
        this.rollsVertically = rollsVertically;
    }
}
