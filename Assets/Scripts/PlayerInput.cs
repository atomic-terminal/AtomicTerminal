using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class PlayerInput
{
    private Player pController;
    public void Init(Player player)
    {
        this.pController = player;
    }
    private bool AcceptInput()
    {
        return true;
    }
    public void InputProcess()
    {
        this.MouseLookAt();
        if (this.AcceptInput())
        {
            if (Input.GetKeyDown(KeyCode.Space)&& this.CanRoll())
            {
                Player.player.Stop(false);
                Player.player.GetAsuraMove().SetCharState(PlayerMove.CharacterState.Roll);
            }
            if (Input.GetMouseButtonDown(0))
            {
                this.AttackAnim(true);
            }
            if (Input.GetMouseButtonDown(1))
            {
                this.AttackAnim(true,true);
            }
        }
    }
    private bool CanRoll()
    {
        if (!Player.player.GetAsuraMove().CanRoll())
        {
            return false;
        }
        return true;
    }
    private void MouseLookAt()
    {
        if (Player.player.GetAsuraMove().GetCharState() == PlayerMove.CharacterState.Idle)
        {
            this.LookAt();
        }
    }
    private Vector3 LookAtVector(float Yheight = 1.5f)
    {
        Plane plane = new Plane(Vector3.up, new Vector3(0f, Yheight, 0f));
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distance = 0f;
        if (plane.Raycast(ray, out distance))
        {
            return ray.GetPoint(distance);
        }
        return Vector3.zero;
    }
    private void LookAt()
    {
        Player.player.LookAt(this.LookAtVector(1.5f));
    }

    private void AttackAnim(bool lookAt,bool special=false)
    {
        if (Player.player.GetPlayerAttack().CanAttack(true))
        {
            if (lookAt)
            {
                this.LookAt();
                Player.player.GetAsuraMove().UpdateMoveDir();
            }
            Player.player.Attack(special);
        } 

    }
}
