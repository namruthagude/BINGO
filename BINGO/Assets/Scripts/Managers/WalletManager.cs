using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalletManager : MonoBehaviour
{
    public event Action OnCoinsAdded;
    public event Action OnCoinsSubtracted;
    public event Action OnTicketsAdded;
    public event Action OnTicketsSubtracted;

    public static WalletManager Singleton;

    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }
    public void AddCoins(int value)
    {
        RuntimeDBManager.instance.Coins += value;
        OnCoinsAdded?.Invoke();
    }

    public void SubtractCoins(int value)
    {
        RuntimeDBManager.instance.Coins -= value;
        OnCoinsSubtracted?.Invoke();
    }

    public void AddTickets(int value)
    {
        RuntimeDBManager.instance.Tickets += value;
        OnTicketsAdded?.Invoke();
    }

    public void SubtractTickets(int value)
    {
        RuntimeDBManager.instance.Tickets -= value;
        OnTicketsSubtracted?.Invoke();
    }
}
