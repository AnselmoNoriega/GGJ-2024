using System;
using System.Collections.Generic;
using UnityEngine;

public class Prisoner
{
    private List<byte> playerMoves;
    private List<byte> currentPattern = null;
    private int currentPatternIndex = -1;

    public Prisoner() {
        playerMoves = new List<byte>();
    }


    public void LogPlayerMove(bool valve1move, bool valve2move) {
        LogPlayerMove((byte)((valve1move ? 1 : 0) + (valve2move ? 2 : 0)));
    }

    // REQUIRES: 0 <= playerMove <= 3
    public void LogPlayerMove(byte playerMove) {
        playerMoves.Add(playerMove);
        if (currentPattern != null && currentPattern[currentPatternIndex] != playerMove) {
            currentPattern = null;
        }
    }

    public bool[] GetNextMove(bool valve1, bool valve2)
    {
        byte move = CalculateMove(valve1, valve2);
        return new bool[] { (move & 0x01) == 0x01, (move & 0x02) == 0x02 };
    }

    private byte CalculateMove(bool valve1, bool valve2) {
        byte anticipated = CheckForPattern();
        if (anticipated == 0xff) {
            return (byte)UnityEngine.Random.Range(0, 4);
        } else {
            return CounterPlayerMove(valve1, valve2, anticipated);
        }
    }

    private byte CheckForPattern() {
        if (currentPattern != null) {
            if (++currentPatternIndex == currentPattern.Count) {
                currentPatternIndex = 0;
            }
            return currentPattern[currentPatternIndex];
        }

        // Choose random number of past player moves to analyze [2-5]
        int depth = UnityEngine.Random.Range(2, 6);
        if (playerMoves.Count < depth) {
            return 0xff;
        }
            
        // Get list of relevant moves
        List<byte> movesToAnalyze = new List<byte>();
        for (int i = playerMoves.Count - depth; i < playerMoves.Count; i++) {
            movesToAnalyze.Add(playerMoves[i]);
        }

        // Check for any patterns
        if (depth > 3 && CheckTriPattern(movesToAnalyze)) {
            SetCurrentPattern(movesToAnalyze, 3, depth);
            return currentPattern[currentPatternIndex];
        }
        if (depth > 2 && CheckBiPattern(movesToAnalyze)) {
            SetCurrentPattern(movesToAnalyze, 2, depth);
            return currentPattern[currentPatternIndex];
        }
        if (CheckUniPattern(movesToAnalyze)) {
            SetCurrentPattern(movesToAnalyze, 1, depth);
            return currentPattern[currentPatternIndex];
        }
        return 0xff;
    }

    private bool CheckTriPattern(List<byte> movesToAnalyze) {
        for (int i = 3; i < movesToAnalyze.Count; i++) {
            if (movesToAnalyze[i] != movesToAnalyze[i-3]) {
                return false;
            }
        }
        return true;
    }

    private bool CheckBiPattern(List<byte> movesToAnalyze) {
        for (int i = 2; i < movesToAnalyze.Count; i++) {
            if (movesToAnalyze[i] != movesToAnalyze[i-2]) {
                return false;
            }
        }
        return true;
    }
    private bool CheckUniPattern(List<byte> movesToAnalyze) {
        for (int i = 1; i < movesToAnalyze.Count; i++) {
            if (movesToAnalyze[i] != movesToAnalyze[i-1]) {
                return false;
            }
        }
        return true;
    }

    private void SetCurrentPattern(List<byte> movesToAnalyze, int length, int depth) {
        currentPattern = new List<byte>();
        for (int i = 0; i < length; i++) {
            currentPattern.Add(movesToAnalyze[i]);
        }
        currentPatternIndex = depth % length;
    }

    private byte CounterPlayerMove(bool valve1, bool valve2, int anticipated) {
        byte move = 0;
        if (((anticipated & 0x01) == 0x01) == valve1) {
            move += 0x01;
        }
        if (((anticipated & 0x02) == 0x02) == valve2) {
            move += 0x02;
        }
        return move;
    }

    // DEBUG
    public void PrintPlayerMoves() {
        string output = "";
        for (int i = 0; i < playerMoves.Count; i++) {
            output += "> MOVE " + i + ": " + playerMoves[i];
        }
        Debug.Log(output);
    }
}