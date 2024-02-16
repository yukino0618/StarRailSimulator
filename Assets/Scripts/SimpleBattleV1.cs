using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleBattleV1
{
    public class SimpleBattleV1 : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            Foo();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void Foo()
        {
            Monster A = new Monster()
            {
                name = "A",
                attack = 2,
                blood = 20,
            };
            Monster B = new Monster()
            {
                name = "B",
                attack = 3,
                blood = 10,
            };

            int round = 0;
            while (true)
            {
                Debug.Log("round: " + round.ToString());
                if (A.blood <= 0)
                {
                    Debug.LogFormat("{0} is dead!!!", A.name);
                    break;
                }
                if (B.blood <= 0)
                {
                    Debug.LogFormat("{0} is dead!!!", B.name);
                    break;
                }
                if (round % 2 == 0)
                {
                    B.blood -= A.attack;
                    Debug.LogFormat("{0} attack {1}, attack is {2}, blood: {3} -> {4}",
                        A.name, B.name, A.attack, B.blood + A.attack, B.blood);
                }
                else
                {
                    A.blood -= B.attack;
                    Debug.LogFormat("{0} attack {1}, attack is {2}, blood: {3} -> {4}",
                        B.name, A.name, B.attack, A.blood + B.attack, A.blood);
                }
                round++;
            }
        }

        public class Monster
        {
            public string name;
            public int attack;
            public int blood;
        }
    }
}
