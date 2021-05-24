using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class money : MonoBehaviour
{
    public TextMeshProUGUI display;
    public int mymoney;

    // Start is called before the first frame update
    void Start()
    {
        mymoney = 0;
        StartCoroutine(givemoney());
    }

    // Update is called once per frame
    void Update()
    {
        if (display != null)
        {
            display.text = "Money: " + mymoney.ToString();
        }
    }

    public void addMoney(int moneytoadd)
    {
        mymoney += moneytoadd;
    }

    public void subtractMoney(int moneytosubtract)
    {
        if (mymoney - moneytosubtract <= 0)
        {
            mymoney = 0;
            Debug.Log("no money lol");
        }
        else
        {
            mymoney -= moneytosubtract;
        }
    }

    IEnumerator givemoney()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            addMoney(2);
        }
    }
}
