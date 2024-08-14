using nazaaaar.platformBattle.mini.model;
using nazaaaar.slime.mini.view;
using RMC.Mini;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;

namespace nazaaaar.slime.mini{

    public struct SlimeInitializer
    {
        private Team team;

        public Quaternion Rotation { get; private set; }

        public SlimeInitializer(Team team)
        {
            this.team = team;

            Rotation = Quaternion.identity;

            if (team == Team.Red)
            {
                Rotation = Quaternion.Euler(0, 180, 0);
            }
        }

        public SlimeMVC Initialize(MonsterSO monsterSO, NetworkObject monsterView, IContext context)
        {
            var slimeView = monsterView.gameObject.AddComponent<slime.mini.view.SlimeView>();

            var slimeAnimation = monsterView.gameObject.AddComponent<SlimeAnimation>();

            slimeAnimation.Animator = slimeView.gameObject.GetComponent<Animator>();

            slimeView.CharacterController = slimeView.gameObject.GetComponent<CharacterController>();

            slimeView.CharacterController.enabled = true;

            var slimeFinder = slimeView.gameObject.GetComponent<SlimeFinder>();

            SlimeMVC slimeMVC = new(context, slimeView, slimeFinder,slimeAnimation, monsterSO, team);
            slimeMVC.Initialize();

            return slimeMVC;
        }
    }
}