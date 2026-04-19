using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    [SerializeField] private float cameraSpeed;
    [SerializeField] private float maxRotation;
    [SerializeField] private float minRotation;

    private bool OnAxisNegative;

    [SerializeField] private int rotationAxis; //0 = y, 1 = x, 2 = z

    [SerializeField] private float compensation;

    private void Awake()
    {
        OnAxisNegative = false;
    }

    private void Update()
    {
        if (compensation == 0) 
        {
            CameraChecker();
        }
        else
        {
            CameraCheckerWithCompensation();
        }

        CameraMovement();
    }

    private void CameraChecker()
    {
        switch (rotationAxis)
        {
            case 0:
                if (OnAxisNegative == false)
                {
                    if (transform.localRotation.eulerAngles.y >= maxRotation)
                    {
                        OnAxisNegative = true;
                        cameraSpeed *= -1;
                    }
                }
                else
                {
                    if (transform.localRotation.eulerAngles.y <= minRotation)
                    {
                        OnAxisNegative = false;
                        cameraSpeed *= -1;
                    }
                }
                break;

            case 1:
                if (OnAxisNegative == false)
                {
                    if (transform.localRotation.eulerAngles.x >= maxRotation)
                    {
                        OnAxisNegative = true;
                        cameraSpeed *= -1;
                    }
                }
                else
                {
                    if (transform.localRotation.eulerAngles.x <= minRotation)
                    {
                        OnAxisNegative = false;
                        cameraSpeed *= -1;
                    }
                }
                break;

            case 2:
                if (OnAxisNegative == false)
                {
                    if (transform.localRotation.eulerAngles.z >= maxRotation)
                    {
                        OnAxisNegative = true;
                        cameraSpeed *= -1;
                    }
                }
                else
                {
                    if (transform.localRotation.eulerAngles.z <= minRotation)
                    {
                        OnAxisNegative = false;
                        cameraSpeed *= -1;
                    }
                }
                break;
        }
    }

    private void CameraCheckerWithCompensation()
    {
        float compensationValue = 0;

        if (OnAxisNegative == true)
        {
            compensationValue = minRotation;

            if (transform.localRotation.eulerAngles.y > 180)
            {
                compensationValue = minRotation + compensation;
            }
        }
        else
        {
            compensationValue = maxRotation;

            if (transform.localRotation.eulerAngles.y > 180)
            {
                compensationValue = maxRotation + compensation;
            }
        }

        switch (rotationAxis)
        {
            case 0:
                if (OnAxisNegative == false)
                {
                    if (transform.localRotation.eulerAngles.y >= compensationValue)
                    {
                        OnAxisNegative = true;
                        cameraSpeed *= -1;
                    }
                }
                else
                {
                    if (transform.localRotation.eulerAngles.y <= compensationValue)
                    {
                        OnAxisNegative = false;
                        cameraSpeed *= -1;
                    }
                }
                break;

            case 1:
                if (OnAxisNegative == false)
                {
                    if (transform.localRotation.eulerAngles.x >= maxRotation)
                    {
                        OnAxisNegative = true;
                        cameraSpeed *= -1;
                    }
                }
                else
                {
                    if (transform.localRotation.eulerAngles.x <= minRotation)
                    {
                        OnAxisNegative = false;
                        cameraSpeed *= -1;
                    }
                }
                break;

            case 2:
                if (OnAxisNegative == false)
                {
                    if (transform.localRotation.eulerAngles.z >= maxRotation)
                    {
                        OnAxisNegative = true;
                        cameraSpeed *= -1;
                    }
                }
                else
                {
                    if (transform.localRotation.eulerAngles.z <= minRotation)
                    {
                        OnAxisNegative = false;
                        cameraSpeed *= -1;
                    }
                }
                break;
        }
    }

    private void CameraMovement()
    {
        switch (rotationAxis)
        {
            case 0:
                transform.Rotate(0, cameraSpeed * Time.deltaTime, 0, Space.Self);
                break;

            case 1:
                transform.Rotate(cameraSpeed * Time.deltaTime,0 ,0, Space.Self);
                break;

            case 2:
                transform.Rotate(0, 0, cameraSpeed * Time.deltaTime, Space.Self);
                break;
        }
    }
}
