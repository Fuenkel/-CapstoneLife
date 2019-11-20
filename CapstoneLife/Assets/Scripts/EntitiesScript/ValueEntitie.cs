using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
public struct ValueEntitie : IComponentData
{
    public Entity prefabs;
    public float maxDistanceFromSpawner;
    public float secondsBetweenSpawns;
    public float secondsToNextSpawn;
}
