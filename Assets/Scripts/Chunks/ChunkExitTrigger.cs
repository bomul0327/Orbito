using UnityEngine;

public class ChunkExitTrigger : MonoBehaviour
{
    void OnTriggerExit2D(Collider2D chunkReturn)
    {
        if (chunkReturn.gameObject.layer != LayerMask.NameToLayer("Chunk"))
        {
            return;
        }
        UnityObjectPool.GetOrCreate(ChunkManager.ChunkPoolName).Return(chunkReturn.gameObject.GetComponent<PooledUnityObject>());
    }
}