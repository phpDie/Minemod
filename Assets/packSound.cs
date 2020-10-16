using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class packSound : MonoBehaviour
{

    public AudioClip souItemGive;
    public AudioClip souClick;
    public AudioClip souChest;
  //  public AudioClip souChest;

    public AudioClip[] souDamage;

    public AudioClip[] souDigStone;
    public AudioClip[] souDigDirt;
    public AudioClip[] souDigWood;
    public AudioClip[] souDigGlass;

    

    public AudioClip[] souEat;
    public AudioClip[] souGunFire;
    public AudioClip[] souGunReload;
    public AudioClip[] souStep;

    public AudioClip[] souZomb;


    public AudioClip getDigSound(blockMaterial m = blockMaterial.all)
    {
        if (m == blockMaterial.ground) return getSound(souDigDirt);
        if (m == blockMaterial.wood) return getSound(souDigWood);
        if (m == blockMaterial.stone) return getSound(souDigStone);
        if (m == blockMaterial.glass) return getSound(souDigGlass);

        return getSound(souDigWood);
    }


    public AudioClip getSound(AudioClip[] s)
    {
        return s[Random.Range(0, s.Length - 1)];
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
