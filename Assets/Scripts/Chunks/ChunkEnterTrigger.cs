using UnityEngine;

public class ChunkEnterTrigger : MonoBehaviour
{
    int ChunkWidth = MapManager.ChunkWidth;
    int ChunkHeight = MapManager.ChunkHeight;
    Vector3 chunkExitColliderSize;
    UnityObjectPool ChunkPool;
    void Start()
    {
        ChunkWidth = MapManager.ChunkWidth;
        ChunkHeight = MapManager.ChunkHeight;
        chunkExitColliderSize = new Vector3(ChunkWidth*2 + 10, ChunkHeight * 2 + 10);
        ChunkPool = UnityObjectPool.GetOrCreate(MapManager.chunkPrefabPath);
    }
    void OnTriggerEnter2D(Collider2D newCenterOfChunk)
    {
        if (newCenterOfChunk.gameObject.layer != LayerMask.NameToLayer("Chunk"))
        {
            return;
        }
        Vector3 newCenterPos = newCenterOfChunk.transform.position;
        Vector3[] newChunksPos =
        {
            newCenterPos + new Vector3(-ChunkWidth, ChunkHeight),
            newCenterPos + new Vector3(-ChunkWidth, 0),
            newCenterPos + new Vector3(-ChunkWidth, -ChunkHeight),
            newCenterPos + new Vector3(0,           ChunkHeight),
            newCenterPos + new Vector3(0,           -ChunkHeight),
            newCenterPos + new Vector3(ChunkWidth,  ChunkHeight),
            newCenterPos + new Vector3(ChunkWidth,  0),
            newCenterPos + new Vector3(ChunkWidth,  -ChunkHeight)
        };

        RaycastHit2D[] Checker;
        Checker = Physics2D.BoxCastAll(newCenterPos, chunkExitColliderSize, 0f, Vector2.zero, 0f, MapManager.chunkLayerMask);
        foreach (var c in Checker)
        {
            for (int i =0; i < newChunksPos.Length ; i++)
            {
                if (newChunksPos[i] == c.collider.transform.position)
                {
                    newChunksPos[i] = Vector3.zero;
                }
            }
        }
        foreach (var c in newChunksPos)
        {
            if (c != Vector3.zero)
            {
                ChunkPool.Instantiate(c, Quaternion.identity);
            }
        }
    }
}