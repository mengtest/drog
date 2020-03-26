using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class eventCompare : IComparer<ValueTuple<float,Action>> {
    public int Compare(ValueTuple<float,Action> a, ValueTuple<float,Action> b) {
        return a.Item1.CompareTo(b.Item1);
    }
}

public class LogicTimer
{
    PriorityQueue<ValueTuple<float,Action>> PQ = new PriorityQueue<ValueTuple<float,Action>>(new eventCompare());

    public int Count = 0;
    public void Init() {
        PQ = new PriorityQueue<ValueTuple<float,Action>>(new eventCompare());
    }
    public void Update() {
        while(PQ.Count > 0 && PQ.Top().Item1<=Logic.time) {
            var temp = PQ.Pop();
            temp.Item2();
            Count--;
        }
    }
    public void Add((float,Action) v) {
        PQ.Push(v);
        Count++;
    }
    public ValueTuple<float,Action> Pop() {
        Count--;
        return PQ.Pop();
    }
    public void Clear() {
        while(PQ.Count>0){
            PQ.Pop();
        }
        Count = 0;
    }
}
