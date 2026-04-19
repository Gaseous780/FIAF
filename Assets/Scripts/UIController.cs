using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject UIOffice;
    [SerializeField] private GameObject UICameras;

    public void ActivateUIOffice()
    {
        UICameras.SetActive(false);
        UIOffice.SetActive(true);
    }

    public void ActivateUICamera() 
    {
        UIOffice.SetActive(false);
        UICameras.SetActive(true);
    }

    public void DesactivateAllUI()
    {
        gameObject.GetComponent<Canvas>().enabled = false;
    }

    public void ActivateNormalUI()
    {
        gameObject.GetComponent<Canvas>().enabled = true;
    }
}
