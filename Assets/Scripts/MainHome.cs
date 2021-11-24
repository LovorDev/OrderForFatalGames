using System;
using System.Collections;
using UnityEngine;


public class MainHome : MonoBehaviour
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
    [Tooltip("AddValueEvent - Вызывается когда добавляется припасы \r \n \r \n" +
             "DecreaseValueEvent - Вызывается когда отбавляются припасы\r \n \r \n" +
             "ChangingValuePercentEvent - Вызывается когда изменяется припасы " +
             "Передается текущий процент припасы  \r \n \r \n" +
             "ChangingValueEvent - Вызывается когда изменяется припасы \r \n \r \n" +
             "Передается текущий припасы  \r \n \r \n" +
             "AmountEndEvent - Вызывается когда кончилось припасы")]
    private Supplies _supplies;

    [SerializeField]
    [Tooltip("Скорость уменьшения припасов со временем")]
    private float _decreaseSuppliesSpeed;

    #endregion
    private void Start()
    {
        _health.Amount = _health.MaxAmount;
        StartCoroutine(DecreaseSupply());
    }

    private IEnumerator DecreaseSupply()
    {
        while (true)
        {
            _supplies.DecreaseAmount(_decreaseSuppliesSpeed * Time.deltaTime);
            yield return null;
        }
    }

    /// <summary>
    /// Use for changing supplies decrease speed
    /// </summary>
    public void SetDecreaseSuppliesSpeed(float value)
    {
        _decreaseSuppliesSpeed = value;
    }
    
    /// <summary>
    /// Если персонаж коснулся забираем у него припасы и прибавляем себе
    /// Если враг сносим здоровье
    /// </summary>
    private void OnCollisionEnter(Collision other)
    {
        var character = other.collider.GetComponent<Character>();
        if (character)
            _supplies.AddAmount(character.GetSupplies());
        
        var enemy = other.collider.GetComponent<EnemyDamageable>();
        if (enemy)
            _health.DecreaseAmount(enemy.Damage);
    }
}