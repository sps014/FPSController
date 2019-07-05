using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace  Sonic.FPS
{
    public class FpsCamera : MonoBehaviour
    {
        //Mouse Sensitivity for Both the Axis 
        public float MouseSensitivity = 150;

        //Root Player Body needed for its Rotation on Mouse Movements
        public Transform PlayerRoot;

        //Minimum And Maximum Values On MouseY to Clamp Rotation
        public float MouseYMin = -60;
        public float MouseYMax = 60;


        //Keeps Track of the rotation on the MouseY Axis
        private float xAxisClamp;

        private float mouseX;
        private float mouseY;

        void Start()
        {
            //Initialize Value To Zero
            xAxisClamp = 0;
        }

        void FixedUpdate()
        {
            //Get the values of the Mouse Movement on the X And Y Axis 
            mouseX = Input.GetAxis(AllAxisAndButtons.MouseX) * MouseSensitivity * Time.deltaTime;
            mouseY = Input.GetAxis(AllAxisAndButtons.MouseY) * MouseSensitivity * Time.deltaTime;

            //Update the xAxisClamp
            xAxisClamp += mouseY;

            //Clamp Rotations and Modify Moude Y 
            GetClampedValues(ref mouseY);

            //Rotate Camera Root On XAxis
            transform.Rotate(Vector3.left * mouseY);

            //Rotate Player Root On XAxis
            PlayerRoot.transform.Rotate(Vector3.up * mouseX);
        }
        void GetClampedValues(ref float mouseY)
        {

            //Check Mouse Bounds if the are in a given Range 

            //Modify MouseY to zero to stop Rotation

            if (xAxisClamp > MouseYMax)
            {
                xAxisClamp = MouseYMax;
                mouseY = 0;

                //Clamp Euler X Axis , +ve is 360-x 
                ClampXMouseAxisToValue(360 - MouseYMax);
            }
            else if (xAxisClamp < MouseYMin)
            {
                xAxisClamp = MouseYMin;
                mouseY = 0;

                //Clamp Euler X Axis,-ve axis is +x
                ClampXMouseAxisToValue(Mathf.Abs(MouseYMin));

            }
        }


        //Set current Euler rotation and Modify the x Axis and Assign to current object
        void ClampXMouseAxisToValue(float value)
        {
            Vector3 eulers = transform.eulerAngles;
            eulers.x = value;
            transform.eulerAngles = eulers;
        }
    }

}