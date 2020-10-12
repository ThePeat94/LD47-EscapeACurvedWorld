using System;
using EventArgs;
using UnityEngine;

namespace Player
{
    public class InputProcessor : MonoBehaviour
    {
        private PlayerInput m_playerInput;
        private Vector2 m_movementInput;


        public float MovementX => this.m_movementInput.x;
        public float MovementZ => this.m_movementInput.y;
        public float MoveAmount => Mathf.Abs(this.MovementX) + Mathf.Abs(this.MovementZ);
        public bool JumpTriggered => this.m_playerInput.Keyboard.Jump.triggered;
        public bool UseItemTriggered => this.m_playerInput.Mouse.UseItem.triggered;
        public bool HoldingSprint { get; private set; }
        public bool SkipDanceTriggered => this.m_playerInput.Keyboard.SkipDance.triggered;

        public event EventHandler<SprintChangedEventArgs> RunningStateChanged
        {
            add => this.m_sprintStateChanged += value;
            remove => this.m_sprintStateChanged -= value;
        }
        
        private EventHandler<SprintChangedEventArgs> m_sprintStateChanged;

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
            this.m_playerInput.Keyboard.Sprint.started += x =>
            {
                this.HoldingSprint = true;
                this.m_sprintStateChanged?.Invoke(this, new SprintChangedEventArgs(true));
            };
            this.m_playerInput.Keyboard.Sprint.canceled += x =>
            {
                this.HoldingSprint = false;
                this.m_sprintStateChanged?.Invoke(this, new SprintChangedEventArgs(false));
            };
        }

        private void OnDisable()
        {
            this.m_playerInput.Disable();
        }
    }
}
