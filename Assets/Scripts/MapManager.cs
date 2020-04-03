using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MapManager : Singleton<MapManager>, IUpdatable
{
    public const int ChunkWidth = 100;
    public const int ChunkHeight = 60;
    public int Seed;
    private BoxCollider2D ChunkBoundaryCollider;
    /// <summary>
    /// 초기설정
    /// </summary>
    void Start()
    {
        // 추후에 json으로 받기
        // ChunkWidth = JsonManager.~~~~;
        // ChunkHeight = JsonManager.~~~~;
        ChunkBoundaryCollider = GetComponent<BoxCollider2D>();
        ChunkBoundaryCollider.isTrigger = true;
        // Boundary Check에 여유공간을 두기 위해 10을 뺐습니다.
        ChunkBoundaryCollider.size = new Vector2(ChunkWidth*2 - 10, ChunkHeight*2 - 10);

        chunkInit();

        UpdateManager.instance.AddUpdatable(this);
    }

    /// <summary>
    /// ChunkSpawnerPrefab에 관한 동작이 이루어 지는곳
    /// </summary>
    private void chunkInit()
    {
        // 추후 json으로 받기
        string chunkPrefabPath = "ChunkSpawner";
        GameObject ChunkSpawnerPrefab;
        BoxCollider2D ChunkPrefabCollider;

        ChunkSpawnerPrefab = Resources.Load<GameObject>(chunkPrefabPath);
        ChunkPrefabCollider = ChunkSpawnerPrefab.GetComponent<BoxCollider2D>();
        ChunkPrefabCollider.size = new Vector2(ChunkWidth, ChunkHeight);
        ChunkPrefabCollider.isTrigger = true;

        UnityObjectPool ChunkPool;
        ChunkPool = UnityObjectPool.GetOrCreate(chunkPrefabPath);
        ChunkPool.SetOption(PoolScaleType.Static, PoolReturnType.Manual);
        // UnityObjectPool.PoolCapacity 가 private set; 으로 설정되어 있어 지금은 불가능
        // ChunkPool.PoolCapacity = 9;

        ChunkPool.Instantiate(new Vector3(-ChunkWidth,  ChunkHeight,  0), Quaternion.identity);
        ChunkPool.Instantiate(new Vector3(0,            ChunkHeight,  0), Quaternion.identity);
        ChunkPool.Instantiate(new Vector3(ChunkWidth,   ChunkHeight,  0), Quaternion.identity);
        ChunkPool.Instantiate(new Vector3(-ChunkWidth,  0,            0), Quaternion.identity);
        ChunkPool.Instantiate(new Vector3(0,            0,            0), Quaternion.identity);
        ChunkPool.Instantiate(new Vector3(ChunkWidth,   0,            0), Quaternion.identity);
        ChunkPool.Instantiate(new Vector3(-ChunkWidth,  -ChunkHeight, 0), Quaternion.identity);
        ChunkPool.Instantiate(new Vector3(0,            -ChunkHeight, 0), Quaternion.identity);
        ChunkPool.Instantiate(new Vector3(ChunkWidth,   -ChunkHeight, 0), Quaternion.identity);
    }
    public void OnUpdate(float dt)
    {
        // 둘중에 어떤걸 쓰는게 더 좋은지 모르겠습니다.
        transform.position = Camera.main.transform.position;
        // ChunkBoundaryCollider.offset = new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y);

        // 시험용 인풋 테스트입니다.
        // if (Input.GetKey(KeyCode.Q))
        // {
        //     Camera.main.transform.Translate(new Vector3(0f, dt*5, 0f));
        // }
        // if (Input.GetKeyDown(KeyCode.W))
        // {
        //     Camera.main.transform.Translate(new Vector3(dt*5, 0f, 0f));
        // }
    }

    private void OnTriggerExit(Collider chunkReturn)
    {
        UnityObjectPool.GetOrCreate("ChunkSpawner").Return(chunkReturn.gameObject.GetComponent<PooledUnityObject>());
    }
    
}