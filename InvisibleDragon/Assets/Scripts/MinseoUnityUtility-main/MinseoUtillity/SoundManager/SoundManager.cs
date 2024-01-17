using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : MonoSingleton<SoundManager>
{
    [SerializeField] private AudioClip[] BGM;
    [SerializeField] private AudioClip[] SFX;

    private Dictionary<String, AudioClip> BGMDic = new Dictionary<String, AudioClip>();
    private Dictionary<String, AudioClip> SFXDic = new Dictionary<String, AudioClip>();

    private AudioSource audioSource;
    private ObjectPool objectPool;

    protected override void Awake()
    {
        base.Awake();
        if(!TryGetComponent(out audioSource))
        {
            transform.AddComponent<AudioSource>();
            audioSource = GetComponent<AudioSource>();
        }
        objectPool = GetComponent<ObjectPool>();

        audioSource.loop = true;
        audioSource.playOnAwake = false;

        foreach(var bgm in BGM)
            BGMDic.Add(bgm.name, bgm);

        foreach (var sfx in SFX)
            SFXDic.Add(sfx.name, sfx);
    }

    public void PrintBGM(String name, Boolean isLoop = true)
    {
        if (BGMDic.ContainsKey(name))
        {
            audioSource.loop = isLoop;
            audioSource.clip = BGMDic[name];
            audioSource.Play();
        }
        else
        {
            throw new Exception($"there is no BGM : {name}");
        }
    }

    public void PrintSFX(String name)
    {
        if (SFXDic.ContainsKey(name))
        {
            SFXPlayer sfx = objectPool.SpawnObject("SFXPlayer").GetComponent<SFXPlayer>();
            sfx.PrintSFX(SFXDic[name]);
        }
        else
            throw new Exception($"ther is no SFX : {name}");
    }

    public void StopCurBGM()
    {
        audioSource.Stop();
    }
}
