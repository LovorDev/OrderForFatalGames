using System;
using UnityEngine;
using UnityEngine.Events;


[Serializable]
public class Health : Values
{
}

[Serializable]
public class Supplies : Values
{
}


/// <summary>
/// Сlass implements changes to some data and calls events about its different changes
/// </summary>
[Serializable]
public class Values
{
    [SerializeField]
    private float _maxAmount;

    public float MaxAmount => _maxAmount;

    public bool HasAmount => Amount > 0;
    public bool IsAmountEmpty => Amount <= 0;

    [Header("Events")]
    [Space(10)]
    [SerializeField]
    private AddValueEvent AddValue;

    [SerializeField]
    private DecreaseValueEvent DecreaseValue;

    [SerializeField]
    private ChangingValuePercentEvent _changingValuePercent;   
    [SerializeField]
    private ChangingValueEvent _changingValue;

    [SerializeField]
    private AmountEndEvent _amountEnd;

    private float amount = -1;

    public float Amount
    {
        get
        {
            if (amount == -1)
                amount = _maxAmount;
            return amount;
        }
        set
        {
            amount = Mathf.Clamp(value, 0, _maxAmount);
            
            _changingValuePercent?.Invoke(Amount / _maxAmount);
            _changingValue?.Invoke(Amount);
        }
    }

    public bool HaveSupplies => Amount > 0;


    [Serializable]
    public class AddValueEvent : UnityEvent
    {
    }

    [Serializable]
    public class DecreaseValueEvent : UnityEvent
    {
    }

    [Serializable]
    public class ChangingValuePercentEvent : UnityEvent<float>
    {
    }   
    [Serializable]
    public class ChangingValueEvent : UnityEvent<float>
    {
    }

    [Serializable]
    public class AmountEndEvent : UnityEvent
    {
    }

    public void AddAmount(float value)
    {
        Amount += value;
        AddValue?.Invoke();
        _changingValuePercent?.Invoke(Amount / _maxAmount);
    }

    public void DecreaseAmount(float value)
    {
        Amount -= value;
        DecreaseValue?.Invoke();

        if(IsAmountEmpty)
            _amountEnd?.Invoke();
    }
}