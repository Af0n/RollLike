using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody))]

public class DropSnap : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField]
    [Tooltip("Target to snap to when dropped.")]
    private Transform target;
    [SerializeField]
    private bool doSnapPosition;
    [SerializeField]
    private bool doSnapRotation;
    [SerializeField]
    [Tooltip("Time in seconds it takes to snap")]
    private float snapTime = 1;
    [SerializeField]
    [Tooltip("Multiplier for the speed of the position Lerp")]
    private float positionModifier = 1;
    [SerializeField]
    [Tooltip("Multiplier for the speed of the rotation Lerp")]
    private float rotationModifier = 1;
    [Header("Equations")]
    [SerializeField]
    [Tooltip("Ease Aggression. Values above 7 recommended for Lerp [0-1]")]
    private float easeAggression = 7;
    [SerializeField]
    private SnapType snapType;

    private Coroutine _snapRoutine;
    private bool _canSnap;

    private enum SnapType
    {
        LINEAR,
        QUADRATIC,
        EASE_OUT
    }

    /**
     * Initiates snap to position when Pointer lifts off GameObject
     * 
     * Implements OnPointerUp(PointerEventData eventData) from Unity IPointerUpHandler Interface.
     * 
     * @param PointerEventData: contains information about the Pointer provided by Unity Event System
     */
    public void OnPointerUp(PointerEventData eventData)
    {
        // Check if snapping is allowed
        if (!_canSnap)
            return;

        // Don't snap if no existing target
        if (target == null)
            return;

        DoSnap(target);
    }

    /**
     * Cancels snap coroutine if it running
     * 
     * Implements OnPointerDown(PointerEventData eventData) from Unity IPointerDownHandler Interface.
     * 
     * @param PointerEventData: contains information about the Pointer provided by Unity Event System
     */
    public void OnPointerDown(PointerEventData eventData)
    {
        // Checks for snap routine not existing
        if(_snapRoutine == null)
            return;

        StopCoroutine(_snapRoutine);
        _snapRoutine = null;
    }

    private void Awake()
    {
        _canSnap = true;
    }

    public void SetCanSnap(bool b)
    {
        _canSnap = b;
    }

    /**
     * Publicly accessible snap function.
     * Snaps to existing target
     */
    public void SnapToTarget()
    {   
        // Don't snap if not allowed
        if (!_canSnap)
            return;

        // Don't snap if no existing target
        if (target == null)
            return;

        DoSnap(target);
    }

    /**
     * Publicly accessible snap function.
     * Used to make object snap to object from outside.
     * Changes target
     * 
     * @param t: target for the snap
     */
    public void DoSnap(Transform t)
    {
        // Sanity check 
        if (!_canSnap)
            return;

        target = t;

        GetComponent<Rigidbody>().isKinematic = true;

        _snapRoutine = snapType switch
        {
            SnapType.LINEAR => StartCoroutine(DoLinearSnap(transform.position, transform.rotation)),
            SnapType.QUADRATIC => StartCoroutine(DoQuadSnap(transform.position, transform.rotation)),
            SnapType.EASE_OUT => StartCoroutine(DoEaseOutSnap(transform.position, transform.rotation)),
            _ => StartCoroutine(DoLinearSnap(transform.position, transform.rotation)),
        };
    }

    /**
     * Linear Lerp between initial position/rotation to target position/rotation
     */
    private IEnumerator DoLinearSnap(Vector3 initialPosition, Quaternion initialRotation)
    {

        float time = 0;

        while(time <= 1)
        {
            // Check if snapping is allowed
            if (!_canSnap)
                yield break;

            // Count for time between frames
            time += Time.deltaTime / snapTime;

            // Apply time modifiers
            float positionTime = Mathf.Min(time * positionModifier, 1);
            float rotationTime = Mathf.Min(time * rotationModifier, 1);

            // Potential constants for this frame
            Vector3 position = transform.position;
            Quaternion rotation = transform.rotation;

            // Check for enabled position snapping
            if (doSnapPosition)
                // Apply Lerp using initial position 
                position = Vector3.Lerp(initialPosition, target.position, positionTime);

            // Check for enabled rotation snapping
            if (doSnapRotation)
                // Apply Lerp using initial rotation 
                rotation = Quaternion.Lerp(initialRotation, target.rotation, rotationTime);

            // Set values within transform for this frame
            transform.SetPositionAndRotation(position, rotation);

            // Wait for next frame
            yield return 0;
        }

        // Final jsut-to-make-sure set
        transform.SetPositionAndRotation(target.position, target.rotation);
    }

    /**
     * Quadratic Lerp between initial position/rotation to target position/rotation
     */
    private IEnumerator DoQuadSnap(Vector3 initialPosition, Quaternion initialRotation)
    {
        float time = 0;

        while (time <= 1)
        {
            // Check if snapping is allowed
            if (!_canSnap)
                yield break;

            // Count for time between frames
            time += Time.deltaTime / snapTime;

            // Apply time modifiers
            float positionTime = Mathf.Min(time * positionModifier, 1);
            float rotationTime = Mathf.Min(time * rotationModifier, 1);

            // Apply quadratic equation
            float lerpTimePosition = -0.5f * Mathf.Cos(Mathf.PI * positionTime) + 0.5f;
            float lerpTimeRotation = -0.5f * Mathf.Cos(Mathf.PI * rotationTime) + 0.5f;

            // Potential constants for this frame
            Vector3 position = transform.position;
            Quaternion rotation = transform.rotation;

            // Check for enabled position snapping
            if (doSnapPosition)
                // Apply Lerp using initial position 
                position = Vector3.Lerp(initialPosition, target.position, lerpTimePosition);

            // Check for enabled rotation snapping
            if (doSnapRotation)
                // Apply Lerp using initial rotation 
                rotation = Quaternion.Lerp(initialRotation, target.rotation, lerpTimeRotation);

            // Set values within transform for this frame
            transform.SetPositionAndRotation(position, rotation);

            // Wait for next frame
            yield return 0;
        }

        // Final jsut-to-make-sure set
        transform.SetPositionAndRotation(target.position, target.rotation);
    }

    /**
     * Quadratic Lerp between initial position/rotation to target position/rotation
     */
    private IEnumerator DoEaseOutSnap(Vector3 initialPosition, Quaternion initialRotation)
    {
        float time = 0;

        while (time <= 1)
        {
            // Check if snapping is allowed
            if (!_canSnap)
                yield break;

            // Count for time between frames
            time += Time.deltaTime / snapTime;

            // Apply time modifiers
            float positionTime = Mathf.Min(time * positionModifier, 1);
            float rotationTime = Mathf.Min(time * rotationModifier, 1);

            // Apply Ease In equation
            float lerpTimePosition = -Mathf.Pow(2, -easeAggression * positionTime) + 1;
            float lerpTimeRotation = -Mathf.Pow(2, -easeAggression * rotationTime) + 1;

            // Potential constants for this frame
            Vector3 position = transform.position;
            Quaternion rotation = transform.rotation;

            // Check for enabled position snapping
            if (doSnapPosition)
                // Apply Lerp using initial position 
                position = Vector3.Lerp(initialPosition, target.position, lerpTimePosition * positionModifier);

            // Check for enabled rotation snapping
            if (doSnapRotation)
                // Apply Lerp using initial rotation 
                rotation = Quaternion.Lerp(initialRotation, target.rotation, lerpTimeRotation * rotationModifier);

            // Set values within transform for this frame
            transform.SetPositionAndRotation(position, rotation);

            // Wait for next frame
            yield return 0;
        }

        // Final jsut-to-make-sure set
        transform.SetPositionAndRotation(target.position, target.rotation);
    }
}
