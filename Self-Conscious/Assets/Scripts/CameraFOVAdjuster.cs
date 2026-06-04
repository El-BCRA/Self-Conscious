using UnityEngine;

public class CameraFOVAdjuster : MonoBehaviour
{
    [SerializeField] private Camera cameraToAdjust;
    private void Awake()
    {
        if (Screen.currentResolution.width /  Screen.currentResolution.height == 16 / 10)
        {
            cameraToAdjust.fieldOfView = 44;
        } else
        {
            cameraToAdjust.fieldOfView = 40;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
