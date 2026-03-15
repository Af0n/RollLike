using TMPro;
using UnityEngine;

public class Money : MonoBehaviour
{
    public static Money Instance;
    [SerializeField]
    private int startingMoney;
    [SerializeField]
    private int amount;
    [Header("Unity Set Up")]
    [SerializeField]
    [Tooltip("TMProTextMesh that appears when winning Round")]
    private TextMeshProUGUI moneyDisplay;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // Initial money + display
        amount = startingMoney;
        UpdateDisplay();
    }

    public void Delta(int d)
    {
        amount += d;
        UpdateDisplay();
    }

    public void Set(int s)
    {
        amount = s;
        UpdateDisplay();
    }

    public void Multiply(float m)
    {
        amount = Mathf.FloorToInt(amount * m);
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        moneyDisplay.text = amount.ToString() + " H";
    }
}
