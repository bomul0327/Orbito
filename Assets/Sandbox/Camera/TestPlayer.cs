using UnityEngine;

namespace Experimental
{
    /// <summary>
    /// Test class for testing camera controller features.
    /// </summary>
    public class TestPlayer : MonoBehaviour
    {
        public Camera playerCamera;

        [SerializeField] CameraController cameraController;

        [Header("Intitial camera settings")]
        [SerializeField] CameraControlSettings controlSettings;

        [SerializeField] CameraFollowSettings followSettings;

        [SerializeField] CameraZoomSettings zoomSettings;


        // Start is called before the first frame update
        void Start()
        {
            cameraController = new CameraController(playerCamera, playerCamera.transform.parent)
            {
                ZoomSettings = zoomSettings,
                ControlSettings = controlSettings,
                FollowSettings = followSettings
            };
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetButton("Fire1"))
            {
               transform.position = CameraUtils.MouseToWorldPos(playerCamera, Input.mousePosition);
            }
        }
    }
}