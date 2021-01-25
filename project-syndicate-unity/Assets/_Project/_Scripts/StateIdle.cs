using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController
{
    public class StateIdle : StateBase
    {
        public override StateBase OnUpdate()
        {
            //ResetGravityOnVerticalCollision();
            
            if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
            {
                return new StateWalk();
            }

            //Debug.Log("IDLE: " + Controller.move.ToString());
            //AddGravityToMovement();
            MoveWithMotor();
            return null;
        }

    }
}