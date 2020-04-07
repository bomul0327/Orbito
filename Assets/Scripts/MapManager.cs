using UnityEngine;

public class MapManager : Singleton<MapManager>, IUpdatable
{
    public static int ChunkWidth = 100;
    public static int ChunkHeight = 60;
    public static LayerMask chunkLayerMask;
    public static BoxCollider2D[] ChunkBoundaryCollider;
    public static int Seed;
    public static string chunkPrefabPath = "ChunkSpawner";
    // json이 완성되면 이 멤버를 없애고 json으로 받아올 것
    [SerializeField]
    public int seed;
    public void OnAfterDeserialize()
    {
        Seed = seed;
    }
    private Rigidbody2D rb2D;
    private UnityObjectPool ChunkPool;
    /// <summary>
    /// 초기설정
    /// </summary>
    void Start()
    {
        chunkLayerMask = LayerMask.GetMask("Chunk");
    
        rb2D = GetComponent<Rigidbody2D>();
        // 추후에 json으로 받기
        // ChunkWidth = JsonManager.~~~~;
        // ChunkHeight = JsonManager.~~~~;
        ChunkBoundaryCollider = GetComponentsInChildren<BoxCollider2D>();
        ChunkBoundaryCollider[0].isTrigger = true;
        ChunkBoundaryCollider[1].isTrigger = true;
        // Boundary Check에 여유공간을 두기 위해 10을 뺐습니다.
        ChunkBoundaryCollider[0].size = new Vector2(ChunkWidth*2 + 10, ChunkHeight*2 + 10);
        ChunkBoundaryCollider[1].size = new Vector2(1, 1);

        chunkPrefabInit();

        UpdateManager.instance.AddUpdatable(this);
    }

    /// <summary>
    /// ChunkSpawnerPrefab에 관한 동작이 이루어 지는곳
    /// </summary>
    private void chunkPrefabInit()
    {
        GameObject ChunkSpawnerPrefab;
        BoxCollider2D ChunkPrefabCollider;

        ChunkSpawnerPrefab = Resources.Load<GameObject>(chunkPrefabPath);
        ChunkPrefabCollider = ChunkSpawnerPrefab.GetComponent<BoxCollider2D>();
        ChunkPrefabCollider.size = new Vector2(ChunkWidth, ChunkHeight);
        ChunkPrefabCollider.isTrigger = true;

        ChunkPool = UnityObjectPool.GetOrCreate(chunkPrefabPath);

        // ChunkPool.Instantiate(new Vector3(-ChunkWidth,  ChunkHeight,  0), Quaternion.identity);
        // ChunkPool.Instantiate(new Vector3(0,            ChunkHeight,  0), Quaternion.identity);
        // ChunkPool.Instantiate(new Vector3(ChunkWidth,   ChunkHeight,  0), Quaternion.identity);
        // ChunkPool.Instantiate(new Vector3(-ChunkWidth,  0,            0), Quaternion.identity);
        ChunkPool.Instantiate(new Vector3(0,            0,            0), Quaternion.identity);
        // ChunkPool.Instantiate(new Vector3(ChunkWidth,   0,            0), Quaternion.identity);
        // ChunkPool.Instantiate(new Vector3(-ChunkWidth,  -ChunkHeight, 0), Quaternion.identity);
        // ChunkPool.Instantiate(new Vector3(0,            -ChunkHeight, 0), Quaternion.identity);
        // ChunkPool.Instantiate(new Vector3(ChunkWidth,   -ChunkHeight, 0), Quaternion.identity);
    }
    public void OnUpdate(float dt)
    {
        rb2D.MovePosition(Camera.main.transform.position);

        // 시험용 인풋 테스트입니다.
        if (Input.GetKey(KeyCode.Q))
        {
            Camera.main.transform.Translate(new Vector3(0f, dt*10, 0f));
        }
        if (Input.GetKey(KeyCode.W))
        {
            Camera.main.transform.Translate(new Vector3(dt*10, 0f, 0f));
        }
    }

}