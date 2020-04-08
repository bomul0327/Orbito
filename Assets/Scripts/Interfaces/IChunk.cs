using System;
using UnityEngine;

/// <summary>
/// Chunk 인터페이스 입니다.
/// </summary>
public interface IChunk
{
    Vector3 Position{get;}
    void Spawn ();
}