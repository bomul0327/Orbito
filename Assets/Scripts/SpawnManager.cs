using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : Singleton<SpawnManager>
{
    private void Awake()
    {
        SpawnPlayer();
        SpawnPlanets(100, 15f);
    }

    public void SpawnPlayer()
    {
        var playerObjectPool = UnityObjectPool.GetOrCreate("Player");
        playerObjectPool.SetOption(PoolScaleType.Static, PoolReturnType.Manual);

        SpawnAt("Player", Vector2.zero, Quaternion.identity);
    }

    public void SpawnPlanets(int spawnCount, float spawnRange)
    {
        var planetObjectPool = UnityObjectPool.GetOrCreate("Planet");
        planetObjectPool.SetOption(PoolScaleType.Limited, PoolReturnType.Manual);
        planetObjectPool.MaxPoolCapacity = 1000;

        SpawnInCircle("Planet", spawnCount, Vector2.zero, radius: spawnRange);
    }


    /// <summary>
    /// Spawn objects at random positions in a circle-shaped area.
    /// </summary>
    /// <param name="poolToSpawn">Pool name to spawn object.</param>
    /// <param name="spawnCount">Number of object to spawn.</param>
    /// <param name="center">Center position of circle area.</param>
    /// <param name="radius">Radius of circle area.</param>
    public void SpawnInCircle(string poolToSpawn, int spawnCount, Vector2 center, float radius)
    {
        var unityObjectPool = UnityObjectPool.GetOrCreate(poolToSpawn);

        for (int i = 0; i < spawnCount; i++)
        {

            var position = center + Random.insideUnitCircle * radius;
            var rotation = Quaternion.identity;

            var instance = unityObjectPool.Instantiate(position, rotation);
        }
    }

    /// <summary>
    /// Spawn an object at given position and rotation.
    /// </summary>
    /// <param name="poolToSpawn">Pool name to spawn object.</param>
    public void SpawnAt(string poolToSpawn, Vector2 position, Quaternion rotation)
    {
        var unityObjectPool = UnityObjectPool.GetOrCreate(poolToSpawn);
        var instance = unityObjectPool.Instantiate(position, rotation);

    }

}
