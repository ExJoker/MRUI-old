﻿using UnityEngine;
using HoloToolkit.Unity.SpatialMapping;

/// <summary>
/// Move Holograms to place them.
/// based on https://developer.microsoft.com/en-us/windows/mixed-reality/holograms_230
/// </summary>

namespace MRUi
{
    /// <summary>
    /// Enumeration containing the surfaces on which a GameObject
    /// can be placed.  For simplicity of this sample, only one
    /// surface type is allowed to be selected.
    /// </summary>
    public enum PlacementSurfaces
    {
        // Horizontal surface with an upward pointing normal.    
        Horizontal = 1,

        // Vertical surface with a normal facing the user.
        Vertical = 2,
    }

    public class MoveToPlace : MonoBehaviour
    {
        public PlacementSurfaces PlacementSurface = PlacementSurfaces.Horizontal;

        // Threshold (the closer to 1, the stricter the standard) used to determine if a surface is vertical.
        private float upNormalThreshold = 0.9f;

        [Tooltip("The distance away from the target surface that the object should hover prior while being placed.")]
        public float hoverDistance = 0.15f;

        // The most recent distance to the surface.  This is used to 
        // locate the object when the user's gaze does not intersect
        // with the Spatial Mapping mesh.
        private float lastDistance = 2.0f;

        // Speed (1.0 being fastest) at which the object settles to the surface upon placement.
        private float placementVelocity = 0.06f;

        // Use this for initialization
        void Start()
        {

        }

        /// <summary>
        /// Orients the object so that it faces the user.
        /// </summary>
        /// <param name="alignToVerticalSurface">
        /// If true and the object is to be placed on a vertical surface, 
        /// orient parallel to the target surface.  If false, orient the object 
        /// to face the user.
        /// </param>
        /// <param name="surfaceNormal">
        /// The target surface's normal vector.
        /// </param>
        /// <remarks>
        /// The aligntoVerticalSurface parameter is ignored if the object
        /// is to be placed on a horizontalSurface
        /// </remarks>
        private void OrientObject(bool alignToVerticalSurface, Vector3 surfaceNormal)
        {
            Quaternion rotation = Camera.main.transform.localRotation;

            // If the user's gaze does not intersect with the Spatial Mapping mesh,
            // orient the object towards the user.
            if (alignToVerticalSurface && (PlacementSurface == PlacementSurfaces.Vertical))
            {
                // We are placing on a vertical surface.
                // If the normal of the Spatial Mapping mesh indicates that the
                // surface is vertical, orient parallel to the surface.
                if (Mathf.Abs(surfaceNormal.y) <= (1 - upNormalThreshold))
                {
                    rotation = Quaternion.LookRotation(-surfaceNormal, Vector3.up);
                }
            }
            else
            {
                rotation.x = 0f;
                rotation.z = 0f;
            }

            gameObject.transform.rotation = rotation;
        }

        /// <summary>
        /// Positions the object along the surface toward which the user is gazing.
        /// </summary>
        /// <remarks>
        /// If the user's gaze does not intersect with a surface, the object
        /// will remain at the most recently calculated distance.
        /// </remarks>
        private void Move()
        {
            Vector3 moveTo = gameObject.transform.position;
            Vector3 surfaceNormal = Vector3.zero;
            RaycastHit hitInfo;

            if (SpatialMappingManager.Instance == null)
            {
                return;
            }

            bool hit = Physics.Raycast(Camera.main.transform.position,
                                    Camera.main.transform.forward,
                                    out hitInfo,
                                    20f,
                                    SpatialMappingManager.Instance.LayerMask);

            if (hit)
            {
                float offsetDistance = hoverDistance;

                // Place the object a small distance away from the surface while keeping 
                // the object from going behind the user.
                if (hitInfo.distance <= hoverDistance)
                {
                    offsetDistance = 0f;
                }

                moveTo = hitInfo.point + (offsetDistance * hitInfo.normal);

                lastDistance = hitInfo.distance;
                surfaceNormal = hitInfo.normal;
            }
            else
            {
                // The raycast failed to hit a surface.  In this case, keep the object at the distance of the last
                // intersected surface.
                moveTo = Camera.main.transform.position + (Camera.main.transform.forward * lastDistance);
            }

            // Follow the user's gaze.
            float dist = Mathf.Abs((gameObject.transform.position - moveTo).magnitude);
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, moveTo, placementVelocity / dist);

            // Orient the object.
            // We are using the return value from Physics.Raycast to instruct
            // the OrientObject function to align to the vertical surface if appropriate.
            OrientObject(hit, surfaceNormal);
        }

        // Update is called once per frame
        void Update()
        {
            Move();
        }
    }
}