using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityDevTools.Common
{
    public class SmoothDampTransform : MonoBehaviour
    {
        #region data

#pragma warning disable CS0649

        [SerializeField]
        private bool m_isWorkingInLocalCordinates = false;
        [SerializeField]
        private float m_smoothTimeMove = 0.3F;
        [SerializeField]
        private float m_maxSpeedMove = int.MaxValue;
        [SerializeField]
        private float m_smoothTimeRotation = 0.3F;

#pragma warning restore CS0649

        private Vector3 m_velocity = Vector3.zero;
        private Quaternion m_deriv = Quaternion.identity;

        #endregion

        #region interface

        public Vector3 TargetPosition { get; set; }
        public Quaternion TargetRotation { get; set; }

        public void ForceApply()
        {
            transform.position = TargetPosition;
            transform.rotation = TargetRotation;
            m_velocity = Vector3.zero;
            m_deriv = Quaternion.identity;
        }

        #endregion

        #region MonoBehaviour

        private void LateUpdate()
        {
            if (m_isWorkingInLocalCordinates)
            {
                transform.localPosition = Vector3.SmoothDamp(transform.localPosition, TargetPosition, ref m_velocity, m_smoothTimeMove, m_maxSpeedMove);
                transform.localRotation = QuaternionUtil.SmoothDamp(transform.localRotation, TargetRotation, ref m_deriv, m_smoothTimeRotation);
            }
            else
            {
                transform.position = Vector3.SmoothDamp(transform.position, TargetPosition, ref m_velocity, m_smoothTimeMove, m_maxSpeedMove);
                transform.rotation = QuaternionUtil.SmoothDamp(transform.rotation, TargetRotation, ref m_deriv, m_smoothTimeRotation);
            }
        }

        #endregion

        #region implementation

        private void Awake()
        {
            if (m_isWorkingInLocalCordinates)
            {
                TargetPosition = transform.localPosition;
                TargetRotation = transform.localRotation;
            }
            else
            {
                TargetPosition = transform.position;
                TargetRotation = transform.rotation;
            }
        }

        #endregion
    }
}