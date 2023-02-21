using UnityEngine;

namespace Infrastructure.Services.Input
{
    public class InputService : IInputService
    {
        private Vector2 _mousePosition1;
        private Vector2 _mousePosition2;
        private float _magnitudeBetweenPositions;
        private float _lastDisplacement;


        public float GetMovement()
        {
            if (UnityEngine.Input.GetMouseButtonDown(0))
            {
                _mousePosition1 = UnityEngine.Input.mousePosition;
            }

            if (UnityEngine.Input.GetMouseButton(0))
            {
                _mousePosition2 = UnityEngine.Input.mousePosition;
                float currentDisplacement = _mousePosition2.x - _mousePosition1.x;

                if (currentDisplacement != 0)
                {
                    float result = currentDisplacement - _lastDisplacement;
                    _lastDisplacement = currentDisplacement;

                    return result;
                }

                _lastDisplacement = 0;
            }

            return 0;
        }
    }
}