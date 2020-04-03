using System;
using UnityEngine;

/// <summary>
/// Chunk 인터페이스 입니다.
/// </summary>
public abstract class IChunk : MonoBehaviour
{
    public Vector3 LocalPos (FieldObject obj)
    {
        return obj.transform.position - transform.position + 
                new Vector3(MapManager.ChunkWidth*1/2, MapManager.ChunkHeight*1/2, 0);
    }
    public abstract void Spawn ();
}