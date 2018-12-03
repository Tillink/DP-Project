using System;
using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics;

// Object Pool을 위한 클래스 ver.Stack
public class ObjectPoolStack<T>
{
    private Stack<T> objects = new Stack<T>();

    private Func<T> createFunc;
    private int createAmount;

    public ObjectPoolStack(int createAmount, Func<T> createFunc)
    {
        this.createAmount = createAmount;
        this.createFunc = createFunc;
        CreateObjects();
    }

    private void CreateObjects()
    {
        for (int i = 0; i < createAmount; i++)
        {
            objects.Push(createFunc());
        }
    }

    public void ReturnObject(T obj)
    {
        objects.Push(obj);
    }

    public T GetObject()
    {
        if (objects.Count == 0)
        {
            CreateObjects();
        }

        return objects.Pop();
    }
}
