/*  Zjednoduseny a upraveny MouseLook z UnityStandardAssets
    src: https://assetstore.unity.com/packages/essentials/asset-packs/standard-assets-for-unity-2018-4-32351 */

using System;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[Serializable]
public class MouseLook_Custom {
    
    public float XSensitivity = 2f;
    public float YSensitivity = 2f;
    public float MinimumX = -90F;
    public float MaximumX = 90F;

    private Quaternion m_CharacterTargetRot;
    private Quaternion m_CameraTargetRot;
    
    private static Transform transformToFollow = null;
    private static float lookAtSpeed = 100f;
    
    public void Init(Transform character, Transform camera){ 
        m_CharacterTargetRot = character.localRotation;
        m_CameraTargetRot = camera.localRotation;
    }

    public void LookRotation(Transform character, Transform camera) {
        if (GameManager.paused) return; // pri pauznutej hre sa neda otacat

        if (transformToFollow == null) {
            float xRot = CrossPlatformInputManager.GetAxis("Mouse Y") * YSensitivity;
            float yRot = CrossPlatformInputManager.GetAxis("Mouse X") * XSensitivity;
            
            m_CharacterTargetRot *= Quaternion.Euler(0f, yRot, 0f);
            m_CameraTargetRot *= Quaternion.Euler(-xRot, 0f, 0f);
            m_CameraTargetRot = ClampRotationAroundXAxis(m_CameraTargetRot);
            
            character.localRotation = m_CharacterTargetRot;
            camera.localRotation = m_CameraTargetRot;
        }
        else {
            // relativna poloha pre character a kameru
            Vector3 targetDirection_Character = (transformToFollow.position - character.transform.position).normalized;
            Vector3 targetDirection_Camera = (transformToFollow.position - camera.transform.position).normalized;
            
            // vektor otocenia v smere osi X (kamera) a Y (character)
            m_CameraTargetRot = Quaternion.Euler(
                Quaternion.LookRotation(targetDirection_Camera).eulerAngles.x, 
                camera.localRotation.eulerAngles.y, 
                camera.localRotation.eulerAngles.z);
            
            m_CharacterTargetRot = Quaternion.Euler(
                character.localRotation.eulerAngles.x, 
                Quaternion.LookRotation(targetDirection_Character).eulerAngles.y, 
                character.localRotation.eulerAngles.z);
            
            // nastavenie rotacie kamery a charactera
            character.localRotation = Quaternion.RotateTowards(
                character.localRotation, 
                m_CharacterTargetRot, 
                lookAtSpeed*Time.deltaTime);
            
            camera.localRotation = Quaternion.RotateTowards(
                camera.localRotation, 
                m_CameraTargetRot, 
                lookAtSpeed*Time.deltaTime);
        }

        if (RotationFinished(character, camera))
            transformToFollow = null;
    }

    public static void SetTransformToFollow(GameObject obj, float speed) {
        transformToFollow = obj.transform;
        lookAtSpeed = speed;
    }
    

    private Boolean RotationFinished(Transform character, Transform camera) {
        return m_CameraTargetRot == camera.localRotation && m_CharacterTargetRot == character.localRotation;
    }

    Quaternion ClampRotationAroundXAxis(Quaternion q) {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);
        angleX = Mathf.Clamp(angleX, MinimumX, MaximumX);
        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }
    
}
