using UnityEngine;
using UnityEngine.AI;

public class ZigZagMovement : MonoBehaviour
{
    public Transform target;
    public float zigZagAmplitude = 2f; // 左右偏移幅度
    public float zigZagFrequency = 5f; // 每隔多远拐一次

    private NavMeshAgent agent;
    private Vector3 nextPoint;
    private int direction = 1;
    private float traveled = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        nextPoint = transform.position;
    }

    void Update()
    {
        Vector3 toTarget = target.position - transform.position;
        float distanceToTarget = toTarget.magnitude;

        if (distanceToTarget > 0.5f)
        {
            traveled += agent.velocity.magnitude * Time.deltaTime;
            if (traveled >= zigZagFrequency)
            {
                // 计算左右偏移方向
                Vector3 directionToTarget = toTarget.normalized;
                Vector3 perpendicular = Vector3.Cross(directionToTarget, Vector3.up).normalized;

                // 生成新的目标点
                nextPoint = transform.position + directionToTarget * zigZagFrequency + perpendicular * zigZagAmplitude * direction;
                direction *= -1;
                traveled = 0f;
            }

            agent.SetDestination(nextPoint);
        }
    }
}
