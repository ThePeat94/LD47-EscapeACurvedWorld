using UnityEngine;

namespace Player
{
    public class InputProcessor : MonoBehaviour
    {
        public float MovementX => this.m_movementInput.x;
        public float MovementZ => this.m_movementInput.y;
        public float MoveAmount => Mathf.Abs(this.MovementX) + Mathf.Abs(this.MovementZ);
        public bool IsRunTriggered => this.m_playerInput.Keyboard.Sprint.triggered;

        private PlayerInput m_playerInput;
        private Vector2 m_movementInput;

        private void Awake()
        {
            this.m_playerInput = new PlayerInput();
        }

        private void Update()
        {
            this.m_movementInput = this.m_playerInput.Keyboard.Movement.ReadValue<Vector2>();
        }

        private void OnEnable()
        {
            this.m_playerInput.Enable();
            this.m_playerInput.Keyboard.Sprint.started += x => Debug.Log("Sprint started");
            this.m_playerInput.Keyboard.Sprint.canceled += x => Debug.Log("Sprint ended");
        }

        private void OnDisable()
        {
            this.m_playerInput.Disable();
        }
    }
}
