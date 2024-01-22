//Copyright 2022, Infima Games. All Rights Reserved.

namespace InfimaGames.Animated.ModernGuns
{
    /// <summary>
    /// Game Mode Service.
    /// </summary>
    public class GameModeService : IGameModeService
    {
        #region FIELDS
        
        /// <summary>
        /// The Player Character.
        /// </summary>
        private WeaponAnimatorBehaviour playerCharacter;
        
        #endregion
        
        #region FUNCTIONS
        
        public WeaponAnimatorBehaviour GetPlayerCharacter()
        {
            //Make sure we have a player character that is good to go!
            if (playerCharacter == null)
            {
				WeaponAnimatorBehaviour[] characters  = UnityEngine.Object.FindObjectsOfType<WeaponAnimatorBehaviour>();
                foreach (var character in characters)
                {
					if (character.isWeaponAnimator)

					{
						playerCharacter = character;

					}
                }
			}
            //Return.
            return playerCharacter;
        }

        /// <summary>
        /// GetEquippedWeapon.
        /// </summary>
        public WeaponBehaviour GetEquippedWeapon()
        {
            //Get Character.
            WeaponAnimatorBehaviour characterBehaviour = GetPlayerCharacter();
            //Check Reference.
            if (characterBehaviour == null)
                return null;
            
            //Get Inventory.
            InventoryBehaviour inventoryBehaviour = characterBehaviour.GetInventory();
            //Check Reference.
            if (inventoryBehaviour == null)
                return null;
            
            //Return.
            return inventoryBehaviour.GetEquipped();
        }

        #endregion
    }
}