using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MapManager : Singleton<MapManager>
{
    private Collider ChunkBoundaryChecker;
    void Start()
    {
        ChunkBoundaryChecker = gameObject.GetComponentInChildren<Collider>();
    }
}