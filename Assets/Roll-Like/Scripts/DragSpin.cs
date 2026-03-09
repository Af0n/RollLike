using UnityEngine;
using UnityEngine.EventSystems;

public class DragSpin : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private Vector3 minimumRotation = new(0, 0, 0);
    [SerializeField]
    private Vector3 maximumRotation = new(0, 0, 0);

    private bool _doSpin;
    private Vector3 _rotation;

    public Vector3 SpinRotation
    {
        get { return _rotation; }
    }

    /**
     * Enables spin functionality when Pointer touches down on GameObject.
     * 
     * Implements OnPointerDown(PointerEventData eventData) from Unity IPointerDownHandler Interface.
     * 
     * @param PointerEventData: contains information about the Pointer provided by Unity Event System
     */
    public void OnPointerDown(PointerEventData eventData)
    {
        _doSpin = true;

        // Randomize values for rotation
        float randX = Random.Range(minimumRotation.x, maximumRotation.x);
        float randY = Random.Range(minimumRotation.y, maximumRotation.y);
        float randZ = Random.Range(minimumRotation.z, maximumRotation.z);

        _rotation = new(randX, randY, randZ);
    }

    /**
     * Disables spin functionality when Pointer lifts off of GameObject.
     * 
     * Implements OnPointerUp(PointerEventData eventData) from Unity IPointerUpHandler Interface.
     * 
     * @param PointerEventData: contains information about the Pointer provided by Unity Event System
     */
    public void OnPointerUp(PointerEventData eventData)
    {
        _doSpin = false;
    }

    private void Update()
    {
        // Check if spin should be executed
        if (!_doSpin)
            return;

        // Apply rotation
        transform.Rotate(_rotation * Time.deltaTime);
    }
}
