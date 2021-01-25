using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController : MonoBehaviour
{
    public CharacterMotor motor;
    public float speed = 10f;
    public float jumpHeight = 2f;
    public float timeToJumpApex = 0.5f;

    StateBase _state;

    public Vector3 move;

    private void OnEnable()
    {
        _state = new StateIdle();
        _state.Init(this);
        Debug.Log($"[PlayerController] -> {_state}");
        _state.OnEnter();
    }

    public void Update()
    {
        if(_state == null) { Debug.LogError("[PlayerController] State not set up!"); }

        StateBase newState = _state.OnUpdate();
        if(newState != null)
        {
            _state.OnExit();

            Debug.Log($"[PlayerController] {_state} -> {newState}");
            _state = newState;
            _state.Init(this);
            _state.OnEnter();
        }
    }
}
