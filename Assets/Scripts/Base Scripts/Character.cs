using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Models;
using UnityEngine;

namespace Assets.Scripts.Base_Scripts
{
    public abstract class Character : MonoBehaviour
    {
        public float baseDamage = 10;
        public float damageModifier = 1;
        public float hitPoints = 100;
        public bool dead = false;
        public bool chasing;


        protected float damage
        {
            get { return baseDamage * damageModifier; }
        }


        public abstract void ApplyDamage(Attack parameters);

        public abstract void Death(Character attacker);

        public void PlayerDead()
        {
            chasing = false;
        }

    }
}
