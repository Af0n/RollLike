using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(DragSpin))]
[RequireComponent(typeof(Die))]

public class DropRoll : MonoBehaviour, IPointerUpHandler
{
    public float MinimumThrowPower;
    public float MaximumThrowPower;

    private Vector3 _throwDirection = new(0, 1, 1);

    private Rigidbody rb;
    private bool _doRoll;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    /**
     * Conditionally activates the OnRoll UnityEvent
     * 
     * Implements OnPointerUp(PointerEventData eventData) from Unity IPointerUpHandler Interface.
     * 
     * @param PointerEventData: contains information about the Pointer provided by Unity Event System
     */
    public void OnPointerUp(PointerEventData eventData)
    {
        // Don't proceed if not rolling
        if (!_doRoll)
            return;

        // Invoke Roll Unity Event
        GetComponent<Die>().OnRoll.Invoke();
    }

    /**
     * Enables roll functionality
     * 
     * @param other: The trigger collider entered
     */
    private void OnTriggerEnter(Collider other)
    {
        _doRoll = true;
    }

    /**
     * Disables roll functionality
     * 
     * @param other: The trigger collider left
     */
    private void OnTriggerExit(Collider other)
    {
        _doRoll = false;
    }

    /**
     * Applies enables physics and applies throw force
     */
    public void DoThrow()
    {
        // Enable physics
        rb.isKinematic = false;

        // Access existing die spin
        Vector3 rotation = GetComponent<DragSpin>().SpinRotation;

        // Apply spin to RigidBody
        rb.AddTorque(rotation * Time.deltaTime, ForceMode.Impulse);

        // Apply throw force with randomized power
        rb.AddForce(_throwDirection * Random.Range(MinimumThrowPower, MaximumThrowPower));
    }
}
