using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WalletUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text text_coins;
    [SerializeField]
    private TMP_Text text_tickets;
    // Start is called before the first frame update
    void Start()
    {
        WalletManager.Singleton.OnCoinsAdded += UpdateCoinsUI;
        WalletManager.Singleton.OnCoinsSubtracted += UpdateCoinsUI;
        WalletManager.Singleton.OnTicketsAdded += UpdateTickets;
    }

    private void UpdateTickets()
    {
        text_coins.text = RuntimeDBManager.instance.Coins.ToString();
    }

    private void UpdateCoinsUI()
    {
        text_tickets.text = RuntimeDBManager.instance.Tickets.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
