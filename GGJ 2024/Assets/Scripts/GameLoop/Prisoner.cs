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

        int depth = depthLimit;
        if (playerMoves.Count <= 1) {
            return 0xff;
        } else if (playerMoves.Count < depthLimit) {
            depth = playerMoves.Count;
        } 
            
        // Get list of relevant moves
        List<byte> movesToAnalyze = new List<byte>();
        for (int i = playerMoves.Count - depth; i < playerMoves.Count; i++) {
            movesToAnalyze.Add(playerMoves[i]);
        }

        // Check for any sub patterns
        for (int i = depth - 1; i > 0; i--) {
            if (CheckSubPattern(movesToAnalyze, i)) {
                SetCurrentPattern(movesToAnalyze, i, depth);
                return currentPattern[currentPatternIndex];
            }
        }
        return 0xff;
    }

    private bool CheckSubPattern(List<byte> movesToAnalyze, int length) {
        for (int h = length; h < movesToAnalyze.Count; h++) {
            bool foundPattern = true;

            for (int i = h; i < movesToAnalyze.Count; i++) {
                if (movesToAnalyze[i] != movesToAnalyze[i-length]) {
                    foundPattern = false;
                    break;
                }
            }

            if (foundPattern) {
                patternOffset = h - length;
                return true;
            }
        }

        return false;
    }
    
    private void SetCurrentPattern(List<byte> movesToAnalyze, int length, int depth) {
        currentPattern = new List<byte>();
        for (int i = 0; i < length; i++) {
            currentPattern.Add(movesToAnalyze[i + patternOffset]);
        }
        currentPatternIndex = length == 1 ? 0 : depth - patternOffset - length;
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

    public bool HasPattern()
    {
        return currentPattern != null;
    }

    // IMPORTANT: Does not modify currentpattern
    public byte GetCurrentAnticipatedPlayerMove()
    {
        return currentPattern[currentPatternIndex];
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