using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class UIEnergy : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI procentage;
    [SerializeField] private GameObject player;

    private EnergyBehaviour energyBehaviour;

    [SerializeField] private Image[] bars;

    void Start()
    {
        energyBehaviour = player.GetComponent<PlayerBehaviour>()._energy;
    }

    void Update()
    {
        procentage.text = "Energy: " + Mathf.Round(energyBehaviour._currentEnergy).ToString() + "%";
        UpdateBars();
    }

    public void UpdateBars()
    {
        switch (energyBehaviour._usesOfEnergy)
        {
            case 0:
                bars[0].enabled = false;
                bars[1].enabled = false;
                bars[2].enabled = false;
                bars[3].enabled = false;
                break;

            case 1:
                bars[0].enabled = true;
                bars[1].enabled = false;
                bars[2].enabled = false;
                bars[3].enabled = false;
                break;

            case 2:
                bars[0].enabled = true;
                bars[1].enabled = true;
                bars[2].enabled = false;
                bars[3].enabled = false;
                break;
            case 3:
                bars[0].enabled = true;
                bars[1].enabled = true;
                bars[2].enabled = true;
                bars[3].enabled = false;
                break;

            case 4:
                bars[0].enabled = true;
                bars[1].enabled = true;
                bars[2].enabled = true;
                bars[3].enabled = true;
                break;
        }
    }
}
