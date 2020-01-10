using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private bool cancelAttack;
    public bool canAttackDash = false;
    private int attackType = 1;
    private float tempAttackDisable = -1f;
    private AnimatorStateInfo stateInfo;
    private Player pController;
    private Coroutine currentCor;

    [Header("Normal")]
    public float normalAttackDelay = 0.1f;
    public float normalAttackDash = 12f;
    public float normalDashTime = 0.15f;
    [Header("Special")]
    public float specialAttackDelay = 0.2f;
    public float specialAttackDash = 50f;
    public float specialDashTime = 0.15f;

    public void Init(Player player)
    {
        this.pController = player;
    }
    public bool CanCancelAttack()
    {
        return this.cancelAttack;
    }
    public void ResetAttackType()
    {
        this.attackType = 1;
    }
    public int GetAttackType()
    {
        return this.attackType;
    }
    public void DisableAttack()
    {
        this.tempAttackDisable = 0.3f;
    }
    public Coroutine GetCurrentCoro()
    {
        return this.currentCor;
    }
    public void Attack(bool special)
    {
        if (special)
        {
            ResetAttackType();
            Player.player.GetAsuraMove().SetCharState(PlayerMove.CharacterState.SpecialAttack);
        }
        else
        {
            if (this.attackType == 1)
            {
                Player.player.GetAsuraMove().SetCharState(PlayerMove.CharacterState.Attack1);
                this.attackType = 2;
            }
            else if (this.attackType == 2)
            {
                Player.player.GetAsuraMove().SetCharState(PlayerMove.CharacterState.Attack2);
                this.attackType = 1;
            }
        }
        currentCor= StartCoroutine(Dash(special));
    }
    public bool CanAttack(bool isMelee)
    {
        if (this.tempAttackDisable != -1f)
        {
            return false;
        }
        if (!Player.player.GetAsuraMove().CanAttack())
        {
            return false;
        }
        return true;
    }

    private void Update()
    {
        stateInfo = Player.player.GetPlayerAnim().GetCurrentAnimatorStateInfo(0);
        float StateTime = Mathf.Repeat(stateInfo.normalizedTime, 1);
        if (stateInfo.normalizedTime >= 0.6f
            && (stateInfo.IsName("Attack1")|| stateInfo.IsName("Attack2")|| stateInfo.IsName("SpecialAttack")))
        {
            Player.player.PlayIdle();
        }
    }
    IEnumerator  Dash(bool special)
    {
        yield return base.StartCoroutine(this.WaitForSeconds(special ? specialAttackDelay : normalAttackDelay));
        if (this.canAttackDash)
        {
            Player.player.GetAsuraMove().AttackDash(pController.GetPlayerTransform().forward
                *(special ? specialAttackDash : normalAttackDash), (special ? specialDashTime : normalDashTime));
        }
    }
    private IEnumerator WaitForSeconds(float numberOfSeconds)
    {
        float t = 0f;
        while (t < numberOfSeconds)
        {
            t += Time.deltaTime;
            yield return null;
        }
        yield break;
    }
}
