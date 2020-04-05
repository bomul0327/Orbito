using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkSpawner : MonoBehaviour
{
    private IChunk Chunk;
    private int seed;
    void Awake()
    {
        seed = MapManager.Seed;

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

        // 여러 chunk가 중첩된다거나 추가적으로 chunk를 만들어낸다거나 하는 상황이
        // 올지 모르겠습니다만 하나의 chunk만이 사용됨이 보장될때 Destroy함이 맞아 보입니다.
        // Destroy(this); 
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
