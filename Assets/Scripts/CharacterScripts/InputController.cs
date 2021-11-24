using UnityEngine;

public class InputController : MonoBehaviour
{
    [Tooltip("Скорость движения персонажа")]
    [SerializeField]
    private float _speed;

    [Tooltip("Скорость поворота персонажа")]
    [SerializeField]
    private float _rotationSpeed = 0.05f;

    [Tooltip("Ссылка на джойстик из канваса")]
    [SerializeField]
    private FloatingJoystick _joystick;

    private Rigidbody rigidbody;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Определяем направление джойстика
    /// Обнуляем значения поворота и двжиения
    /// Если есть направление двигаем и поворачиваем персонажа
    /// </summary>
    private void FixedUpdate()
    {
        var direction = new Vector3(_joystick.Direction.x, 0, _joystick.Direction.y);
        rigidbody.angularVelocity=Vector3.zero;
        rigidbody.velocity=Vector3.zero;
        if (direction != Vector3.zero)
        {
            rigidbody.velocity = direction * _speed;

            var toRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation =
                Quaternion.RotateTowards(transform.rotation, toRotation, _rotationSpeed * Time.deltaTime);
        }
    }
}