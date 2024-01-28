using System;
using System.Collections.Generic;
using UnityEngine;

public class Prisoner
{
    private List<byte> playerMoves;
    private List<byte> currentPattern = null;
    private int currentPatternIndex = -1;
    private int depthLimit = 5;
    private int patternOffset = 0;

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
            patternOffset = 0;
            Debug.Log("Pattern scrapped"); // Debug
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
            Debug.Log("random");
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

        Debug.Log("player moves " + playerMoves.Count);
        int depth = depthLimit;
        if (playerMoves.Count <= 1) {
            return 0xff;
        } else if (playerMoves.Count < depthLimit) {
            depth = playerMoves.Count;
        } 
            
        // Get list of relevant moves
        List<byte> movesToAnalyze = new List<byte>();
        // Debug.Log("depth: " + depth);
        for (int i = playerMoves.Count - depth; i < playerMoves.Count; i++) {
            // Debug.Log("===");
            // Debug.Log("i: " + i);
            // Debug.Log("playermove[i]: " + playerMoves[i]);
            // Debug.Log("===");
            movesToAnalyze.Add(playerMoves[i]);
        }

        // Check for any patterns
        if (depth > 4 && CheckQuadPattern(movesToAnalyze)) {
            SetCurrentPattern(movesToAnalyze, 4, depth);
            return currentPattern[currentPatternIndex];
        }
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

    private bool CheckQuadPattern(List<byte> movesToAnalyze) {
        for (int h = 4; h < movesToAnalyze.Count; h++) {
            bool foundPattern = true;

            for (int i = h; i < movesToAnalyze.Count; i++) {
                if (movesToAnalyze[i] != movesToAnalyze[i-4]) {
                    foundPattern = false;
                    break;
                }
            }

            if (foundPattern) {
                patternOffset = h - 4;
                return true;
            }
        }

        return false;
    }
    private bool CheckTriPattern(List<byte> movesToAnalyze) {
        for (int h = 3; h < movesToAnalyze.Count; h++) {
            bool foundPattern = true;

            for (int i = h; i < movesToAnalyze.Count; i++) {
                if (movesToAnalyze[i] != movesToAnalyze[i-3]) {
                    foundPattern = false;
                    break;
                }
            }

            if (foundPattern) {
                patternOffset = h - 3;
                return true;
            }
        }

        return false;
    }
    private bool CheckBiPattern(List<byte> movesToAnalyze) {
        for (int h = 2; h < movesToAnalyze.Count; h++) {
            bool foundPattern = true;

            for (int i = h; i < movesToAnalyze.Count; i++) {
                if (movesToAnalyze[i] != movesToAnalyze[i-2]) {
                    foundPattern = false;
                    break;
                }
            }

            if (foundPattern) {
                patternOffset = h - 2;
                return true;
            }
        }
        return false;
    }
    private bool CheckUniPattern(List<byte> movesToAnalyze) {
        for (int h = 1; h < movesToAnalyze.Count; h++) {
            bool foundPattern = true;

            for (int i = h; i < movesToAnalyze.Count; i++) {
                if (movesToAnalyze[i] != movesToAnalyze[i-1]) {
                    foundPattern = false;
                    break;
                }
            }

            if (foundPattern) {
                patternOffset = h - 1;
                return true;
            }
        }
        return false;
    }

    private void SetCurrentPattern(List<byte> movesToAnalyze, int length, int depth) {
        PrintPlayerMoves();
        currentPattern = new List<byte>();
        Debug.Log("depth: " + depth);
        Debug.Log("pattern offset: " + patternOffset);
        Debug.Log("length: " + length);
        for (int i = 0; i < length; i++) {
            Debug.Log("moves to analyze: " + movesToAnalyze[i + patternOffset]);
            currentPattern.Add(movesToAnalyze[i + patternOffset]);
        }
        currentPatternIndex = length == 1 ? 0 : depth - patternOffset - length;
        Debug.Log("current pattern index: " + currentPatternIndex);
        Debug.Log("Pattern found! Next player move: " + currentPattern[currentPatternIndex]); // Debug
    }

    private byte CounterPlayerMove(bool valve1, bool valve2, int anticipated) {
        byte move = 0;
        if (((anticipated & 0x01) == 0x01) == valve1) {
            move += 0x01;
        }
        if (((anticipated & 0x02) == 0x02) == valve2) {
            move += 0x02;
        }
        Debug.Log((int) move);
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