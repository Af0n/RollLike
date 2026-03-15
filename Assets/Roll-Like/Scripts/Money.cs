using UnityEngine;

public class Money : MonoBehaviour
{
    public Money Instance;

    [SerializeField]
    private int _amount;

    private void Awake()
    {
        Instance = this;
    }

    public void Delta(int d)
    {
        _amount += d;
    }

    public void Set(int s)
    {
        _amount = s;
    }

    public void Multiply(float m)
    {
        _amount = Mathf.FloorToInt(_amount * m);
    }
}
