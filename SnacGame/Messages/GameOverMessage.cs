using CommunityToolkit.Mvvm.Messaging.Messages;

namespace SnacGame.Messages;

public class GameOverMessage : ValueChangedMessage<bool>
{
	public GameOverMessage(bool isGameOver) : base(isGameOver)
	{
	}
}
