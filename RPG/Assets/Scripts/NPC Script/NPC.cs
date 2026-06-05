using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class NPC : MonoBehaviour
{
    public enum NPCState
    {
        Default, Idle, Patrol, Wander, Talk
    }

    [Header("状态设置")]
    public NPCState currentState = NPCState.Patrol;
    public float stateChangeDelay = 0.2f;  // 状态切换延迟

    [Header("组件引用")]
    public NPC_Patrol patrol;
    public NPC_Wander wander;
    public NPC_Talk talk;



    private NPCState defaultState;
    private bool isSwitching;
    private Coroutine delayedExitCoroutine;

    void Start()
    {
        defaultState = currentState;
        SwitchState(currentState);
    }

    public void SwitchState(NPCState newState)
    {
        Debug.Log($"切换状态 {currentState} -> {newState}");
        if (isSwitching || currentState == newState) return;

        isSwitching = true;

        // 退出当前状态的特殊处理
        if (currentState == NPCState.Talk && talk != null)
        {
            // 可以在这里做清理
        }

        currentState = newState;

        // 切换组件
        if (patrol != null) patrol.enabled = (newState == NPCState.Patrol);
        if (wander != null) wander.enabled = (newState == NPCState.Wander);
        if (talk != null) talk.enabled = (newState == NPCState.Talk);


        StartCoroutine(ResetSwitchFlag());
    }

    IEnumerator ResetSwitchFlag()
    {
        yield return new WaitForSeconds(stateChangeDelay);
        isSwitching = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (delayedExitCoroutine != null)
                StopCoroutine(delayedExitCoroutine);

            SwitchState(NPCState.Talk);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            delayedExitCoroutine = StartCoroutine(DelayedRestore());
        }
    }

    IEnumerator DelayedRestore()
    {
        yield return new WaitForSeconds(0.3f);
        SwitchState(defaultState);
    }
}