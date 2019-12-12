﻿using SkinnedModel;
using JigLibX.Collision;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;

namespace GDLibrary
{
    public class AnimatedEnemyObject : EnemyObject
    {
        #region Variables
        private AnimationStateType animationState;
        private AnimationPlayer animationPlayer;
        private SkinningData skinningData;

        //stores all the data related to a character with multiple individual FBX animation files (e.g. walk.fbx, idle,fbx, run.fbx)
        private Dictionary<AnimationDictionaryKey, Model> modelDictionary;
        private Dictionary<AnimationDictionaryKey, AnimationPlayer> animationPlayerDictionary;
        private Dictionary<AnimationDictionaryKey, SkinningData> skinningDataDictionary;
        private AnimationDictionaryKey oldKey;
        #endregion

        #region Properties
        public AnimationStateType AnimationState
        {
            get
            {
                return this.animationState;
            }
            set
            {
                this.animationState = value;
            }
        }

        public AnimationPlayer AnimationPlayer
        {
            get
            {
                return this.animationPlayer;
            }
        }
        #endregion

        private readonly float moveSpeed;
        private readonly float rotateSpeed;

        public AnimatedEnemyObject(
            string id,
            ActorType actorType,            
            Transform3D transform,
            EffectParameters effectParameters,
            Model model,
            float accelerationRate,
            float decelerationRate,
            Vector3 movementVector,
            Vector3 rotationVector,
            float moveSpeed,
            float rotateSpeed,
            float health,
            float attack,
            float defence,
            ManagerParameters managerParameters
        ) : base (
            id,
            actorType,
            transform,
            effectParameters,
            model,
            accelerationRate,
            decelerationRate,
            movementVector,
            rotationVector,
            moveSpeed,
            rotateSpeed,
            health,
            attack,
            defence,
            managerParameters
        ) {
            //Add extra constructor parameters like health, inventory etc...
            this.moveSpeed = moveSpeed;
            this.rotateSpeed = rotateSpeed;

            //initialize dictionaries
            this.modelDictionary = new Dictionary<AnimationDictionaryKey, Model>();
            this.animationPlayerDictionary = new Dictionary<AnimationDictionaryKey, AnimationPlayer>();
            this.skinningDataDictionary = new Dictionary<AnimationDictionaryKey, SkinningData>();
        }

        //This methods defines how your player interacts with ALL collidable objects in the world - its really the players complete behaviour
        private bool CollisionSkin_callbackFn(CollisionSkin collider, CollisionSkin collidee)
        {
            HandleCollisions(collider.Owner.ExternalData as CollidableObject, collidee.Owner.ExternalData as CollidableObject);
            return true;
        }

        //want do we want to do now that we have collided with an object?
        private void HandleCollisions(CollidableObject collidableObjectCollider, CollidableObject collidableObjectCollidee)
        {
            //did the "as" typecast return a valid object?
            if (collidableObjectCollidee != null)
            {
                if (collidableObjectCollidee.ActorType == ActorType.CollidablePickup)
                {
                    #region demo removed from nialls code
                    //do whatever you want here when you hit a collidable pickup...
                    #endregion
                }
                //add else if statements here for all the responses that you want your player to have
               // else if (collidableObjectCollidee.ActorType == ActorType.CollidableDoor)
               // {
               //
               // }
               // else if (collidableObjectCollidee.ActorType == ActorType.CollidableAmmo)
               // {
               //
               // }
            }
        }

        //Sets the first frame for the take and file (e.g. "Take 001", "dude")
        public void SetAnimation(string takeName, string fileNameNoSuffix)
        {
            AnimationDictionaryKey key = new AnimationDictionaryKey(takeName, fileNameNoSuffix);

            //Have we requested a different animation and is it in the dictionary?
            //First time or different animation request
            if (this.oldKey == null || (!this.oldKey.Equals(key) && this.modelDictionary.ContainsKey(key)))
            {
                //Set the model based on the animation being played
                this.Model = modelDictionary[key];

                //Retrieve the animation player
                animationPlayer = animationPlayerDictionary[key];

                //Retrieve the skinning data
                skinningData = skinningDataDictionary[key];

                //Set the skinning data in the animation player and set the player to start at the first frame for the take
                animationPlayer.StartClip(skinningData.AnimationClips[key.takeName]);
            }

            //Store current key for comparison in next update to prevent re-setting the same animation in successive calls to SetAnimation()
            this.oldKey = key;
        }

        public void AddAnimation(string takeName, string fileNameNoSuffix, Model model)
        {
            AnimationDictionaryKey key = new AnimationDictionaryKey(takeName, fileNameNoSuffix);

            //If not already added
            if (!this.modelDictionary.ContainsKey(key))
            {
                this.modelDictionary.Add(key, model);

                //Read the skinning data (i.e. the set of transforms applied to each model bone for each frame of the animation)
                skinningData = model.Tag as SkinningData;

                if (skinningData == null) throw new InvalidOperationException("The model [" + fileNameNoSuffix + "] does not contain a SkinningData tag.");

                //Make an animation player for the model
                this.animationPlayerDictionary.Add(key, new AnimationPlayer(skinningData));

                //Store the skinning data for the model 
                this.skinningDataDictionary.Add(key, skinningData);
            }
        }

        public override void Update(GameTime gameTime)
        {
            //Update character to return bone transforms for the appropriate frame in the animation
            animationPlayer.Update(gameTime.ElapsedGameTime, true, Matrix.Identity);
            base.Update(gameTime);
        }

        protected virtual void SetAnimationByInput()
        {
            switch (this.AnimationState)
            {
                case AnimationStateType.Attacking:
                    //call SetAnimation() with your Running FBX file name and Take 001
                    SetAnimation("Take 001", "dude");
                    break;

                case AnimationStateType.Defending:
                    SetAnimation("Take 001", "dude");
                    break;

                case AnimationStateType.Idle:
                    //call SetAnimation() with your Jumping FBX file name and Take 001
                    break;
            }
        }
    }
}