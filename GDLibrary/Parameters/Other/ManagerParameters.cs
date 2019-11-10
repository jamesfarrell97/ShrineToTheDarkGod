﻿/*
Function: 		Encapsulates manager parameters for those classes (e.g. MouseManager) that need access to a large number of managers.
Author: 		NMCG
Version:		1.0
Bugs:			None
Fixes:			None
*/

namespace GDLibrary
{
    public class ManagerParameters
    {
        #region Fields
        private ObjectManager objectManager;
        private CameraManager cameraManager;
        private MouseManager mouseManager;
        private KeyboardManager keyboardManager;
        private GamePadManager gamePadManager;
        private SoundManager soundManager;
        #endregion

        #region Properties
        public ObjectManager ObjectManager
        {
            get
            {
                return this.objectManager;
            }
        }

        public CameraManager CameraManager
        {
            get
            {
                return this.cameraManager;
            }
        }

        public MouseManager MouseManager
        {
            get
            {
                return this.mouseManager;
            }
        }

        public KeyboardManager KeyboardManager
        {
            get
            {
                return this.keyboardManager;
            }
        }

        public GamePadManager GamePadManager
        {
            get
            {
                return this.gamePadManager;
            }
        }

        public SoundManager SoundManager
        {
            get
            {
                return this.soundManager;
            }
        }
        #endregion

        #region Constructors
        public ManagerParameters(
            ObjectManager objectManager, 
            CameraManager cameraManager,
            MouseManager mouseManager, 
            KeyboardManager keyboardManager, 
            GamePadManager gamePadManager,
            SoundManager soundManager
        ) {
            this.objectManager = objectManager;
            this.cameraManager = cameraManager;
            this.mouseManager = mouseManager;
            this.keyboardManager = keyboardManager;
            this.gamePadManager = gamePadManager;
            this.soundManager = soundManager;
        }
        #endregion
    }
}