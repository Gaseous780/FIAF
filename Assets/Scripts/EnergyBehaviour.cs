using UnityEngine;

public class EnergyBehaviour : MonoBehaviour
{
    private float energyMax;
    private float energyMin;

    [SerializeField] private float currentEnergy;

    [SerializeField] private float reductionEnergySpeed;

    private int usesOfEnergy;
    private int maxUsesOfEnergy;

    public float _currentEnergy => currentEnergy;

    public void Init()
    {
        energyMax = 100;
        energyMin = 0;

        reductionEnergySpeed = 0.15f;

        usesOfEnergy = 1;

        maxUsesOfEnergy = 4;

        currentEnergy = energyMax;
    }

    public void UpdateEnergy()
    {
        if (currentEnergy > energyMin)
        {
            currentEnergy -= (reductionEnergySpeed * usesOfEnergy) * Time.deltaTime;
        }
    }

    public void IncreaseUsesOfEnergy()
    {
        if (usesOfEnergy <= maxUsesOfEnergy)
        {
            usesOfEnergy++;
        }
    }

    public void DecreaseUsesOfEnergy()
    {
        if (usesOfEnergy > 1)
        {
            usesOfEnergy--;
        }
    }

    public void ResetUsesEnergy()
    {
        usesOfEnergy = 0;
    }
}
