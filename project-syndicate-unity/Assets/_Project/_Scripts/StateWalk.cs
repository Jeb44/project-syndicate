using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController
{
    public class StateWalk : StateBase
    {
        public override StateBase OnUpdate()
        {
            //ResetGravityOnVerticalCollision();

            Vector2 input = new Vector2(0f, 0f);

            if (Input.GetKey(KeyCode.W))
            {
                input += new Vector2(0f, 1f);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                input += new Vector2(0f, -1f);
            }

            if (Input.GetKey(KeyCode.D))
            {
                input += new Vector2(1f, 0f);
            }
            else if (Input.GetKey(KeyCode.A))
            {
                input += new Vector2(-1f, 0f);
            }

            //Debug.Log($"INPUT: {input} => W {Input.GetKey(KeyCode.W)} | S {Input.GetKey(KeyCode.S)} | D {Input.GetKey(KeyCode.D)} | A {Input.GetKey(KeyCode.A)}");
            //Debug.Log("WALK A: " + Controller.move.ToString());
            if (input.x == 0 && input.y == 0)
            {
                //Debug.Log("=> TO IDLE");
                return new StateIdle();
            }

            //Debug.Log("WALK B: " + Controller.move.ToString());
            Controller.move += new Vector3(input.x, 0f, input.y) * Controller.speed;
            //Debug.Log("WALK C: " + Controller.move.ToString());

            //AddGravityToMovement();
            MoveWithMotor();
            return null;
        }
    }
}