using UnityEngine;

[System.Serializable]
public class Asset
{
    public string assetName;     // e.g. "Real Estate"
    public int price;            // e.g. 100
    public int yieldPercent;     // e.g. +50
    public bool isVolatile;      // e.g. Crypto true/false
    public Sprite icon;          // Asset UI icon
    public int priceToAutomate;
    public float cooldown; // Time between yield collections
}
