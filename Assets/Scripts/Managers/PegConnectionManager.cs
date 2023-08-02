using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PegConnectionManager : MonoBehaviour {
    public static PegConnectionManager Instance;
    [SerializeField] [ReadOnly] private Peg currentStartingPeg;
    [SerializeField] [ReadOnly] private Peg currentEndingPeg;

    [Space(15)]

    [SerializeField] private CurveChain curveChainPrefab;
    [SerializeField] private List<PegConnection> pegConnections = new List<PegConnection>();

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    public void ConfigurePeg(Peg newPeg) {
        if (currentStartingPeg == null) {
            currentStartingPeg = newPeg;
        } else {
            currentEndingPeg = newPeg;
            CreateConnection();
            ResetCurrentConnections();
        }
    }

    private void CreateConnection() {
        var connection = new PegConnection() { 
            startingPeg = currentStartingPeg,
            endingPeg = currentEndingPeg,
            curveChainObj = Instantiate(curveChainPrefab)
        };

        connection.curveChainObj.transform.position = connection.startingPeg.transform.position;
        connection.curveChainObj.SetAnchors(connection.startingPeg.Anchor, connection.endingPeg.Anchor);
        connection.curveChainObj.BuildChain();

        pegConnections.Add(connection);
    }

    private void ResetCurrentConnections() {
        currentStartingPeg = null;
        currentEndingPeg = null;
    }
}

[Serializable]
public class PegConnection {
    public Peg startingPeg;
    public Peg endingPeg;
    public CurveChain curveChainObj;
}
