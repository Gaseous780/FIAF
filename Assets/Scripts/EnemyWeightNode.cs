using System;
using UnityEngine;

public class EnemyWeightNode : DecisionNode
{
    private (float weight, Action<EnemyBase> action)[] options;

    public EnemyWeightNode((float weight, Action<EnemyBase> action)[] options)
    {
        this.options = options;
    }

    public override void Evaluate(EnemyBase enemy, EnemyContext context)
    {
        float totalWeight = 0f;

        foreach (var option in options)
        {
            totalWeight += option.weight;
        }

        float randomValue = UnityEngine.Random.Range(0f, totalWeight);
        float currentWeight = 0f;

        foreach (var option in options)
        {
            currentWeight += option.weight;

            if (randomValue <= currentWeight)
            {
                option.action(enemy);
                return;
            }
        }
    }
}
