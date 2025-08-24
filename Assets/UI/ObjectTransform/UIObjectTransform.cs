using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DCG_UI
{
    public class UIObjectTransform : MonoBehaviour
    {
        private enum TF_CHANNEL
        {
            POS_X,
            POS_Y,
            POS_Z,
            ROT_X,
            ROT_Y,
            ROT_Z
        }

        [SerializeField]
        public float m_UIWidth = 500;

        [SerializeField]
        public float m_UIHeight = 70;

        [SerializeField]
        public ANCHOR_POSITION m_AnchorPosition = ANCHOR_POSITION.MIDDLE_RIGHT;

        [SerializeField]
        public GameObject m_positionX, m_positionY, m_positionZ;
        private TMP_InputField m_txtPosX, m_txtPosY, m_txtPosZ;

        [SerializeField]
        public GameObject m_rotationX, m_rotationY, m_rotationZ;
        private TMP_InputField m_txtRotX, m_txtRotY, m_txtRotZ;

        private RectTransform m_rectTransform;

        public ObjectSelector m_objectSelector;

        private Transform m_selectedObject;

        void OnEnable()
        {
            Init();
            m_txtPosX.onValueChanged.AddListener(EditFinished_PosX);
            m_txtPosY.onValueChanged.AddListener(EditFinished_PosY);
            m_txtPosZ.onValueChanged.AddListener(EditFinished_PosZ);
            m_txtRotX.onValueChanged.AddListener(EditFinished_RotX);
            m_txtRotY.onValueChanged.AddListener(EditFinished_RotY);
            m_txtRotZ.onValueChanged.AddListener(EditFinished_RotZ);

            m_objectSelector.OnObjectSelected += OnObjectSelected;
            m_objectSelector.OnObjectUnSelected += OnObjectUnSelected;
        }

        void OnDisable()
        {
            m_txtPosX.onValueChanged.RemoveListener(EditFinished_PosX);
            m_txtPosY.onValueChanged.RemoveListener(EditFinished_PosY);
            m_txtPosZ.onValueChanged.RemoveListener(EditFinished_PosZ);
            m_txtRotX.onValueChanged.RemoveListener(EditFinished_RotX);
            m_txtRotY.onValueChanged.RemoveListener(EditFinished_RotY);
            m_txtRotZ.onValueChanged.RemoveListener(EditFinished_RotZ);

            m_objectSelector.OnObjectSelected -= OnObjectSelected;
            m_objectSelector.OnObjectUnSelected -= OnObjectUnSelected;
        }

        private void OnObjectSelected(Transform obj)
        {
            m_selectedObject = obj;
            ActivateUI(true);
        }
        private void OnObjectUnSelected()
        {
            m_selectedObject = null;
            ActivateUI(false);
        }
        private void EditFinished_PosX(string text)
        {
            UpdateTransform(text, TF_CHANNEL.POS_X);
        }
        private void EditFinished_PosY(string text)
        {
            UpdateTransform(text, TF_CHANNEL.POS_Y);
        }
        private void EditFinished_PosZ(string text)
        {
            UpdateTransform(text, TF_CHANNEL.POS_Z);
        }
        private void EditFinished_RotX(string text)
        {
            UpdateTransform(text, TF_CHANNEL.ROT_X);
        }
        private void EditFinished_RotY(string text)
        {
            UpdateTransform(text, TF_CHANNEL.ROT_Y);
        }
        private void EditFinished_RotZ(string text)
        {
            UpdateTransform(text, TF_CHANNEL.ROT_Z);
        }

        void UpdateTransformUI(bool clearText = false)
        {
            m_txtPosX.SetTextWithoutNotify(clearText ? string.Empty : m_selectedObject.position.x.ToString("0.00"));
            m_txtPosY.SetTextWithoutNotify(clearText ? string.Empty : m_selectedObject.position.y.ToString("0.00"));
            m_txtPosZ.SetTextWithoutNotify(clearText ? string.Empty : m_selectedObject.position.z.ToString("0.00"));

            m_txtRotX.SetTextWithoutNotify(clearText ? string.Empty :
                                           GetEulerAngle(m_selectedObject.localEulerAngles.x).ToString("0.00"));
            m_txtRotY.SetTextWithoutNotify(clearText ? string.Empty :
                                          GetEulerAngle(m_selectedObject.localEulerAngles.y).ToString("0.00"));
            m_txtRotZ.SetTextWithoutNotify(clearText ? string.Empty :
                                          GetEulerAngle(m_selectedObject.localEulerAngles.z).ToString("0.00"));
        }

        void UpdateTransform(string text, TF_CHANNEL channel)
        {
            float val;
            Vector3 pos = m_selectedObject.position;
            Vector3 rot = m_selectedObject.rotation.eulerAngles;

            if (float.TryParse(text, out val))
            {
                switch (channel)
                {
                    case TF_CHANNEL.POS_X:
                        m_selectedObject.position = new Vector3(val, pos.y, pos.z);
                        break;
                    case TF_CHANNEL.POS_Y:
                        m_selectedObject.position = new Vector3(pos.x, val, pos.z);
                        break;
                    case TF_CHANNEL.POS_Z:
                        m_selectedObject.position = new Vector3(pos.x, pos.y, val);
                        break;
                    case TF_CHANNEL.ROT_X:
                        m_selectedObject.rotation = Quaternion.Euler(new Vector3(val, rot.y, rot.z));
                        break;
                    case TF_CHANNEL.ROT_Y:
                        m_selectedObject.rotation = Quaternion.Euler(new Vector3(rot.x, val, rot.z));
                        break;
                    case TF_CHANNEL.ROT_Z:
                        m_selectedObject.rotation = Quaternion.Euler(new Vector3(rot.x, rot.y, val));
                        break;
                }
            }
            UpdateTransformUI();
        }

        void Start()
        {
            m_rectTransform = GetComponent<RectTransform>();
            Vector2 UISizeDelta = new Vector2(m_UIWidth, m_UIHeight);
            Utilities.PositionUI(m_rectTransform, UISizeDelta, m_AnchorPosition);
            
            ActivateUI(false);
        }

        private void Init()
        {
            m_txtPosX = m_positionX.GetComponent<TMP_InputField>();
            m_txtPosY = m_positionY.GetComponent<TMP_InputField>();
            m_txtPosZ = m_positionZ.GetComponent<TMP_InputField>();

            m_txtRotX = m_rotationX.GetComponent<TMP_InputField>();
            m_txtRotY = m_rotationY.GetComponent<TMP_InputField>();
            m_txtRotZ = m_rotationZ.GetComponent<TMP_InputField>();
        }
        private void ActivateUI(bool active)
        {
            if (active)
            {
                m_txtPosX.interactable = true;
                m_txtPosX.ActivateInputField();

                m_txtPosY.interactable = true;
                m_txtPosY.ActivateInputField();

                m_txtPosZ.interactable = true;
                m_txtPosZ.ActivateInputField();

                m_txtRotX.interactable = true;
                m_txtRotX.ActivateInputField();

                m_txtRotY.interactable = true;
                m_txtRotY.ActivateInputField();

                m_txtRotZ.interactable = true;
                m_txtRotZ.ActivateInputField();

                UpdateTransformUI();
            }
            else
            {
                UpdateTransformUI(clearText: true);

                m_txtPosX.interactable = false;
                m_txtPosX.DeactivateInputField(true);

                m_txtPosY.interactable = false;
                m_txtPosY.DeactivateInputField(true);

                m_txtPosZ.interactable = false;
                m_txtPosZ.DeactivateInputField(true);

                m_txtRotX.interactable = false;
                m_txtRotX.DeactivateInputField(true);

                m_txtRotY.interactable = false;
                m_txtRotY.DeactivateInputField(true);

                m_txtRotZ.interactable = false;
                m_txtRotZ.DeactivateInputField(true);
            }
        }

        float GetEulerAngle(float angle)
        {
            if (angle > 180)
            {
                angle -= 360;
            }
            return angle;
        }

        void Update()
        {

        }
    }
}
