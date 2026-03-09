using UnityEngine;

public class Side : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Unmodified Value for side")]
    private float _baseValue;

    public float BaseValue
    {
        get { return _baseValue; }
    }

    public float EvaluateSideScore()
    {
        return BaseValue;
    }
}
