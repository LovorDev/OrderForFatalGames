using System;
using System.Collections.Generic;
using System.Linq;
using BuffsScript;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class CharacterEvent
{
    [Serializable]
    public class IdleEvent : UnityEvent
    {
    }

    [Serializable]
    public class RunEvent : UnityEvent
    {
    }

    [Serializable]
    public class PickUpSuppliesEvent : UnityEvent<float>
    {
    }

    [Serializable]
    public class PlaceSuppliesEvent : UnityEvent
    {
    }

    [Serializable]
    public class PickUpBuffEvent : UnityEvent<Buff>
    {
    }


    public IdleEvent Idle;

    public RunEvent Run;

    public PickUpSuppliesEvent PickUpSupplies;

    public PlaceSuppliesEvent PlaceSupplies;

    public PickUpBuffEvent PickUpBuff;
}

public class Character : MonoBehaviour
{
    #region SerializeFields

    [SerializeField]
    [Tooltip("AddValueEvent - Вызывается когда добавляется хп \r \n \r \n" +
             "DecreaseValueEvent - Вызывается когда отбавляются хп\r \n \r \n" +
             "ChangingValuePercentEvent - Вызывается когда изменяется хп " +
             "Передается текущий процент хп  \r \n \r \n" +
             "ChangingValueEvent - Вызывается когда изменяется хп \r \n \r \n" +
             "Передается текущий хп  \r \n \r \n" +
             "AmountEndEvent - Вызывается когда кончилось хп")]
    private Health _health;

    [SerializeField]
    [Tooltip("IdleEvent - Вызывается когда персонаж переходит в состояние покоя \r \n \r \n" +
             "RunEvent - Вызывается когда персонаж переходит в состояние движения\r \n \r \n" +
             "PickUpSuppliesEvent - Вызывается когда персонаж подбирает припасы " +
             "Передается количество припасов в подобраном мешке \r \n \r \n" +
             "PlaceSuppliesEvent - Вызывается когда персонаж калдет припасы в дом \r \n \r \n" +
             "PickUpBuffEvent - Вызывается когда персонаж подбирает бафф " +
             "Передается ссылка на подобранный бафф")]
    private CharacterEvent _events;


    [SerializeField]
    private int _maxSupplies;

    [SerializeField]
    private Transform _pointToHoldSupplies;

    #endregion

    private List<SuppliesItem> _suppliesItems;
    public Health Health => _health;

    private Rigidbody _rigidbody;
    private MovingState _movingState;
    private int _currentSupplies;

    enum MovingState
    {
        Idle,
        Run
    }

    private void Start()
    {
        _suppliesItems = new List<SuppliesItem>();
        Health.Amount = Health.MaxAmount;
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void LateUpdate()
    {
        UpdateState();
    }

    public void SetMaxSupplies(int value)
    {
        _maxSupplies = value;
    }
    
    private void UpdateState()
    {
        if (_rigidbody.velocity == Vector3.zero && _movingState != MovingState.Idle)
        {
            _movingState = MovingState.Idle;
            _events.Idle?.Invoke();
        }
        else if (_rigidbody.velocity != Vector3.zero && _movingState != MovingState.Run)
        {
            _movingState = MovingState.Run;
            _events.Run?.Invoke();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        print(other);
        if (other.TryGetComponent<IPickable>(out var pickedItem))
            switch (pickedItem)
            {
                case SuppliesItem suppliesItem:
                    PickUpSupplies(suppliesItem);
                    break;
                case Buff buff:
                    PickUpBuff(buff);
                    break;
            }
    }

    private void OnCollisionEnter(Collision other)
    {
        var enemy = other.collider.GetComponent<EnemyDamageable>();
        if (enemy)
            _health.DecreaseAmount(enemy.Damage);
    }

    private void PickUpSupplies(SuppliesItem suppliesItem)
    {
        if (_currentSupplies >= _maxSupplies)
            return;

        _suppliesItems.Add(suppliesItem);
        suppliesItem.transform.parent = transform;
        suppliesItem.transform.localPosition = _pointToHoldSupplies.localPosition +
                                               new Vector3(0,  suppliesItem.Size.y * _suppliesItems.Count, 0);
        _events.PickUpSupplies?.Invoke(suppliesItem.Amount);
        _currentSupplies++;
    }

    private void PickUpBuff(Buff buff)
    {
        Destroy(buff.gameObject);
        
        _events.PickUpBuff?.Invoke(buff);
    }

    public float GetSupplies()
    {
        float summ;
        if (_suppliesItems.Count == 0) return 0;


        summ = _suppliesItems.Sum(item => item.Amount);
        foreach (var suppliesItem in _suppliesItems)
        {
            Destroy(suppliesItem.gameObject);
        }

        _suppliesItems = new List<SuppliesItem>();

        _events.PlaceSupplies?.Invoke();
        _currentSupplies = 0;
        return summ;
    }
}