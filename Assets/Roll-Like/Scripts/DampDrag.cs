using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody))]

public class DampDrag : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [Header("Constraints")]
    [SerializeField]
    [Tooltip("Toggle MaintainX")]
    private bool maintainX = false;
    [SerializeField]
    [Tooltip("Toggle MaintainY")]
    private bool maintainY = false;

    [Header("Settings")]
    [SerializeField]
    [Tooltip("Damping applied while dragging")]
    private float damping = 0.05f;

    private Vector3 _velocity = Vector3.zero;

    private bool _doDrag;
    private Vector3 _targetPosition;

    /**
     * Calls to UpdateTarget when Pointer drags on the GameObject.
     * 
     * Implements OnDrag(PointerEventData eventData) from Unity IDragHandler Interface.
     * 
     * @param PointerEventData: contains information about the Pointer provided by Unity Event System
     */
    public void OnDrag(PointerEventData eventData)
    {
        UpdateTarget(eventData);
    }

    /**
     * Enables drag functionality and disables physics when Pointer touches down on GameObject.
     * Initiates an initial snap to the Pointer when clicked.
     * 
     * Implements OnPointerDown(PointerEventData eventData) from Unity IPointerDownHandler Interface.
     * 
     * @param PointerEventData: contains information about the Pointer provided by Unity Event System
     */
    public void OnPointerDown(PointerEventData eventData)
    {
        _doDrag = true;

        GetComponent<Rigidbody>().isKinematic = true;

        UpdateTarget(eventData);
    }

    /**
     * Disables drag functionality when Pointer lifts off of GameObject.
     * 
     * Implements OnPointerUp(PointerEventData eventData) from Unity IPointerUpHandler Interface.
     * 
     * @param PointerEventData: contains information about the Pointer provided by Unity Event System
     */
    public void OnPointerUp(PointerEventData eventData)
    {
        _doDrag = false;
    }

    private void Update()
    {
        // Check if drag should be executed
        if (!_doDrag)
            return;

        // Apply damped translation
        transform.position = Vector3.SmoothDamp(transform.position, _targetPosition, ref _velocity, damping);
    }

    /**
     * Updates _targetPosition when called
     * 
     * @param PointerEventData: contains information about the Pointer provided by Unity Event System
     */
    private void UpdateTarget(PointerEventData eventData)
    {
        // Access eventData pointer position
        Vector3 mousePos = eventData.position;

        // Convert to world coordinates
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        // Maintain Z position within scene
        mousePos.z = transform.position.z;

        // Maintain X position within scene
        if (maintainX)
            mousePos.x = transform.position.x;

        // Maintain Y position within scene
        if (maintainY)
            mousePos.y = transform.position.y;

        // Apply translation
        _targetPosition = mousePos;
    }
}
