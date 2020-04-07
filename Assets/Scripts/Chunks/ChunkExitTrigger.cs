using UnityEngine;

public class ChunkExitTrigger : MonoBehaviour
{
    UnityObjectPool Pool;
    void OnTriggerExit2D(Collider2D chunkReturn)
    {
        if (chunkReturn.gameObject.layer != LayerMask.NameToLayer("Chunk"))
        {
            return;
        }
         UnityObjectPool.GetOrCreate(ChunkManager.chunkPrefabPath).Return(chunkReturn.gameObject.GetComponent<PooledUnityObject>());
    }
}