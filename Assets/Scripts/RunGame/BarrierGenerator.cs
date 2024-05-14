
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class BarrierGenerator : MonoBehaviour
{
    public RoadGenerator _roadGenerator;
    public barrierControl BarrierPrefab;
  
    
    public GameObject CoinPrefab;
    private List<barrierControl> barriers = new List<barrierControl>();
    private List<GameObject> coins = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
        ResetLevel();
       
    }

    // Update is called once per frame
    void Update()
    {
        
        if (_roadGenerator.speed == 0) return;
        foreach (barrierControl barrier in barriers)
        {
            /*if (barrier.chekBarrier == false)
            {
                
                if(wait < 5)
                {
                    wait += Time.deltaTime;
                    img.SetActive(true);
                }
                else
                {
                    wait = 0;
                    img.SetActive(false);
                }
                

            }*/
            barrier.transform.position -= new Vector3(0, 0, _roadGenerator.maxSpeed * Time.deltaTime);
        }
        foreach (GameObject coin in coins)
        {
            coin.transform.position -= new Vector3(0, 0, _roadGenerator.maxSpeed * Time.deltaTime);
        }

        if (barriers[0].transform.position.z < -15)
        {
            Destroy(barriers[0]);
            barriers.RemoveAt(0);
          
            CreateNextBarrier();
        }


        if (coins[0].transform.position.z < -15)
        {
            Destroy(coins[0]);
            coins.RemoveAt(0);

            CreateNextCoins();
        }
    }

    private bool SortBarrier(List<GameObject> coins, Vector3 position)
    {
        bool flag = true;
        foreach (GameObject coin in coins)
        {
            if (coin.transform.position == position) flag = false;
        }
        return flag;
    }


    private void CreateNextBarrier()
    {
        Vector3 pos = new Vector3(0, 0.35f, 40);
        if (barriers.Count > 0)
        {
            int x = Random.Range(0, 4);
            switch (x)
            {
                case 0: 
                    pos = new Vector3(-4, 0.35f, 40);
                    break;
                case 1:
                    pos = new Vector3(0, 0.35f, 40);
                    break;
                case 2:
                    pos = new Vector3(4, 0.35f, 40);
                    break;
                case 3:
                    pos = new Vector3(4, 0.35f, 40);
                    break;
                
            }
            
        }

            barrierControl go = Instantiate<barrierControl>(BarrierPrefab, pos, Quaternion.identity);
            go.transform.SetParent(transform);
            barriers.Add(go);
        
    }

    private bool SortCoins(List<barrierControl> barriers, Vector3 position)
    {
        bool flag = true;
        foreach (barrierControl barrier in barriers)
        {
           if( barrier.transform.position == position) flag=false;
        }
        return flag;
    }

    

    private void CreateNextCoins()
    {
        Vector3 pos = new Vector3(3, 1, 30);
        if (coins.Count > 0)
        {
            int x = Random.Range(0, 3);
            switch (x)
            {
                case 0:
                    pos = new Vector3(-3, 1, 30);
                    break;
                case 1:
                    pos = new Vector3(0, 1, 30);
                    break;
                case 2:
                    pos = new Vector3(3, 1, 30);
                    break;
                case 3:
                    pos = new Vector3(3, 1, 30);
                    break;

            }

        }
        
            GameObject go = Instantiate(CoinPrefab, pos, Quaternion.identity);
            go.transform.SetParent(transform);
            coins.Add(go);
        
    }

    public void ResetLevel()
    {
        

        while (barriers.Count > 0)
        {
            Destroy(barriers[0]);
            barriers.RemoveAt(0);
        }
        for (int i = 0; i < 2; i++)
        {
            CreateNextBarrier();
        }

        while (coins.Count > 0)
        {
            Destroy(coins[0]);
            coins.RemoveAt(0);
        }
        int n = Random.Range(1, 3);
        for (int i = 0; i < n; i++)
        {
            CreateNextCoins();
        }
        
    }
}
