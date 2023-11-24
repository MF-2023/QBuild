using System;
using System.Collections;
using System.Collections.Generic;
using QBuild.Player;
using QBuild.Player.Controller;
using QBuild.Player.Core;
using UnityEngine;

public class PlayerMoverTest : MonoBehaviour
{
    [SerializeField] private Movement _movement;
    private Rigidbody _myRB;

    private IMover _mover;

    private void Start()
    {
        _myRB = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        _mover = _movement;
        _mover.OnMover = true;
        Vector3 v = new Vector3(10, 0, 5);
        _myRB.velocity = v;
        _mover.CurrentMoverVelo = v;
    }
}
