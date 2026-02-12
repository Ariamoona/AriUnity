using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

public class ECSCounterDisplaySimple : MonoBehaviour
{
    public Text CounterText;

    void Update()
    {
        var world = World.DefaultGameObjectInjectionWorld;
        if (world == null || !world.IsCreated)
            return;

        var entityManager = world.EntityManager;

        using (var query = entityManager.CreateEntityQuery(
            ComponentType.ReadOnly<FrameCounter>()
        ))
        {
            if (!query.IsEmptyIgnoreFilter)
            {
                var counter = query.GetSingleton<FrameCounter>();

                if (CounterText != null)
                    CounterText.text = $"Frame: {counter.Value}";
            }
        } 
    }
}