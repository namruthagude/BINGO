using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Economy", menuName ="SOObjects")]
public class EconomySO : ScriptableObject
{
    [Header("Daily Streak Rewards")]
    public int[] dailyStreakRewards;
    public int dailyReward;

    [Header("Match Rewards")]
    public int coinsPerWin;
    public int coinsPerLoss;

    [Header("Tickets")]
    public int startingTickets;
    public int maxTickets;
    public int ticketRegainTime;

    [Header("Ad Rewards")]
    public int rewardedAdCoins;
    public int rewardedAdTickets;
    [Range(0, 1)]
    public float interestialFrequency;

    [Header("Bonus Drop Rates")]
    public float bonusTicketChance = 0.15f;
    [Range(20,30)]
    public int bounceCoinRange;

}
