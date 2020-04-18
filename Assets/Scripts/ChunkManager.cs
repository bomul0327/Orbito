using UnityEngine;
using System;
using System.Collections.Generic;

public class ChunkManager : Singleton<ChunkManager>, IUpdatable
{
    public int Width = 100;
    public int Height = 60;
    private Rigidbody2D RB2D;
    private List<Chunk> ChunkList;
    private List<Int32> RemoveList; 
    /// <summary>
    /// 초기설정
    /// </summary>
    void Start()
    {
        RB2D = GetComponent<Rigidbody2D>();

        ChunkList = new List<Chunk>();
        ChunkList.Add(new Chunk(new Vector3(-Width, Height  )));
        ChunkList.Add(new Chunk(new Vector3(-Width, 0       )));
        ChunkList.Add(new Chunk(new Vector3(-Width, -Height )));
        ChunkList.Add(new Chunk(new Vector3(0,      Height  )));
        ChunkList.Add(new Chunk(new Vector3(0,      0       )));
        ChunkList.Add(new Chunk(new Vector3(0,      -Height )));
        ChunkList.Add(new Chunk(new Vector3(Width,  Height  )));
        ChunkList.Add(new Chunk(new Vector3(Width,  0       )));
        ChunkList.Add(new Chunk(new Vector3(Width,  -Height )));


        UpdateManager.instance.AddUpdatable(this);
    }

    public void OnUpdate(float dt)
    {
        RB2D.MovePosition(Camera.main.transform.position);
 
        for(int i =0 ; i < ChunkList.Count; i++)
        {
            if (ChunkList[i].Position.x > transform.position.x + Width * 1.5f)
            {
                var prev = ChunkList[i].Position;
                ChunkList[i].Position = new Vector3(ChunkList[i].Position.x - Width*3, ChunkList[i].Position.y);
                var cur = ChunkList[i].Position;
                Debug.Log("Chunk renewal from "+ prev + " to "+ cur);
                continue;
            }

            if (ChunkList[i].Position.x < transform.position.x - Width * 1.5f)
            {
                var prev = ChunkList[i].Position;
                ChunkList[i].Position = new Vector3(ChunkList[i].Position.x + Width*3, ChunkList[i].Position.y);
                var cur = ChunkList[i].Position;
                Debug.Log("Chunk renewal from "+ prev + " to "+ cur);
                continue;
            }

            if (ChunkList[i].Position.y > transform.position.y + Height * 1.5f)
            {
                var prev = ChunkList[i].Position;
                ChunkList[i].Position = new Vector3(ChunkList[i].Position.x, ChunkList[i].Position.y - Height*3);
                var cur = ChunkList[i].Position;
                Debug.Log("Chunk renewal from "+ prev + " to "+ cur);
                continue;
            }

            if (ChunkList[i].Position.y < transform.position.y - Height * 1.5f)
            {
                var prev = ChunkList[i].Position;
                ChunkList[i].Position = new Vector3(ChunkList[i].Position.x, ChunkList[i].Position.y + Height*3);
                var cur = ChunkList[i].Position;
                Debug.Log("Chunk renewal from "+ prev + " to "+ cur);
                continue;
            }
        }

        // 시험용 인풋 테스트입니다.
        // if (Input.GetKey(KeyCode.UpArrow))
        // {
        //     Camera.main.transform.Translate(new Vector3(0f, dt*10, 0f));
        // }
        // if (Input.GetKey(KeyCode.RightArrow))
        // {
        //     Camera.main.transform.Translate(new Vector3(dt*10, 0f, 0f));
        // }
        // if (Input.GetKey(KeyCode.DownArrow))
        // {
        //     Camera.main.transform.Translate(new Vector3(0f, -dt*10, 0f));
        // }
        // if (Input.GetKey(KeyCode.LeftArrow))
        // {
        //     Camera.main.transform.Translate(new Vector3(-dt*10, 0f, 0f));
        // }
    }
}