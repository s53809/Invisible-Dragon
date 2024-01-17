using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public delegate void MusicPlayHandler();

public class HallwaySpawner : MonoGameEvent
{
    public Single elevatorTime = 7f;
    public Single cameraShake = 1f;
    public Single hallwaySpeed = 1f;
    public Single hallwayLastxPosition = -21.5f;
    public Single hallwayFirstxPosition = 30.5f;
    
    public event MusicPlayHandler musicPlayHandler;

    [SerializeField] private Transform m_elevator;
    [SerializeField] private Sprite[] hallway_sprites;
    [SerializeField] private Transform m_endElevator;
    private List<Transform> m_hallway = new List<Transform>();
    private Boolean m_isEventStart;
    private Boolean m_isEventEnd = false;
    private void Start()
    {
        m_hallway.Add(transform.GetChild(1).GetChild(0));
        m_hallway.Add(transform.GetChild(1).GetChild(1));
    }
    [InspectorButton("Game Start")]
    public override void EventStart()
    {
        SoundManager.Ins.PrintSFX("longElevator");
        StartCoroutine(Shake(cameraShake, elevatorTime));
        transform.DOMoveY(0, elevatorTime).OnComplete(() =>
        {
            musicPlayHandler();
            m_elevator.DOMoveX(-25, 2).SetEase(Ease.Linear);
            transform.DOMoveX(-6, 2).SetEase(Ease.Linear).OnComplete(() =>
            {
                m_isEventStart = true;
            });
        });
    }

    [InspectorButton("Game End")]
    public override void EventStop()
    {
        m_isEventEnd = true;
    }

    private void NonEventUpdate()
    {

    }
    private void EventUpdate()
    {
        foreach(var t in m_hallway)
        {
            t.position = t.position + new Vector3(-hallwaySpeed * Time.deltaTime, 0, 0);
            if(t.position.x <= hallwayLastxPosition)
            {
                t.position = new Vector3(hallwayFirstxPosition, t.position.y, t.position.z);
                t.GetComponent<SpriteRenderer>().sprite = hallway_sprites[
                    UnityEngine.Random.Range(0, hallway_sprites.Length)
                    ];
                if (m_isEventEnd)
                {
                    m_isEventStart = false;
                    transform.DOMoveX(-53, 5f);
                }
            }
        }
    }

    private void Update()
    {
        NonEventUpdate();
        if (!m_isEventStart) return;
        EventUpdate();
    }


    private IEnumerator Shake(float ShakeAmount, float ShakeTime)
    {
        float timer = 0;
        while (timer <= ShakeTime)
        {
            Camera.main.transform.position =
                (Vector3)UnityEngine.Random.insideUnitCircle * ShakeAmount
                + new Vector3(0, 0, -10);
            timer += Time.deltaTime;
            yield return null;
        }
        Camera.main.transform.position = new Vector3(0f, 0f, -10f);
    }
}
