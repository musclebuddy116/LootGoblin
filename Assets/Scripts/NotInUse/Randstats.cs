/*using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Randstats
{
    [SerializeField] float[] stats;
    [SerializeField] int size;
    [SerializeField] float startTotal = 0;
    [SerializeField] float endTotal = 0;
    public Randstats(int x) {
        size = x;
        stats = new float[x];
    }

    /**
     * My first idea was the following, to initialize a list with all values the exact same that sum up to the desired total,
     * and then increase an index by a random value and decrease an index by a different value. But this tends to break Unity,
     * which is of course bad. I sought some help and was given the idea to use normalization, which is at the bottom and
     * does indeed seem to work.
     */
    
    /*public void Randomize(int seed) {
        Random.InitState(seed);
        //Sum to half length of list
        float sum = size/2f;
        float eachX = sum / size;
        float excess;
        for(int i = 0; i < size; i++) {
            stats[i] = eachX; //Init each element so that total begins as a sum to half size
        }
        for(int i = 0; i < size; i++) {
            startTotal += stats[i];
        }
        Debug.Log("startTotal: " + startTotal);

        for(int i = 0; i < size * 1000; i++) {
            if(size == 1) {
                break;
            }
            float change = Random.Range(0f,.1f); //Set random change variable
            excess = change;
            int j = 0;
            do {
                j++;
                int changeI = Random.Range(0,size); //Pick a random index to change
                stats[changeI] += excess; //Increase index by change
                if(stats[changeI] > 1f) { //Account for higher than 1, we need to spread the rest of change
                    excess = stats[changeI] - 1f;
                    stats[changeI] = 1f;
                } else {
                    excess = 0; //If not, excess is 0 so we're done
                }
            } while (excess > 0 && j < 5);
            excess = change - excess;
            j = 0;
            do {
                int changeI = Random.Range(0,size); //Pick a random index to change
                stats[changeI] -= excess; //Increase index by change
                if(stats[changeI] < 0f) { //Account for lower than 0, we need to spread the rest of change
                    excess = stats[changeI] * -1;
                    stats[changeI] = 0f;
                } else { excess = 0; } //If not, excess is 0 so we're done
            } while (excess > 0 && j < 5);
        }
        
        
        do {
            for(int i = 0; i < size; i++) {
            Debug.Log("stats[" + i + "]: " + stats[i]);
            endTotal += stats[i];
            }
            if(Math.Abs(endTotal - startTotal) > .0001) {
                excess = endTotal - startTotal;
                int j = 0;
                do {
                    j++;
                    int changeI = Random.Range(0,size); //Pick a random index to change
                    stats[changeI] -= excess; //Increase index by change
                    if(stats[changeI] > 1f) { //Account for higher than 1, we need to spread the rest of change
                        excess = stats[changeI] - 1;
                        stats[changeI] = 1f;
                    } else if(stats[changeI] < 0f) {
                        excess = stats[changeI] * -1;
                        stats[changeI] = 0f;
                    } else {
                        excess = 0; //If not, excess is 0 so we're done
                    }
                } while (excess > 0 && j < 5);
            }
        } while(Math.Abs(endTotal - startTotal) > .0001);

       Debug.Log("endTotal: " + endTotal);
    }*\/

    public void Randomize(int seed) {
        Random.InitState(seed);
        float sum = size/2f;
        float tempSum = 0;
        for(int i = 0; i < size; i++) {
            stats[i] = Random.Range(0f,1f);
            tempSum += stats[i];
        }
        float quotient = tempSum / sum;
        Debug.Log("Normalization quotient for size " + size + ": " + quotient);
        tempSum = 0;
        for(int i = 0; i < size; i++) {
            stats[i] /= quotient;
            tempSum += stats[i];
            Debug.Log("stats[" + i + "] for size " + size + ": " + stats[i]);
        }
        Debug.Log("Final sum for size " + size + ": " + tempSum);
    }
}*/