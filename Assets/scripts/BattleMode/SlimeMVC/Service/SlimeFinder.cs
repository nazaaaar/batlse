using UnityEngine;
using System.Collections;
using RMC.Mini.Service;
using RMC.Mini;
using System;
using nazaaaar.platformBattle.mini.model;
using nazaaaar.slime.mini.controller;
using System.Collections.Generic;

public class SlimeFinder : MonoBehaviour, IService
{
    public bool IsInitialized{get; private set;}

    public IContext Context{get; private set;}

    private Coroutine routine;

    public Action<IMonster> OnMonsterFound;

    private IMonster myMonster;

    private List<IMonster> monstersList;

    private float agroRangeSqr;

    void Search()
    {
        foreach (var monster in monstersList)
        {
            var diff = (myMonster.Position-monster.Position).sqrMagnitude;
            Debug.Log(myMonster.Team +" " + diff + " " + agroRangeSqr);
            if (diff <= agroRangeSqr){
                OnMonsterFound?.Invoke(monster);
            }
        }
    }

    IEnumerator PeriodicSearch(float delay)
    {
        while (true)
        {
            Search();
            yield return new WaitForSeconds(delay);
        }
    }

    public void StartSearchCoroutine(float delay)
    {
        routine = StartCoroutine(PeriodicSearch(delay));
    }

    void OnEnable()
    {
        StartSearchCoroutine(0.25f);
    }

    void OnDisable()
    {
        StopCoroutine(routine);
    }

    public void Initialize(IContext context)
    {
        if (!IsInitialized){
            IsInitialized = true;
            Context = context;
        }
    }

    public void StartSearching(IMonster mySlime, List<IMonster> monstersList, float agroRange){
        myMonster = mySlime;
        this.monstersList = monstersList;        
        this.agroRangeSqr = agroRange*agroRange;
        
        this.enabled=true;
    }

    public void RequireIsInitialized()
    {
        if (!IsInitialized){
            throw new System.Exception("MustBeInitialized");
        }
    }
}
