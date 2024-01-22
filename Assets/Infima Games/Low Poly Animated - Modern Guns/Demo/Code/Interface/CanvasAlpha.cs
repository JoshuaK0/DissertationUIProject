//Copyright 2022, Infima Games. All Rights Reserved.

using UnityEngine;

namespace InfimaGames.Animated.ModernGuns.Interface
{
    /// <summary>
    /// CanvasAlpha. Changes the canvas alpha based on certain things that can happen in-game.
    /// </summary>
    public class CanvasAlpha : Element
    {
        #region FIELDS SERIALIZED
        
        [Title(label: "References")]

        [Tooltip("Canvas group to update the alpha for.")]
        [SerializeField, NotNull]
        private CanvasGroup canvasGroup;
        
        #endregion
        
        #region METHODS
        
        /// <summary>
        /// Tick.
        /// </summary>
        protected override void Tick()
        {
            //Base.
            base.Tick();

            //Check References.
            if (canvasGroup == null)
            {
                //ReferenceError.
                Log.ReferenceError(this, gameObject);
                
                //Return.
                return;
            }

            //Update Alpha.
            //canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, characterBehaviour.IsCursorLocked() ? 1.0f : cursorUnlockedAlpha, Time.deltaTime * interpolationSpeed);
        }
        
        #endregion
    }
}