using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Randstats randstats = new Randstats(1);
        randstats.Randomize(Random.Range(int.MinValue, int.MaxValue));
        Randstats randstats1 = new Randstats(10);
        randstats1.Randomize(Random.Range(int.MinValue, int.MaxValue));
        Randstats randstats2 = new Randstats(100);
        randstats2.Randomize(Random.Range(int.MinValue, int.MaxValue));
        Randstats randstats3 = new Randstats(1000);
        randstats3.Randomize(Random.Range(int.MinValue, int.MaxValue));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
