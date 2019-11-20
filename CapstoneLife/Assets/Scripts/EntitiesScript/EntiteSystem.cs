using System.Collections;
using System.Collections.Generic;
//using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Burst;
using Unity.Transforms;
using Unity.Mathematics;
using Time = UnityEngine.Time;

public class EntiteSystem : JobComponentSystem
{
    private EndSimulationEntityCommandBufferSystem endSimulationEntityCommandBufferSystem;

    protected override void OnCreate()
    {
        endSimulationEntityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();

    }

    private struct ValueEntitieJob : IJobForEachWithEntity<ValueEntitie, LocalToWorld>
    {
        private readonly float deltaTime;
        private EntityCommandBuffer.Concurrent entityCommandBuffer;
        private Random random;
        public ValueEntitieJob(EntityCommandBuffer.Concurrent buffer,Random random,float deltaTime)
        {
            this.entityCommandBuffer = buffer;
            this.random = random;
            this.deltaTime = deltaTime;
        }
        public void Execute(Entity entity, int index, ref ValueEntitie c0, ref LocalToWorld localToWorld)
        {
            c0.secondsToNextSpawn -= deltaTime;

            if(c0.secondsToNextSpawn >= 0) { return; }

            c0.secondsToNextSpawn += c0.secondsBetweenSpawns;

            Entity instance = entityCommandBuffer.Instantiate(index, c0.prefabs);
            entityCommandBuffer.SetComponent(index, instance, new Translation
            {
                Value = localToWorld.Position + random.NextFloat3Direction() * random.NextFloat() * c0.maxDistanceFromSpawner
            }) ;
        }
    }
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var spawnerJob = new ValueEntitieJob(
            endSimulationEntityCommandBufferSystem.CreateCommandBuffer().ToConcurrent(),
            new Random((uint)UnityEngine.Random.Range(0,int.MaxValue)),
            Time.deltaTime
            );

        JobHandle jobHandle = spawnerJob.Schedule(this, inputDeps);

        endSimulationEntityCommandBufferSystem.AddJobHandleForProducer(jobHandle);

        return jobHandle;
    }
}
