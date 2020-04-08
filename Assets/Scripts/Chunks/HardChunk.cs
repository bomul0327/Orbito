using UnityEngine;

public class HardChunk : IChunk
{
    public Vector3 Position{get; private set;}
    public HardChunk(Vector3 position)
    {
        Position = position;
    }
    public void Spawn ()
    {
        return;
    }
}
