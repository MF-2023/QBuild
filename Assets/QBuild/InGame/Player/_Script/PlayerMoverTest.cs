using System.Collections.Generic;
using QBuild.Player;
using UnityEngine;

public class PlayerMoverTest : MonoBehaviour
{
    private Rigidbody _myRB;

    private List<IMover> _movers = new List<IMover>();

    private void Start()
    {
        _myRB = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 v = new Vector3(10, 0, 5);
        _myRB.velocity = v;
        foreach (IMover mover in _movers)
        {
            mover.SetMoverVelocity(v);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.TryGetComponent<PlayerAdapter>(out PlayerAdapter adapter))
        {
            adapter.OnMoverEnter();
            _movers.Add(adapter);
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.transform.TryGetComponent<PlayerAdapter>(out PlayerAdapter adapter))
        {
            adapter.OnMoverExit();
            _movers.Remove(adapter);
        }
    }
}
