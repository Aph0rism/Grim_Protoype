using System;
using System.Collections.Generic;
using Settings;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace GameManagement
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private int actionLength = 1000;
        [SerializeField] private int enemyAction = 0;
        [SerializeField] private bool isPlayerMoving = false;
        [SerializeField] private Queue<Action> actionQueue;
        [SerializeField] private Rigidbody rb;
        
        private readonly Tags _tags = new Tags();

        private void Awake()
        {
            // initialisation de la file et du rigidbody du joueur
            actionQueue = new Queue<Action>();
            if (rb == null)
            {
                rb = GameObject.FindGameObjectWithTag(_tags.PLAYER_TAG).GetComponent<Rigidbody>();
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                // message de débug pour voir les éléments importants dans la console
                Debug.Log("isPlayerMoving = " + isPlayerMoving + " enemyAction = " + enemyAction);
                PrintQueue();
            }
            if (Input.GetKeyDown(KeyCode.Q) && enemyAction <= 0)
            {
                // gestion de l'attaque (donne du temps d'action à l'ennemi) et ajout de l'action à la file
                enemyAction += actionLength;
                    
                Action playerAttack = new Action("Player", "PlayerAttack", "Player Attacking", actionLength);
                actionQueue.Enqueue(playerAttack);
                    
                Debug.Log("attacking, the enemies are gaining action time");
            }
        }

        void FixedUpdate() // frame fixées
        {
            isPlayerMoving = rb.linearVelocity != Vector3.zero;

            if (enemyAction > 0)
            {
                // diminution de l'action ennemie avec le temps
                enemyAction -= 1;
            }
        }

        private void PrintQueue() // affiche les actions de la file dans la console
        {
            foreach (Action action in actionQueue)
            {
                Debug.Log(action.Name + ", " + action.Description);
            }
        }
    }
}
