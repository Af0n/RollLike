using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(DampDrag))]
[RequireComponent(typeof(DragSpin))]
[RequireComponent(typeof(DropSnap))]
[RequireComponent(typeof(DropRoll))]
[RequireComponent(typeof(Rigidbody))]

public class Die : MonoBehaviour
{
    public UnityEvent OnRoll;
    [Header("Editor Set Up")]
    [SerializeField]
    [Tooltip("Transforms dictating how to display each side.")]
    private Transform[] displaySides;

    [SerializeField]
    [Tooltip("Margin for dot product check when rolling.")]
    private float dotMargin = 0.9f;

    private Side[] _sides;

    private DampDrag _dampDrag;
    private DragSpin _dragSpin;
    private DropSnap _dropSnap;
    private DropRoll _dropRoll;
    private Rigidbody _rb;

    private int _rolledSideIndex = 0;

    private void Awake()
    {
        // Populate fields
        _sides = GetComponentsInChildren<Side>();

        _dampDrag = GetComponent<DampDrag>();
        _dragSpin = GetComponent<DragSpin>();
        _dropSnap = GetComponent<DropSnap>();
        _dropRoll = GetComponent<DropRoll>();
    }

    private void Start()
    {
        _dropSnap.SnapToTarget();
    }

    /**
     * Sets and initiates snap to appropriate Transform to display a certain side
     * 
     * @param targetIndex: index of Transform to snap to. [0-5]
     */
    public void DisplaySide(int targetIndex)
    {
        // Sanity check
        targetIndex = Mathf.Clamp(targetIndex, 0, displaySides.Length-1);
        _rolledSideIndex = targetIndex;

        DisplayRolledSide();
    }

    /**
     * Sets and initiates snap to appropriate Transform to display a certain side
     */
    public void DisplayRolledSide()
    {
        // Enable snapping and initiates snap
        _dropSnap.SetCanSnap(true);
        _dropSnap.DoSnap(displaySides[_rolledSideIndex]);
    }

    /**
     * Test function to make sure parsing is working
     */
    public void TestParse()
    {
        if (!ParseRoll(out Side sideRolled))
        {
            Debug.Log("Inconclusive");
            return;
        }
        else
        {
            DisplaySide((int)sideRolled.BaseValue-1);
            return;
        }
    }

    /**
     * Called to determine if roll is able to be read
     * 
     * @param sideRolled: Stores the side rolled if sufficiently able, null of not able
     * @return: success on parsing or not
     */
    public bool ParseRoll(out Side sideRolled)
    {
        foreach (Side side in _sides)
        {
            // Calculate dot product between the side's forward vector and world up
            float dot = Vector3.Dot(side.transform.forward, Vector3.up);

            // If current side is sufficiently up facing, that is the side we rolled
            if(dot >= dotMargin)
            {
                sideRolled = side;
                return true;
            }
        }

        sideRolled = null;
        return false;
    }
}
