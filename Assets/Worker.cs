using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Worker : MonoBehaviour
    {
        public List<Work> currentWorks;
        public Collider2D collider;
        public Image photo;
        public Sprite face;
        public int A_Ability;
        public int P_Ability;
        public int T_Ability;

        public Worker(Sprite icon,int A, int P, int T)
        {
            face = icon;
            A_Ability = A;
            P_Ability = P;
            T_Ability = T;
        }

        private void Start()
        {
            collider.gameObject.GetComponentInChildren<Collider2D>();
            
        }
        
        
    }
