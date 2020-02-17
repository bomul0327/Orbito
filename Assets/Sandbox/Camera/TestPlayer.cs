﻿using UnityEngine;

namespace Experimental
{
    /// <summary>
    /// Test class for testing camera controller features.
    /// </summary>
    public class TestPlayer : MonoBehaviour
    {
        [SerializeField] CameraController camController;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetButton("Fire1"))
            {
               transform.position = CameraUtils.MouseToWorldPos(camController.MainCamera, Input.mousePosition);
            }
        }
    }
}