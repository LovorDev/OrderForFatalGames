using UnityEngine;

public class SuppliesItem : MonoBehaviour,IPickable
{
    [SerializeField]
    private float _amount;

    [SerializeField]
    private Vector3 _size;

    public float Amount => _amount;

    public Vector3 Size => _size;

    public void SetAmount(float value)
    {
        _amount = value;
    }
}