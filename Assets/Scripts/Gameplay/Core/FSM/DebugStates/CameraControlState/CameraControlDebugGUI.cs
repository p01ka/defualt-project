using UnityEngine;

namespace IdxZero.Gameplay.States.DebugStates.CameraControl
{
    public class CameraControlDebugGUI : MonoBehaviour
    {
        // private IContainerMover _containerMover;
        // private ICameraMover _cameraMover;

        private int _screenWidth;
        private int _screenHeight;
        private bool _updateLayout;

        private Rect _startPosition;
        private Rect _mainToBake;
        private Rect _bakeToShow;
        private Rect _showToStart;
        private Rect _mainToSlicing;
        private Rect _slicingToMain;
        private Rect _startToClient;
        private Rect _startToMain;


        private void Awake()
        {
            _screenWidth = Screen.width;
            _screenHeight = Screen.height;

            UpdateLayout();
        }

        // public void SetMovers(IContainerMover containerMover,
        //     ICameraMover cameraMover)
        // {
        //     _containerMover = containerMover;
        //     _cameraMover = cameraMover;
        // }

        public void UpdateLayout()
        {
            _updateLayout = true;
        }

        private void UpdateLayoutIfNeed()
        {
            if (!_updateLayout) return;

            _updateLayout = false;
            UpdateLayoutInternal();
        }

        private void Update()
        {
            if (Screen.width != _screenWidth || Screen.height != _screenHeight)
            {
                _screenWidth = Screen.width;
                _screenHeight = Screen.height;

                UpdateLayout();
            }
        }

        private void UpdateLayoutInternal()
        {
            const float buttonWidth = 125;
            const float buttonHeight = 75;
            const float space = 8;

            _startPosition = new Rect(buttonWidth * 0.2f + 2 * (buttonWidth + space),
                _screenHeight - 9 * buttonHeight, buttonWidth * 2f, buttonHeight * 2f);


            _bakeToShow = new Rect(buttonWidth * 0.2f, _screenHeight - 6 * buttonHeight, buttonWidth * 2f, buttonHeight * 2f);
            _showToStart = new Rect(buttonWidth * 0.2f + 2 * (buttonWidth + space), _screenHeight - 6 * buttonHeight, buttonWidth * 2f, buttonHeight * 2f);
            _slicingToMain = new Rect(buttonWidth * 0.2f + 4 * (buttonWidth + space), _screenHeight - 6 * buttonHeight, buttonWidth * 2f, buttonHeight * 2f);
            _startToMain = new Rect(buttonWidth * 0.2f + 6 * (buttonWidth + space), _screenHeight - 6 * buttonHeight, buttonWidth * 2f, buttonHeight * 2f);


            _mainToBake = new Rect(buttonWidth * 0.2f, _screenHeight - 3 * buttonHeight, buttonWidth * 2f,
                buttonHeight * 2f);
            _mainToSlicing = new Rect(buttonWidth * 0.2f + 2 * (buttonWidth + space),
                _screenHeight - 3 * buttonHeight, buttonWidth * 2f, buttonHeight * 2f);

            _startToClient = new Rect(buttonWidth * 0.2f + 4 * (buttonWidth + space),
                _screenHeight - 3 * buttonHeight, buttonWidth * 2f, buttonHeight * 2f);
        }

        private void OnGUI()
        {
            UpdateLayoutIfNeed();
            GUIStyle myButtonStyle = new GUIStyle(GUI.skin.button);
            myButtonStyle.fontSize = 35;

            if (GUI.Button(_startPosition, "START\nPOSITION", myButtonStyle))
            {
                // if (_cameraMover != null)
                //     _cameraMover.SetStartMovePositionAndRotationWithType(PathMoveType.START_CLIENT_POSITION);
            }

            if (GUI.Button(_startToMain, "START\nTO MAIN", myButtonStyle))
            {
                // if (_cameraMover != null)
                //     _cameraMover.MoveCamera(PathMoveType.START_CLIENT_POSITION_TO_MAIN);
            }

            if (GUI.Button(_mainToSlicing, "MAIN\nTO SLICING", myButtonStyle))
            {
                // if (_containerMover != null && _cameraMover != null)
                // {
                //     //   _containerMover.MoveTarget(PathMoveType.MAIN_TO_SLICING);
                //     _cameraMover.MoveCamera(PathMoveType.MAIN_TO_SLICING);
                // }
            }

            if (GUI.Button(_slicingToMain, "SLICING\nTO MAIN", myButtonStyle))
            {
                // if (_containerMover != null && _cameraMover != null)
                // {
                //     //    _containerMover.MoveTarget(PathMoveType.SLICING_TO_MAIN);
                //     _cameraMover.MoveCamera(PathMoveType.SLICING_TO_MAIN);
                // }
            }

            if (GUI.Button(_mainToBake, "MAIN\nTO BAKE", myButtonStyle))
            {
                // if (_containerMover != null && _cameraMover != null)
                // {
                //     _containerMover.MoveTarget(PathMoveType.MAIN_TO_BAKE);
                //     _cameraMover.MoveCamera(PathMoveType.MAIN_TO_BAKE);
                // }
            }

            if (GUI.Button(_bakeToShow, "BAKE\nTO SHOW", myButtonStyle))
            {
                // if (_containerMover != null && _cameraMover != null)
                // {
                //     _containerMover.MoveTarget(PathMoveType.BAKE_TO_SHOW);
                //     _cameraMover.MoveCamera(PathMoveType.BAKE_TO_SHOW);
                // }
            }

            if (GUI.Button(_showToStart, "SHOW\nTO START", myButtonStyle))
            {
                // if (_containerMover != null && _cameraMover != null)
                // {
                //     _cameraMover.MoveCamera(PathMoveType.SHOW_TO_START_TABLE);
                //     _containerMover.MoveTarget(PathMoveType.SHOW_TO_START_TABLE);
                // }
            }

            if (GUI.Button(_startToClient, "START\nTO CLIENT", myButtonStyle))
            {
                // if (_cameraMover != null)
                //     _cameraMover.MoveCamera(PathMoveType.START_TABLE_TO_START_CLIENT_POSITION);
            }
        }
    }
}