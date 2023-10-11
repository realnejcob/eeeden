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

    public void AddToCurrentPeg(Peg newPeg) {
        if (currentStartingPeg == null) {
            currentStartingPeg = newPeg;
        } else {
            if (!IsConnectionCreated(new Peg[] { currentStartingPeg, newPeg })){
                currentEndingPeg = newPeg;
                CreateConnection();
                ResetCurrentConnections();
            }
        }
    }

    public void DeconfigurePeg(Peg peg) {
        if (currentStartingPeg != null) {
            ResetCurrentConnections();
        } else {
            RemoveConnectionByPeg(peg);
        }
    }

    private void RemoveConnection(PegConnection connection) {
        pegConnections.Remove(connection);
        connection.Destroy();
    }

    public void RemoveConnectionByPeg(Peg peg) {
        for (int i = 0; i < pegConnections.Count; i++) {
            var connection = pegConnections[i];
            if (connection.HasPeg(peg)) {
                RemoveConnection(connection);
                return;
            }
        }
    }

    public void RemoveConnectionByCurveChain(CurveChain curveChain) {
        for (int i = 0; i < pegConnections.Count; i++) {
            var connection = pegConnections[i];
            if (connection.HasChain(curveChain)) {
                RemoveConnection(connection);
                return;
            }
        }
    }

    private void CreateConnection() {
        var newCurveChain = Instantiate(curveChainPrefab);
        newCurveChain.transform.SetParent(currentStartingPeg.transform);

        var connection = new PegConnection(currentStartingPeg, currentEndingPeg, newCurveChain);

        connection.curveChainObj.transform.position = connection.startingPeg.transform.position;
        connection.curveChainObj.ConfigureAnchors(connection.startingPeg.Anchor, connection.endingPeg.Anchor);

        pegConnections.Add(connection);
    }

    public void ResetCurrentConnections() {
        currentStartingPeg = null;
        currentEndingPeg = null;
    }

    private bool IsConnectionCreated(Peg[] pegs) {
        foreach (var connection in pegConnections) {
            if (connection.HasPegs(pegs)) {
                return true;
            }
        }

        return false;
    }
}

[Serializable]
public class PegConnection {
    public Peg startingPeg;
    public Peg endingPeg;
    public CurveChain curveChainObj;

    public PegConnection(Peg _startingPeg, Peg _endingPeg, CurveChain _curveChainPrefab) {
        startingPeg = _startingPeg;
        endingPeg = _endingPeg;
        curveChainObj = _curveChainPrefab;

        startingPeg.AddCurveChain(curveChainObj);
        endingPeg.AddCurveChain(curveChainObj);
    }

    public bool HasPeg(Peg peg) {
        if (startingPeg == peg) {
            return true;
        } else if (endingPeg == peg) {
            return true;
        }

        return false;
    }

    public bool HasChain(CurveChain curveChain) {
        if (curveChainObj == curveChain) {
            return true;
        }

        return false;
    }

    public bool HasPegs(Peg[] pegs) {
        if (pegs[0] == startingPeg || pegs[0] == endingPeg) {
            if (pegs[1] == startingPeg || pegs[1] == endingPeg) {
                return true;
            }
        }

        return false;
    }

    public void Destroy() {
        startingPeg.RemoveCurveChain(curveChainObj);
        endingPeg.RemoveCurveChain(curveChainObj);

        UnityEngine.Object.Destroy(curveChainObj.gameObject);
    }
}
