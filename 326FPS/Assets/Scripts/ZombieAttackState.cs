using UnityEngine;
using UnityEngine.AI;

public class ZombieAttackState : StateMachineBehaviour
{
    Transform player;
    NavMeshAgent agent;

    public float stopAttackRange = 2.5f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (SoundMng.Instance.zombieChannel.isPlaying == false)
        {
            SoundMng.Instance.zombieChannel.PlayOneShot(SoundMng.Instance.zombieAttack);
        }

        LookAtPlayer();
        float distanceToPlayer = Vector3.Distance(player.position, animator.transform.position);
        if (distanceToPlayer > stopAttackRange)
        {
            animator.SetBool("isAttacking", false);
        }
    }
    void LookAtPlayer()
    {
        Vector3 direction = player.position - agent.transform.position;
        agent.transform.rotation = Quaternion.LookRotation(direction);

        var yRotation = agent.transform.eulerAngles.y;
        agent.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
