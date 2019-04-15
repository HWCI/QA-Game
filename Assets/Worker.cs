using System.Collections.Generic;
using UnityEngine;


    public class Worker : MonoBehaviour
    {
        public List<Work> currentWorks;
        public Collider2D collider;
        public float A_Ability;
        public float P_Ability;
        public float T_Ability;

        private void Start()
        {
            collider.gameObject.GetComponentInChildren<Collider2D>();
        }
        
        
    }
