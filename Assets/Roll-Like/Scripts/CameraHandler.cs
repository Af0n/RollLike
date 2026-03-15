using UnityEngine;

[RequireComponent(typeof(Round))]

public class CameraHandler : MonoBehaviour
{
    [SerializeField]
    private Transform[] camPoints;

    private Round _roundScript;
    private Transform _targetPoint;

    private void Awake()
    {
        // Populating field
        _roundScript = GetComponent<Round>();
    }

    /**
     * Shorthand function for clarity
     */
    public void GoToDefault()
    {
        SetPoint(0);
    }

    /**
     * Sets the position for the camera to go to
     * 
     * @param i: Index of point in camPoints to move the camera to
     */
    public void SetPoint(int i)
    {
        // Makes sure to clamp to available indexes
        i = Mathf.Clamp(i, 0, camPoints.Length - 1);

        // Set target
        _targetPoint = camPoints[i];

        MoveCamToTarget();
    }

    private void MoveCamToTarget()
    {
        Camera.main.transform.SetPositionAndRotation(_targetPoint.position, _targetPoint.rotation);
    }
}
