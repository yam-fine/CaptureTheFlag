using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportManager : Singleton<TeleportManager>
{
   public XROrigin _xrOrigin;
   [SerializeField] private TeleportationProvider tp;
   [SerializeField] private InputActionAsset _actionAsset;
   [SerializeField] private XRRayInteractor _rayInteractor;
   public GameObject rightHandWithRay;
   public GameObject rightHandWithDirect;

   private InputAction _thumbstick;
   private bool _isActive;

   protected override void Awake()
   {
      base.Awake();
      tp.enabled = false;
   }

   public void Start()
   {
      _rayInteractor.enabled = false;
      var activate = _actionAsset.FindActionMap("XRI RightHand").FindAction("Teleport Mode Activate");
      activate.Enable();
      activate.performed += OnTeleportActivate;
      
      var cancel = _actionAsset.FindActionMap("XRI RightHand").FindAction("Teleport Mode Cancel");
      cancel.Enable();
      cancel.performed += OnTeleportCancel;
      
      _thumbstick = _actionAsset.FindActionMap("XRI RightHand").FindAction("Move1");
      _thumbstick.Enable();
   }

   public void ManualTeleport(Transform to)
   {
      float player_height = Math.Abs(_xrOrigin.transform.position.y - to.position.y);
      Vector3 new_transform = new Vector3(to.position.x, to.position.y + player_height, to.position.z);
      _xrOrigin.transform.position = new_transform;
     

   }

   public void ActivateTeleportation(bool activate = true)
   {
      tp.enabled = activate;
   }

   public void Update()
   {
      if (!_isActive)
      {
         return;
      }

      if (_thumbstick.ReadValue<Vector2>() != Vector2.zero)
      {
        
         return;
      }
      
      if (!_rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
      {
         _rayInteractor.enabled = false;
         _isActive = true;
         SwitchToGrab();
         return;
      }
      
      TeleportRequest request = new TeleportRequest()
      {
         destinationPosition = hit.point,

      };
      if(hit.transform is null)
         return;
      if (tp.enabled && hit.transform.CompareTag("Floor"))
      {
         tp.QueueTeleportRequest(request);
         // AudioManager.Instance.PlaySound(AudioManager.Sounds.Teleport);
         
      }
      else
      {
         // AudioManager.Instance.PlayOneShot(AudioManager.Sounds.BallWrong);
      }
      
      _rayInteractor.enabled = false;
      _isActive = false;
      SwitchToGrab();
   }

   public void OnTeleportActivate(InputAction.CallbackContext context)
   {
      SwitchToTeleport();
      _rayInteractor.enabled = true;
      _isActive = true;
   }

   public void OnTeleportCancel(InputAction.CallbackContext context)
   {
      _rayInteractor.enabled = false;
      SwitchToGrab();
      _isActive = false;
   }

   private void SwitchToTeleport()
   {
      rightHandWithRay.SetActive(true);
      rightHandWithDirect.SetActive(false);
   }

   private void SwitchToGrab()
   {
      rightHandWithRay.SetActive(false);
      rightHandWithDirect.SetActive(true);

   }
   
}
