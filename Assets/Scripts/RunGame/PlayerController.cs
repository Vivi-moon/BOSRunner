using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : Sounds
{
    public TextMeshProUGUI coinGUI;
    public RoadGenerator _roadGenerator;
    private int count=0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetLevel() {
        _roadGenerator.ResetLevel();
    }

    public void counterCoins()
    {
        count++;
        coinGUI.text = count.ToString();
        PlaySound(sounds[2], 0.1f);
    }

}
