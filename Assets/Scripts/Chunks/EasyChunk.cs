using UnityEngine;

public class EasyChunk : IChunk
{
    public Vector3 Position{get; private set;}
    public EasyChunk(Vector3 position)
    {
        Position = position;
    }
    public void Spawn ()
    {
        return;
    }
}
