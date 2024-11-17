using UnityEngine;

public class BackgroundScript : MonoBehaviour
{
    public Transform mainCam;
    public Transform middleBackground;
    public Transform sidesBackground;

    public float length = 38.4f;

    private void Update()
    {
        // calculates the new positions for the side backgrounds
        Vector3 newLeftPosition = middleBackground.position + Vector3.left * length;
        Vector3 newRightPosition = middleBackground.position + Vector3.right * length;

        // uupdates the positions of the side backgrounds based on the camera's position to make a loop
        if (mainCam.position.x > middleBackground.position.x)
        {
            sidesBackground.position = newRightPosition;
        }
        else
        {
            sidesBackground.position = newLeftPosition;
        }

        if (mainCam.position.x > sidesBackground.position.x || mainCam.position.x < sidesBackground.position.x)
        {
            Transform Z = middleBackground;
            middleBackground = sidesBackground;
            sidesBackground = Z;
        }
    }
}

