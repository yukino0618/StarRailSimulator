using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleBattleV2
{
    public class SimpleBattleV2 : MonoBehaviour
    {
        public static int DISTANCE = 300;

        // Start is called before the first frame update
        void Start()
        {
            List<Monster> monsters = new List<Monster>();

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

            a.Execute = delegate
            {
                Attack(a, b);
            };

            b.Execute = delegate
            {
                Attack(b, a);
            };

            monsters.Add(a);
            monsters.Add(b);

            int round = 0;
            while (true)
            {
                Debug.Log("round: " + round.ToString());
                if (a.blood <= 0)
                {
                    Debug.LogFormat("{0} is dead!!!", a.name);
                    break;
                }
                if (b.blood <= 0)
                {
                    Debug.LogFormat("{0} is dead!!!", b.name);
                    break;
                }

                // get smallest time
                float smallest_time = float.MaxValue;
                Monster useMonster = null;
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

                // current monster's turn
                useMonster.Execute?.Invoke();

                useMonster.distance = DISTANCE;

                round++;
            }
        }

        // Update is called once per frame
        void Update()
        {

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
