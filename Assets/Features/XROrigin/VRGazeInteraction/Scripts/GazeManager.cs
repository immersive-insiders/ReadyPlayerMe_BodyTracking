using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace VRGazeInteraction
{
    public enum GazeState { Inactive, Observed, Activated }

    /// <summary>
    /// Core class for GazeInteraction. This class shoots the ray and invokes the 
    /// Event on the GazeEvents class which has to be attached ob interactable objects.
    /// </summary>
    public class GazeManager : MonoBehaviour
    {
        [Tooltip("What layer do you want your objects to be interactable")]
        public LayerMask layer;
        [Tooltip("Mostly for debug purposes: Do you want to show/hide the gaze?")]
        public bool showRay = true;
        [Tooltip("For observing what gameObject is currently gazed at")]
        public GameObject currentGazedObject;

        private float timer;


        void Update()
        {
            ShootRayCast();
        }

        /// <summary>
        /// The ray cast which get's shot every frame that represents the gaze
        /// </summary>
        public void ShootRayCast()
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit))
            {
                // Bit shift operation, so that we only work with the layer specified
                if (1 << hit.transform.gameObject.layer != layer.value) return;

                if (showRay)
                    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.green);


                if (hit.transform.gameObject != currentGazedObject && currentGazedObject != null)
                    Reset();


                currentGazedObject = hit.transform.gameObject;


                if (!HasGazeEventsComponent()) return;

                if (currentGazedObject.GetComponent<GazeEvents>().gazeState == GazeState.Inactive)
                {
                    StopAllCoroutines();
                    StartCoroutine(GazeActivationRoutine());
                }
            }
            else
            {
                if (showRay)
                    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);

                if (currentGazedObject == null) return;

                //if (currentGazedObject.GetComponent<GazeEvents>().gazeState == GazeState.Activated)
                //{
                    StopAllCoroutines();
                    Reset();
                //}
            }
        }

        /// <summary>
        /// The routine that get's called when you gaze at a inactive interactable object.
        /// Immediately invokes OnGazeEnter and after activationTime is over invokes OnGazeActivated
        /// </summary>
        /// <returns></returns>
        public IEnumerator GazeActivationRoutine()
        {
            timer = 0;
            currentGazedObject.GetComponent<GazeEvents>().gazeState = GazeState.Observed;
            currentGazedObject.GetComponent<GazeEvents>().OnGazeEnter.Invoke(currentGazedObject);

            while (timer <= currentGazedObject.GetComponent<GazeEvents>().activationTime)
            {
                timer += Time.deltaTime;
                yield return null;
            }

            currentGazedObject.GetComponent<GazeEvents>().OnGazeActivated.Invoke(currentGazedObject);
            currentGazedObject.GetComponent<GazeEvents>().gazeState = GazeState.Activated;
        }


        /// <summary>
        /// Get's called when you look at a new interactable object or when you look away from an active object
        /// Invokes OnGazeLeft
        /// </summary>
        public void Reset()
        {
            timer = 0;
            currentGazedObject.GetComponent<GazeEvents>().gazeState = GazeState.Inactive;
            currentGazedObject.GetComponent<GazeEvents>().OnGazeLeft.Invoke(currentGazedObject);
            currentGazedObject = null;
        }

        /// <summary>
        /// Errorhandling: When user forgets to add GazeEventsComponent to interactable object then he'll get notified. 
        /// </summary>
        /// <returns></returns>
        bool HasGazeEventsComponent()
        {
            if (currentGazedObject.GetComponent<GazeEvents>() == null)
            {
                Debug.LogError("Jo Dude, you need to add the GazeEvents component if you want to make it gaze interactable");
                currentGazedObject = null;
                return false;
            }
            return true;
        }
    }
}