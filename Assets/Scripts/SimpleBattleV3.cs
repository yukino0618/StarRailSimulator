using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SimpleBattleV3
{
    public enum TurnState
    {
        WaitUserInput,
        AllMonsterRun,
        AutoAttack,
    }

    public class SimpleBattleV3 : MonoBehaviour
    {
        public static int DISTANCE = 300;

        List<Monster> monsters = new List<Monster>();
        private int turnNumber = 0;
        bool updateFlag = true;
        public TurnState turnState;
        Monster useMonster = null;

        public Button buttonA;
        public Button buttonB;

        // Start is called before the first frame update
        void Start()
        {
            Monster a = new Monster();
            a.name = "Apple";
            a.attack = 2;
            a.blood = 20;
            a.velocity = 100;
            a.distance = DISTANCE;

            Monster b = new Monster();
            b.name = "Banana";
            b.attack = 3;
            b.blood = 10;
            b.velocity = 300;
            b.distance = DISTANCE;

            Monster c = new Monster();
            c.name = "Ciallo";
            c.attack = 3;
            c.blood = 20;
            c.velocity = 100;
            c.distance = DISTANCE;

            //a.Execute = delegate
            //{
            //    Attack(a, b);
            //};

            b.Execute = delegate
            {
                Attack(b, a);
            };

            c.Execute = delegate
            {
                Attack(c, a);
            };

            monsters.Add(a);
            monsters.Add(b);
            monsters.Add(c);

            turnState = TurnState.AllMonsterRun;
        }

        // Update is called once per frame
        void Update()
        {
            while (updateFlag)
            {
                Debug.Log("round: " + turnNumber.ToString());
                if (monsters[0].blood <= 0)
                {
                    Debug.LogFormat("{0} is dead!!!", monsters[0].name);
                    updateFlag = false;
                    break;
                }
                if (monsters[1].blood <= 0)
                {
                    Debug.LogFormat("{0} is dead!!!", monsters[1].name);
                    updateFlag = false;
                    break;
                }

                switch (turnState)
                {
                    case TurnState.AllMonsterRun:
                        {
                            // get smallest time
                            float smallest_time = float.MaxValue;
                            foreach (var monster in monsters)
                            {
                                float currTime = monster.distance / monster.velocity;
                                if (currTime < smallest_time)
                                {
                                    smallest_time = currTime;
                                    useMonster = monster;
                                }
                            }
                            int time = Mathf.CeilToInt(smallest_time);

                            // change all monsters' distance
                            foreach (var monster in monsters)
                            {
                                int runDistance = Mathf.FloorToInt(time * monster.velocity);
                                monster.distance -= runDistance;
                            }

                            turnState = TurnState.AutoAttack;

                            break;
                        }

                    case TurnState.AutoAttack:
                        {
                            if (useMonster == monsters[0])
                            {
                                turnState = TurnState.WaitUserInput;
                                break;
                            }

                            // current monster's turn
                            useMonster.Execute?.Invoke();

                            useMonster.distance = DISTANCE;

                            turnState = TurnState.AllMonsterRun;
                            break;
                        }

                    case TurnState.WaitUserInput:
                        {
                            updateFlag = false;
                            buttonA.gameObject.SetActive(true);
                            buttonB.gameObject.SetActive(true);
                            buttonA.onClick.RemoveAllListeners();
                            buttonA.onClick.AddListener(delegate
                            {
                                updateFlag = true;
                                Attack(monsters[0], monsters[1]);
                                monsters[0].distance = DISTANCE;
                                turnState = TurnState.AllMonsterRun;
                                buttonA.gameObject.SetActive(false);
                                buttonB.gameObject.SetActive(false);
                            });
                            buttonB.onClick.RemoveAllListeners();
                            buttonB.onClick.AddListener(delegate
                            {
                                updateFlag = true;
                                Attack(monsters[0], monsters[2]);
                                monsters[0].distance = DISTANCE;
                                turnState = TurnState.AllMonsterRun;
                                buttonA.gameObject.SetActive(false);
                                buttonB.gameObject.SetActive(false);
                            });
                            break;
                        }
                }

                turnNumber++;
            }
        }

        private void Attack(Monster from, Monster to)
        {
            Debug.LogFormat("{0} attack {1} ! damage is: {2}, blood: {3} -> {4}",
                    from.name, to.name, from.attack, to.blood, to.blood - from.attack);
            to.blood -= from.attack;
        }
    }

    public class Monster
    {
        public string name;
        public int attack;
        public int blood;
        public float velocity;
        public int distance;

        public int Vecolicy => Mathf.FloorToInt(velocity);

        public Action Execute { get; set; }
    }
}
