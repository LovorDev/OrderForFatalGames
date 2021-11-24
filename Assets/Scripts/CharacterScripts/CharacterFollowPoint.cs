using UnityEngine;

public class CharacterFollowPoint : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Ось для слежания за персонажем. Vector(1,0,0) - камера будет слежить только по оси x")]
    private Vector3 _axisToFollow;

    [SerializeField]
    [Tooltip("Ссылка на персонажа")]
    private Transform _character;

    [SerializeField]
    [Tooltip("Задержка движения")]
    private float _damping;

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position,Mp(_character.position,_axisToFollow),_damping*Time.deltaTime);
    }

    public Vector3 Mp (Vector3 one, Vector3 two)
    {
        return new Vector3(one.x * two.x, one.y * two.y, one.z * two.z);
    }
}
