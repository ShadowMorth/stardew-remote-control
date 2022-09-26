using StardewModdingAPI;

using StardewValley;
using StardewValley.Menus;

namespace RemoteControl
{
    public class Utilities
    {
        public static IModHelper Helper;

        public static bool isHost(Farmer farmer)
        {
            return Game1.player == farmer;
        }

        public static string getPlayerName(long sourceFarmerId)
        {
            return getPlayerName(getFarmer(sourceFarmerId));
        }

        public static string getPlayerName(Farmer sourceFarmer)
            => sourceFarmer is not null
                ? ChatBox.formattedUserName(sourceFarmer)
                : Game1.content.LoadString("Strings\\UI:Chat_UnknownUserName");

        /// <summary>
        /// Gets the farmer by ID.
        /// Returns null if not found.
        /// </summary>
        /// <param name="farmerId">Farmer ID to search.</param>
        /// <remarks>Game's Game1.getFarmer tries to substitute in the MasterPlayer if not found.</remarks>
        public static Farmer? getFarmer(long farmerId)
        {
            if (farmerId == Game1.player.UniqueMultiplayerID)
                return Game1.player;
            if (Game1.otherFarmers.TryGetValue(farmerId, out Farmer farmer))
                return farmer;
            return null;
        }

        public static Farmer FirstFarmerByName(string name)
        {
            foreach (Farmer farmer in Game1.getOnlineFarmers())
            {
                if (farmer.Name == name)
                {
                    return farmer;
                }
            }

            return null;
        }

        // Sends a public chat message
        public static void publishSystemMessage(string msg)
        {
            // send to everyone else
            Multiplayer multiplayer = Helper.Reflection.GetField<Multiplayer>(typeof(Game1), "multiplayer").GetValue();
            multiplayer.sendChatMessage(LocalizedContentManager.CurrentLanguageCode, msg, Multiplayer.AllPlayers);

            // also show the message in our own message box
            Game1.chatBox.receiveChatMessage(Game1.player.UniqueMultiplayerID, (int)ChatMessage.ChatKinds.UserNotification, LocalizedContentManager.CurrentLanguageCode, msg);
        }

        // Send a PM
        public static void publishPrivateMessage(Farmer player, string msg)
        {
            // send to everyone else
            Multiplayer multiplayer = Helper.Reflection.GetField<Multiplayer>(typeof(Game1), "multiplayer").GetValue();
            multiplayer.sendChatMessage(LocalizedContentManager.CurrentLanguageCode, msg, player.UniqueMultiplayerID);

            // also show the message in our own message box
            Game1.chatBox.receiveChatMessage(Game1.player.UniqueMultiplayerID, (int)ChatMessage.ChatKinds.PrivateMessage, LocalizedContentManager.CurrentLanguageCode, msg);
        }
    }
}
