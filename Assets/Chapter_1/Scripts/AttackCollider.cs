using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    public Ch1_Player owner;

    //공격 판정 가능
   public bool isAttackEnabled = false;
    float timer = 0f;

    private void Update()
    {
        if (isAttackEnabled)
            timer += Time.deltaTime;
        else
            timer = 0f;

        if (timer >= 0.2f)
            isAttackEnabled = false;
        

    }
    private void OnTriggerStay(Collider other)
    {
        if(isAttackEnabled && other.tag == "EnemyGroup")
        {
            EnemyGroup enemyGroup = other.GetComponent<EnemyGroup>();

            if(enemyGroup != null)
            {
                //공격 성공시 처리
                enemyGroup.HitByPlayer();
                owner.ResetAttackDisableTimer();
            }
        }
    }

    public void SetAttackEnabled(bool bState)
    {
        isAttackEnabled = bState;
    }
}
