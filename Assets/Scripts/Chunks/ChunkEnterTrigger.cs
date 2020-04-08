using UnityEngine;

public class ChunkEnterTrigger : MonoBehaviour
{
    static Vector3 checkerSize = new Vector3(ChunkManager.Width + 10, ChunkManager.Height + 10);
    static Vector3[] checkingPos = 
        {
            new Vector3(-ChunkManager.Width, ChunkManager.Height ),
            new Vector3(-ChunkManager.Width, 0                   ),
            new Vector3(-ChunkManager.Width, -ChunkManager.Height),
            new Vector3(0,                   ChunkManager.Height ),
            new Vector3(0,                   -ChunkManager.Height),
            new Vector3(ChunkManager.Width,  ChunkManager.Height ),
            new Vector3(ChunkManager.Width,  0                   ),
            new Vector3(ChunkManager.Width,  -ChunkManager.Height)
        };
    Vector3[] newChunks = new Vector3[8];

    void Awake()
    {
        checkingPos.CopyTo(newChunks, 0);
    }
    
    void OnTriggerEnter2D(Collider2D centerChunk)
    {
        Debug.Log(centerChunk.name);
        if (centerChunk.gameObject.layer != LayerMask.NameToLayer("Chunk"))
        {
            return;
        }

        RaycastHit2D[] checker;
        checker = Physics2D.BoxCastAll(centerChunk.transform.position, checkerSize, 0f, Vector2.zero, 0f, LayerMask.GetMask("Chunk"));

        foreach (var c in checker)
        {
            for (int i = 0 ; i < checkingPos.Length; i++)
            {
                if (checkingPos[i] == c.collider.transform.position - centerChunk.transform.position)
                {
                    newChunks[i] = Vector3.zero;
                }
            }
        }

        foreach (var c in newChunks)
        {
            Debug.Log(c);
            if (c != Vector3.zero)
            {
                Debug.Log(c);
                UnityObjectPool.GetOrCreate(ChunkManager.ChunkPoolName).Instantiate(c + centerChunk.transform.position, Quaternion.identity);
            }
        }
        checkingPos.CopyTo(newChunks, 0);
    }
}