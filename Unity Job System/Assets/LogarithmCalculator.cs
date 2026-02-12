using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using System.Collections.Generic;

public class LogarithmCalculator : MonoBehaviour
{
    [Header("Settings")]
    public int calculationsPerSecond = 1000;
    public bool useJobs = true;
    public int objectIndex = 0;

    private ObjectSpawner spawner;
    private List<float> results = new List<float>();
    private float timer = 0f;
    private float interval;
    private NativeArray<float> jobResult;
    private JobHandle jobHandle;

    void Start()
    {
        spawner = GetComponent<ObjectSpawner>();
        interval = 1f / calculationsPerSecond;
        jobResult = new NativeArray<float>(1, Allocator.Persistent);

        InvokeRepeating("LogStatistics", 5f, 5f);
    }

    void Update()
    {
        timer += Time.deltaTime;

        while (timer >= interval)
        {
            timer -= interval;

            if (useJobs)
            {
                ScheduleLogarithmJob();
            }
            else
            {
                CalculateLogarithm();
            }
        }
    }

    void ScheduleLogarithmJob()
    {
        jobHandle.Complete();

        var job = new LogarithmJob
        {
            randomNumber = Random.Range(1f, 100f),
            result = jobResult
        };

        jobHandle = job.Schedule();

    }

    void LateUpdate()
    {
        if (useJobs)
        {
            if (jobHandle.IsCompleted)
            {
                jobHandle.Complete();
                float result = jobResult[0];

                results.Add(result);
                if (results.Count > 100) results.RemoveAt(0);
            }
        }
    }

    void CalculateLogarithm()
    {
        float random = Random.Range(1f, 100f);
        float result = Mathf.Log(random);

        for (int i = 0; i < 1000; i++)
        {
            result += Mathf.Log(random * 0.999f) * 0.0001f;
        }

        results.Add(result);
        if (results.Count > 100) results.RemoveAt(0);
    }

    void LogStatistics()
    {
        if (results.Count == 0) return;

        float sum = 0;
        foreach (var r in results)
            sum += r;

        float avg = sum / results.Count;

        Debug.Log($"=== LOGARITHM PERFORMANCE ===\n" +
                 $"Mode: {(useJobs ? "IJob" : "Main Thread")}\n" +
                 $"Calculations/sec: {calculationsPerSecond}\n" +
                 $"Average result: {avg:F4}\n" +
                 $"Samples collected: {results.Count}");
    }

    void OnDestroy()
    {
        jobHandle.Complete();
        if (jobResult.IsCreated)
            jobResult.Dispose();
    }
}