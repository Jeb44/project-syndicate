using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController
{
    public abstract class StateBase
    {
        protected PlayerController _controller;
        public PlayerController Controller => _controller;
        public void Init(PlayerController controller)
        {
            _controller = controller;
        }

        public virtual void OnEnter() { }

        public virtual StateBase OnUpdate()
        {
            return null;
        }

        public virtual void OnExit() { }

        protected virtual void MoveWithMotor()
        {
            //Check in which direction we are walking, if not moving, stay with the current direction
            //if (move.x > 0f)
            //{
            //    references.direction.Right = true;
            //}
            //else if (move.x < 0f)
            //{
            //    references.direction.Left = true;
            //}

            //Move with accumulated move amount
            Controller.motor.Move(Controller.move * Time.deltaTime);
            Controller.move.Set(0f, 0f, 0f);
        }

        protected virtual void ResetGravityOnVerticalCollision()
        {
            if (Controller.motor.collision.above || Controller.motor.collision.below)
            {
                Controller.move.y = 0f;
            }
        }

        protected virtual void AddGravityToMovement()
        {
            float gravity = -(2 * Controller.jumpHeight) / Mathf.Pow(Controller.timeToJumpApex, 2);
            Controller.move.y += gravity * Time.deltaTime;
        }
    }
}
