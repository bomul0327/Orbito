using UnityEngine;

public class ChunkSpawner : MonoBehaviour
{
    private IChunk Chunk;
    private int seed;
    void Awake()
    {
        seed = ChunkManager.Seed;

        Random.InitState(seed);
        // 여기 중요합니다. 어떤 청크가 생성될지 거리에 비례해서 만드는 식인데 적절한 식이 필요
        int value = chunkSelectionEquation(Random.Range(0f , ChunkMaxDifficulty));

        switch (value)
        {
            case 0 :
                Chunk = gameObject.AddComponent<EasyChunk>();
                break;
            case 1 :
                Chunk = gameObject.AddComponent<NormalChunk>();
                break;
            case 2 :
                Chunk = gameObject.AddComponent<HardChunk>();
                break;
        }

        Chunk.Spawn();

    }

    void OnDisable()
    {
        Destroy(GetComponent<IChunk>());
    }

    static float ChunkMaxDifficulty = 2.9f;
    const int ChunkMaxDistance = 1000; 
    private int chunkSelectionEquation(float x)
    {
        int value;
        value = (int)(((Vector2)transform.position).magnitude/ChunkMaxDistance * x);
        return value;
    }
}
