using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempInput : MonoBehaviour
{
    public CharacterMotor controller;

    public float speed;

    // Update is called once per frame
    void Update()
    {
        Vector3 input = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            input += new Vector3(0f, 0f, 1f);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            input += new Vector3(0f, 0f, -1f);
        }

        if (Input.GetKey(KeyCode.D))
        {
            input += new Vector3(1f, 0f, 0f);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            input += new Vector3(-1f, 0f, 0f);
        }

        input *= Time.deltaTime * speed;
        controller.Move(input);
    }
}
