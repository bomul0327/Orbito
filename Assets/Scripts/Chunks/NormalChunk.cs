using UnityEngine;

public class NormalChunk : IChunk
{
    public Vector3 Position{get; private set;}
    public NormalChunk(Vector3 position)
    {
        Position = position;
    }
    public void Spawn ()
    {
        return;
    }
}
