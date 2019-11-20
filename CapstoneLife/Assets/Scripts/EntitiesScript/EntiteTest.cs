﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
public class EntiteTest : MonoBehaviour, IDeclareReferencedPrefabs, IConvertGameObjectToEntity
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private float spawnRate;
    [SerializeField] private float maxDistanceFromSpawner;
    
    

    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
    {
        referencedPrefabs.Add(prefab);
    }

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponentData(entity, new ValueEntitie
        {
            prefabs = conversionSystem.GetPrimaryEntity(prefab),
            maxDistanceFromSpawner = maxDistanceFromSpawner,
            secondsBetweenSpawns = 1 / spawnRate,
            secondsToNextSpawn = 0f
        }) ;
    }
}