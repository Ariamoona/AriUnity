using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

public class ObjectSpawner : MonoBehaviour
{
    [Header("Settings")]
    public GameObject objectPrefab;
    public int spawnCount = 1000;
    public float radius = 10f;
    public float circleRadius = 5f;
    public float speed = 2f;

    [Header("Performance")]
    public bool useJobs = true;
    public bool logPerformance = true;

    private TransformAccessArray transformAccessArray;
    private JobHandle jobHandle;
    private float[] frameTimes;
    private int frameIndex = 0;

    void Start()
    {
        SpawnObjects();
        frameTimes = new float[60];
    }

    void SpawnObjects()
    {
        Transform[] transforms = new Transform[spawnCount];

        for (int i = 0; i < spawnCount; i++)
        {
            float angle = (i / (float)spawnCount) * 2 * Mathf.PI;
            Vector3 pos = transform.position;
            pos.x += radius * Mathf.Cos(angle);
            pos.z += radius * Mathf.Sin(angle);

            GameObject obj = Instantiate(objectPrefab, pos, Quaternion.identity);
            obj.transform.SetParent(transform);
            transforms[i] = obj.transform;
        }

        transformAccessArray = new TransformAccessArray(transforms);
    }

    void Update()
    {
        if (useJobs && transformAccessArray.isCreated)
        {
            var job = new RotatingCircleJob
            {
                deltaTime = Time.deltaTime,
                radius = circleRadius,
                speed = speed,
                center = transform.position 
            };

            jobHandle = job.Schedule(transformAccessArray);
            jobHandle.Complete();
        }
        else if (!useJobs)
        {
            for (int i = 0; i < transformAccessArray.length; i++)
            {
                if (transformAccessArray[i] == null) continue;

                Transform t = transformAccessArray[i];
                Vector3 relativePos = t.position - transform.position;
                float angle = Mathf.Atan2(relativePos.z, relativePos.x);
                angle += speed * Time.deltaTime;

                Vector3 newPos = transform.position;
                newPos.x += circleRadius * Mathf.Cos(angle);
                newPos.z += circleRadius * Mathf.Sin(angle);

                t.position = newPos;
                t.rotation = Quaternion.LookRotation(
                    new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)));
            }
        }

        if (logPerformance)
            LogPerformanceData();
    }

    void LogPerformanceData()
    {
        frameTimes[frameIndex % frameTimes.Length] = Time.unscaledDeltaTime * 1000f;
        frameIndex++;

        if (frameIndex % 60 == 0)
        {
            float avg = 0;
            float max = 0;
            float min = float.MaxValue;
            int validFrames = 0;

            for (int i = 0; i < frameTimes.Length; i++)
            {
                float t = frameTimes[i];
                if (t > 0)
                {
                    avg += t;
                    if (t > max) max = t;
                    if (t < min) min = t;
                    validFrames++;
                }
            }

            if (validFrames > 0)
            {
                avg /= validFrames;

                Debug.Log($"=== PERFORMANCE ({spawnCount} objects) ===\n" +
                         $"Mode: {(useJobs ? "IJobParallelForTransform" : "MonoBehaviour Update")}\n" +
                         $"FPS: {Mathf.RoundToInt(1000f / avg)}\n" +
                         $"Frame Time: avg={avg:F2}ms, min={min:F2}ms, max={max:F2}ms");
            }
        }
    }

    void OnDestroy()
    {
        jobHandle.Complete();
        if (transformAccessArray.isCreated)
            transformAccessArray.Dispose();
    }
}