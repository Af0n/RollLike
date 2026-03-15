using UnityEngine;

[RequireComponent(typeof(Round))]

public class CameraHandler : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Points that the camera can be positioned at.")]
    private Transform[] camPoints;
    [Header("Unity Set Up")]
    [SerializeField]
    [Tooltip("DropSnap script of the camera")]
    private DropSnap camDropSnap;

    private Transform _targetPoint;

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
        camDropSnap.DoSnap(_targetPoint);
    }
}
