using System;
using UnityEngine;

/// <summary>
/// Chunk 인터페이스 입니다.
/// </summary>
public abstract class IChunk : MonoBehaviour
{
    public Vector3 LocalPos (Vector3 pos)
    {
        return pos - transform.position + 
                new Vector3(ChunkManager.ChunkWidth*1/2, ChunkManager.ChunkHeight*1/2, 0);
    }
    public Vector3 GlobalPos (Vector3 pos)
    {
        return pos + transform.position - 
                new Vector3(ChunkManager.ChunkWidth*1/2, ChunkManager.ChunkHeight*1/2, 0);
    }

    public abstract void Spawn ();
}