using UnityEngine;
using System;
using System.Collections.Generic;

public class ChunkManager : Singleton<ChunkManager>, IUpdatable
{
    public float Width;
    public float Height;
    public float ChunkColumnVaildBoundary;
    public float ChunkRowVaildBoundary;
    // ColumnSize와 RowSize는 2이상이어야 합니다.
    public float ChunkColumnSize;
    public float ChunkRowSize;
    private List<Chunk> ChunkList;
    /// <summary>
    /// 초기설정
    /// </summary>
    void Start()
    {
        // 임의의 값들입니다.
        Width = 100;
        Height = 60;
        ChunkColumnSize = 4;
        ChunkRowSize = 4;


        ChunkColumnVaildBoundary = Width * (ChunkColumnSize/2);
        ChunkRowVaildBoundary = Height * (ChunkRowSize/2);

        ChunkList = new List<Chunk>();
        for (int i = 0; i < ChunkColumnSize; i++)
        {
            for (int j = 0; j < ChunkRowSize; j++)
            {
                Debug.Log(new Vector3(Width*(i-((ChunkColumnSize-1)/2)), Height*(j-((ChunkRowSize-1)/2))));
                ChunkList.Add(new Chunk(new Vector3(Width*(i-((ChunkColumnSize-1)/2)), Height*(j-((ChunkRowSize-1)/2)))));
            }
        }

        UpdateManager.Instance.AddUpdatable(this);
    }

    public void OnUpdate(float dt)
    {
        transform.position = Camera.main.transform.position;
 
        for(int i =0 ; i < ChunkList.Count; i++)
        {
            if (ChunkList[i].Position.x > transform.position.x + ChunkColumnVaildBoundary)
            {
                var prev = ChunkList[i].Position;
                ChunkList[i].Position = new Vector3(ChunkList[i].Position.x - Width * ChunkColumnSize, ChunkList[i].Position.y);
                var cur = ChunkList[i].Position;
                continue;
            }

            if (ChunkList[i].Position.x < transform.position.x - ChunkColumnVaildBoundary)
            {
                var prev = ChunkList[i].Position;
                ChunkList[i].Position = new Vector3(ChunkList[i].Position.x + Width * ChunkColumnSize, ChunkList[i].Position.y);
                var cur = ChunkList[i].Position;
                continue;
            }

            if (ChunkList[i].Position.y > transform.position.y + ChunkRowVaildBoundary)
            {
                var prev = ChunkList[i].Position;
                ChunkList[i].Position = new Vector3(ChunkList[i].Position.x, ChunkList[i].Position.y - Height * ChunkRowSize);
                var cur = ChunkList[i].Position;
                continue;
            }

            if (ChunkList[i].Position.y < transform.position.y - ChunkRowVaildBoundary)
            {
                var prev = ChunkList[i].Position;
                ChunkList[i].Position = new Vector3(ChunkList[i].Position.x, ChunkList[i].Position.y + Height * ChunkRowSize);
                var cur = ChunkList[i].Position;
                continue;
            }
        }

        // 시험용 인풋 테스트입니다.
        // if (Input.GetKey(KeyCode.UpArrow))
        // {
        //     Camera.main.transform.Translate(new Vector3(0f, dt*20, 0f));
        // }
        // if (Input.GetKey(KeyCode.RightArrow))
        // {
        //     Camera.main.transform.Translate(new Vector3(dt*20, 0f, 0f));
        // }
        // if (Input.GetKey(KeyCode.DownArrow))
        // {
        //     Camera.main.transform.Translate(new Vector3(0f, -dt*20, 0f));
        // }
        // if (Input.GetKey(KeyCode.LeftArrow))
        // {
        //     Camera.main.transform.Translate(new Vector3(-dt*20, 0f, 0f));
        // }
        // if (Input.GetKey(KeyCode.S))
        // {
        //     foreach(var c in ChunkList)
        //     {
        //         c.Spawn();
        //     }
        // }
        // if (Input.GetKey(KeyCode.R))
        // {
        //     foreach(var c in ChunkList)
        //     {
        //         c.Reset();
        //     }
        // }
    }
}