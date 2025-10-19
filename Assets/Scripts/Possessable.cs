using UnityEngine;

public abstract class Possessable
{
    Vector2Int pos;

    public virtual bool CollidesWith(Vector2Int pos)
    {
        return this.pos == pos;
    }
}

public class Barrel : Possessable
{
    public Vector2Int pos;
    public Barrel(Vector2Int pos)
    {
        this.pos = pos;
    }
}
