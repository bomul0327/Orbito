using UnityEngine;

public class ChunkManager : Singleton<ChunkManager>, IUpdatable
{
    public static int Width = 100;
    public static int Height = 60;
    public static string ChunkPoolName= "ChunkSpawner";
    public static float MaxDifficulty = 2.9f;
    public static int Seed;
    private Rigidbody2D rb2D;
    /// <summary>
    /// 초기설정
    /// </summary>
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        // Exit Check와 Enter Check 사이에 여유공간을 두기 위해 10을 뺐습니다.
        GetComponentsInChildren<BoxCollider2D>()[0].size = new Vector2(Width*2 + 10, Height*2 + 10);
        GetComponentsInChildren<BoxCollider2D>()[1].size = new Vector2(1, 1);

        Resources.Load<GameObject>(ChunkPoolName).GetComponent<BoxCollider2D>().size = new Vector2(Width, Height);

        UnityObjectPool.GetOrCreate(ChunkManager.ChunkPoolName).Instantiate(new Vector3(0, 0, 0), Quaternion.identity);

        UpdateManager.instance.AddUpdatable(this);
    }

    public void OnUpdate(float dt)
    {
        rb2D.MovePosition(Camera.main.transform.position);

        // 시험용 인풋 테스트입니다.
        if (Input.GetKey(KeyCode.UpArrow))
        {
            Camera.main.transform.Translate(new Vector3(0f, dt*10, 0f));
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            Camera.main.transform.Translate(new Vector3(dt*10, 0f, 0f));
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            Camera.main.transform.Translate(new Vector3(0f, -dt*10, 0f));
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Camera.main.transform.Translate(new Vector3(-dt*10, 0f, 0f));
        }
    }

    /// <summary>
    /// Global Position을 Chunk의 Local Position으로 변환해줍니다.
    /// </summary>
    public static Vector3 LocalPos (Vector3 globalPos, IChunk to)
    {
        return globalPos - to.Position + 
                new Vector3(Width*1/2, Height*1/2, 0);
    }
    /// <summary>
    /// Chunk의 Local Position을 Global Position으로 변환해줍니다.
    /// </summary>
    public static Vector3 GlobalPos (Vector3 localPos, IChunk from)
    {
        return localPos + from.Position - 
                new Vector3(Width*1/2, Height*1/2, 0);
    }

}