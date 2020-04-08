using UnityEngine;

public class ChunkSpawner : MonoBehaviour
{
    private IChunk chunk;
    void Awake()
    {
        Random.InitState(ChunkManager.Seed);
        // 여기 중요합니다. 어떤 청크가 생성될지 거리에 비례해서 만드는 식인데 적절한 식이 필요
        int value = chunkSelectionEquation(Random.Range(0f , ChunkManager.MaxDifficulty));

        switch (value)
        {
            case 0 :
                chunk = new EasyChunk(transform.position);
                break;
            case 1 :
                chunk = new NormalChunk(transform.position);
                break;
            case 2 :
                chunk = new HardChunk(transform.position);
                break;
        }

        chunk.Spawn();

    }

    const int MapSize = 1000; 
    private int chunkSelectionEquation(float x)
    {
        int value;
        value = (int)(((Vector2)transform.position).magnitude/MapSize * x);
        return value;
    }
}
