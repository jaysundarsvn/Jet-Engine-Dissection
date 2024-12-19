using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TechXR.Core.Sense
{
    /// <summary>
    /// Grabbable Object
    /// Manages all the configurations required to make a 3D object grabbable by the controller
    /// </summary>
    //[RequireComponent(typeof(Rigidbody))]
    public class Grabbable : MonoBehaviour, IObject, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        #region PUBLIC FIELDS
        [Tooltip("To hold this object from a particular angle Make a new Empty GameObject as a child of this object give it the Direction and angle as you want and assign it to this Field")]
        public GameObject AnchorPoint;
        public Material highlightMaterial;
        #endregion // Public Fields
        //
 
        #region
        private Vector3 initial_position;
        private Quaternion initial_rotation;
        private bool is_released = true;
        private LabelManager labelManager;
        private Material originalMaterial;
        private static bool objInHand;
        #endregion

        #region MONOBEHAVIOUR METHODS
        void Start()
        {
            // Set the rigidbody property
            //this.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;
            initial_position = this.transform.localPosition;
            initial_rotation = this.transform.localRotation; 
            labelManager = FindObjectOfType<LabelManager>();
            originalMaterial = GetComponent<MeshRenderer>().material;
        }

        void Update()
        {
            if(is_released && Vector3.Distance(initial_position, transform.localPosition) < 1f)
            {
                if(initial_position != transform.localPosition || initial_rotation != transform.localRotation)
                {
                    transform.localPosition = Vector3.Lerp(transform.localPosition, initial_position, Time.deltaTime);
                    transform.localRotation = Quaternion.Lerp(transform.localRotation, initial_rotation, Time.deltaTime);

                }
            }    
        }
        #endregion // Monobehaviour Methods
        //
        #region PUBLIC METHODS
        /// <summary>
        /// Get local position of the object
        /// </summary>
        /// <returns></returns>
        public Vector3 GetObjectLocalPosition()
        {
            return this.gameObject.transform.localPosition;
        }

        /// <summary>
        /// Get local rotation of the object
        /// </summary>
        /// <returns></returns>
        public Quaternion GetObjectLocalRotation()
        {
            return this.gameObject.transform.localRotation;
        }

        /// <summary>
        /// Get global position of the object
        /// </summary>
        /// <returns></returns>
        public Vector3 GetObjectPosition()
        {
            return this.gameObject.transform.position;
        }

        /// <summary>
        /// Get global rotation of the object
        /// </summary>
        /// <returns></returns>
        public Quaternion GetObjectRotation()
        {
            return this.gameObject.transform.rotation;
        }

        /// <summary>
        /// Object grabbed event
        /// Dispatches SenseEvent.OBJECT_GRABBED
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerDown(PointerEventData eventData)
        {
            ControllerFactory.GetIXR().TriggerEvent(SenseEvent.OBJECT_GRABBED, this.gameObject, AnchorPoint);
            is_released = false;
            objInHand = true;
        }

        /// <summary>
        /// Object released event
        /// Dispatches SenseEvent.OBJECT_RELEASED
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerUp(PointerEventData eventData)
        {
            ControllerFactory.GetIXR().TriggerEvent(SenseEvent.OBJECT_RELEASED, this.gameObject);
            is_released = true;
            objInHand = false;
            labelManager.UpdateLabel(this.gameObject);
        }

        /// <summary>
        /// Object clicked event
        /// Dispatches SenseEvent.OBJECT_CLICKED
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerClick(PointerEventData eventData)
        {
            ControllerFactory.GetIXR().TriggerEvent(SenseEvent.OBJECT_CLICKED, this.gameObject);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if(objInHand == false)
            {
                GetComponent<MeshRenderer>().material = highlightMaterial;
                labelManager.UpdateLabel(this.gameObject);
            }

        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (objInHand == false)
            {
                GetComponent<MeshRenderer>().material = originalMaterial;
            }
        }


        #endregion // Public Methods
    }
}
