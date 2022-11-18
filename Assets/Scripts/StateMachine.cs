using System;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe faisant le liens entre un objet de type T et ses états
/// </summary>
/// <typeparam name="T">Le type d'objet dont la StateMachine gère les états</typeparam>
public class StateMachine<T>
{

    /// <summary>
    /// L'objet qui vas utiliser les états
    /// </summary>
    protected T _owner;
    
    protected State<T> _currentState;

    public State<T> CurrentState {get => _currentState; set => setState(value);}

    public T Owner => _owner;

    public StateMachine(T owner)
    {
        _owner = owner;
    }

    public virtual void update() {
        if(_currentState == null)
            Debug.LogError("No state to execute");
        else
            _currentState?.update();
    }

    public virtual void fixedUpdate() {
        _currentState?.fixedUpdate();
    }

    public void setState(State<T> newState)
    {
        _currentState?.exit();

        _currentState = newState;
        newState?.enter();
    }
}


/// <summary>
/// Pour définir un état, créer une nouvelle classe dérivant de celle la avec T l'objet qui utilisera l'état
/// </summary>
/// <typeparam name="T">Le type de l'objet de l'état</typeparam>
public class State<T>
{

    /// <summary>
    /// La state machine gérant les états de l'objet Owner
    /// </summary>
    protected StateMachine<T> _stateMachine;

    /// <summary>
    /// Permet d'accèder à l'objet utilisant l'état
    /// </summary>
    protected T Owner {get => _stateMachine.Owner;}

    public State(StateMachine<T> stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public virtual void enter(){}
    public virtual void exit(){}
    public virtual void update(){}
    public virtual void fixedUpdate(){}

    public override string ToString()
    {
        return this.GetType().Name;
    }
}
