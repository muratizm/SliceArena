using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShopSytem
{
    public class Rotate : MonoBehaviour
    {
        [SerializeField]
        private float rotSpeed;


        void Update()
        {
            transform.Rotate(0, rotSpeed * Time.deltaTime, 0);
        }
    }
    
}


