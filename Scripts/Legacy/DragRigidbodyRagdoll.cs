using System;
using System.Collections;
using UnityEngine;

namespace UnityStandardAssets.Utility
{
    public class DragRigidbodyRagdoll : MonoBehaviour
    {
        const float k_Drag = 10.0f;
        const float k_AngularDrag = 5.0f;
        const bool k_AttachToCenterOfMass = false;

        //Array of the arms of the octopus
        public OctoArmRagdoll[] arms;
        public float thrust = 10.0f;
        public float maxDistance = 8.0f;

        //The arm that is currently active for being dragged (as the index for arms array)
        private int activeArm;

        private FixedJoint m_FixedJoint;
        private bool dragging;
        private Rigidbody playerRigidbody;

        private void Start()
        {
            //Init the first arm as the active arm
            activeArm = 0;
            
        }

        private void Update()
        {
            /*! Currently, when all arms in use, flips between 0 and 1, 
              needs to do a check for all arms being in use and lock to arm 0!

            ALSO! Currently the active arm is the one which is not clinging...
            but release button releases active arm? This means it can only 
            release arms if *all* arms are clinging...FIX! :)
            */

            // get the distance between the active arm and the body
            var distance = Vector3.Distance(arms[activeArm].transform.position, gameObject.transform.position);
            
            // check distance between active arm and body, if past max distance disconnect from the draggin object and delete dragging object, reset values
            if (distance >= maxDistance)
            {
                var go = GameObject.Find("Rigidbody dragger");
                Destroy(go);
                //arms[activeArm].ArmSpring.spring = arms[activeArm].defaultSpringValue;
                arms[activeArm].IsBeingDragged = false;
            }

            //Checks if the current active arm is clinging and moves to next arm if true. Loops back to first arm at end of array.
            if (arms[activeArm].IsClinging)
            {
                if (activeArm < (arms.Length - 1))
                {
                    activeArm++;
                }

                else
                {
                    activeArm = 0;
                }
            }

            //If clinging, release active arm on keypress (could be number keys for arms? Or a cylce through from last to first)
            if (Input.GetKey(KeyCode.Space) && (arms[activeArm].IsClinging == true))
            {
                arms[activeArm].Release();
            }

            if (Input.GetKey(KeyCode.W))
            {
                // add force here
                playerRigidbody = arms[activeArm].GetComponent<Rigidbody>();
                var worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3 (Input.mousePosition.x, Input.mousePosition.y, playerRigidbody.transform.position.z));
                var direction = worldMousePosition - playerRigidbody.transform.position;
                direction.Normalize();
                applyForceOnBody(direction , playerRigidbody);
            }

            //Debug.Log("active arm = " + activeArm);

            // Make sure the user pressed the mouse down
            if (!Input.GetMouseButtonDown(0))
            {
                return;
            }

            var mainCamera = FindCamera();

            // We need to actually hit an object
            RaycastHit hit = new RaycastHit();
            if (
                !Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition).origin,
                                 mainCamera.ScreenPointToRay(Input.mousePosition).direction, out hit, 100,
                                 Physics.DefaultRaycastLayers))
            {
                return;
            }

            if (hit.rigidbody != null)
            {
                // We need to hit a rigidbody that is not kinematic
                if (hit.rigidbody.isKinematic)
                {
                    return;
                }

                //If hitting the octopus body
                else if (hit.rigidbody.tag == "OctoBody")
                {
                    //Slingshot code will go here
                }

                //Hitting any other rigidbody
                else
                {
                    //Create new rigidbody to act as dragger tool and attach via springjoint.
                    attachSpringJoint(hit);

                    StartCoroutine("DragObject", hit.distance);
                }
            }

            //When dragging on the screen alone
            else
            {
                //Create new rigidbody to act as dragger tool and attach via springjoint.
                attachSpringJoint(hit);

                StartCoroutine("DragObject", hit.distance);
            }


            // Set active arm value to 0 so the arm can move without dragging the body
            if (!arms[activeArm].IsClinging)
            {
                //arms[activeArm].ArmSpring.spring = 0;
            }

            //StartCoroutine("DragObject", hit.distance);            
        }


        //Coroutine for dragging with mosue
        private IEnumerator DragObject(float distance)
        {
            arms[activeArm].IsBeingDragged = true;
            //Debug.Log("Being dragged = " + arms[activeArm].IsBeingDragged);

            var oldDrag = m_FixedJoint.connectedBody.drag;
            var oldAngularDrag = m_FixedJoint.connectedBody.angularDrag;
            //m_FixedJoint.connectedBody.drag = k_Drag;
            //m_FixedJoint.connectedBody.angularDrag = k_AngularDrag;
            var mainCamera = FindCamera();
            
            if (m_FixedJoint != null)
            {
                while (Input.GetMouseButton(0) && m_FixedJoint != null)
                {
                    var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                    m_FixedJoint.transform.position = ray.GetPoint(distance);
                    yield return null;
                }

                if (m_FixedJoint != null && m_FixedJoint.connectedBody)
                {
                    m_FixedJoint.connectedBody.drag = oldDrag;
                    m_FixedJoint.connectedBody.angularDrag = oldAngularDrag;
                    m_FixedJoint.connectedBody = null;
                }
            }
            

            // Set active arm spring value to default
            //arms[activeArm].ArmSpring.spring = arms[activeArm].defaultSpringValue;
            arms[activeArm].IsBeingDragged = false;
            Debug.Log("Being dragged = " + arms[activeArm].IsBeingDragged);
        }


        //Attaching a spring joint and "dragger" to drag things with mouse. Has problems!
        private void attachSpringJoint(RaycastHit hit)
        {
            //Create new rigidbody to act as dragger tool and attach via fixjoint.
            if (!m_FixedJoint)
            {
                var go = new GameObject("Rigidbody dragger");
                Rigidbody body = go.AddComponent<Rigidbody>();
                m_FixedJoint = go.AddComponent<FixedJoint>();
                body.isKinematic = true;
            }

            m_FixedJoint.transform.position = hit.point;
            m_FixedJoint.anchor = Vector3.zero;

            m_FixedJoint.connectedBody = arms[activeArm].ArmRigidbody;
        }

        private void detachSpringJoint(FixedJoint currentJoint)
        {
            //var go = GameObject.FindGameObjectWithTag("Rigidbody dragger");
            Destroy(currentJoint);
        }

        private Camera FindCamera()
        {
            if (GetComponent<Camera>())
            {
                return GetComponent<Camera>();
            }

            return Camera.main;
        }

        private void applyForceOnBody(Vector3 direction,Rigidbody body)
        {
            body.AddForce(direction * thrust);
        }

        private void addFixedJointToArm(Rigidbody fixOnto)
        {
            var go = arms[activeArm].GetComponent<GameObject>();
            m_FixedJoint = go.AddComponent<FixedJoint>();
            m_FixedJoint.connectedBody = fixOnto;
        }
    }
}
