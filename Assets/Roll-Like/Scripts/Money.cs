using TMPro;
using UnityEngine;

public class Money : MonoBehaviour
{
    public static Money Instance;

    [SerializeField]
    private int _amount;
    [Header("Unity Set Up")]
    [SerializeField]
    [Tooltip("TMProTextMesh that appears when winning Round")]
    private TextMeshProUGUI moneyDisplay;

    private void Awake()
    {
        Instance = this;

        // Just in case
        _amount = 0;
    }

    private void Start()
    {
        UpdateDisplay();
    }

    public void Delta(int d)
    {
        _amount += d;
        UpdateDisplay();
    }

    public void Set(int s)
    {
        _amount = s;
        UpdateDisplay();
    }

    public void Multiply(float m)
    {
        _amount = Mathf.FloorToInt(_amount * m);
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        moneyDisplay.text = _amount.ToString() + " H";
    }
}
