using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdManager : MonoBehaviour
{
    public static AdManager Singleton;

    public enum ADTYPE
    {
        REWARDED_COINS,
        INTERSTATIAL,
        REWARDED_TICKETS,
        DOUBLE_REWARDS
    }

    private void Awake()
    {
        if(Singleton == null)
        {
            Singleton = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayAD(ADTYPE type , int reward = 0)
    {
        switch (type)
        {
            case ADTYPE.REWARDED_COINS:
                break;
            case ADTYPE.INTERSTATIAL:
                break;
            case ADTYPE.DOUBLE_REWARDS:
                break;
            case ADTYPE.REWARDED_TICKETS:
                break;
            default:
                break;
        }
    }
}
