using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.UI;

public class ECSStatsDisplay : MonoBehaviour
{
    [Header("UI References")]
    public Text EntityCountText;
    public Text FpsText;
    public Text JobTimeText;
    public Text MovementParamsText;

    [Header("Settings Display")]
    public float BaseSpeed = 2f;
    public float BaseRadius = 5f;
    public int GridWidth = 224;
    public int GridHeight = 224;

    private EntityManager entityManager;
    private World world;
    private float fpsUpdateInterval = 0.5f;
    private float fpsTimer;
    private int frameCount;
    private float fps;

    void Start()
    {
        world = World.DefaultGameObjectInjectionWorld;
        entityManager = world.EntityManager;
    }

    void Update()
    {
        fpsTimer += Time.deltaTime;
        frameCount++;

        if (fpsTimer >= fpsUpdateInterval)
        {
            fps = frameCount / fpsTimer;
            frameCount = 0;
            fpsTimer = 0f;
        }

        EntityQuery query = entityManager.CreateEntityQuery(
            ComponentType.ReadOnly<LocalTransform>(),
            ComponentType.ReadOnly<MoveSpeed>()
        );

        int entityCount = query.CalculateEntityCount();

        if (EntityCountText != null)
            EntityCountText.text = $"Армия: {entityCount:N0} единиц";

        if (FpsText != null)
            FpsText.text = $"FPS: {fps:F1} ({(1000f / fps):F1} ms)";

        if (MovementParamsText != null)
        {
            MovementParamsText.text = $"Параметры движения:\n" +
                                     $"Скорость: {BaseSpeed:F1}\n" +
                                     $"Радиус: {BaseRadius:F1}\n" +
                                     $"Сетка: {GridWidth} x {GridHeight}";
        }

        query.Dispose();
    }

    private void OnDestroy()
    {
    }
}