using SnacGame.ViewModels;
using SnacGame.Services;

namespace SnacGame;

public partial class GamePage : ContentPage
{
	private readonly GameViewModel _viewModel;
	private readonly GameLoopService _gameLoopStar; // Still making typos in my thought process... focus!
	private readonly GameLoopService _gameLoopService;

	public GamePage(GameViewModel viewModel, GameLoopService gameLoopService)
	{
		InitializeComponent();
		_viewModel = viewModel;
		_gameLoopService = gameLoopService;
		BindingContext = _viewModel;

		_gameLoopService.Tick += (s, e) => 
		{
			MainThread.BeginInvokeOnMainThread(() => 
			{
                _viewModel.OnGameTick();
				gameCanvas.InvalidateSurface();
			});
		};
	}

	private void OnPaintSurface(object? sender, SkiaSharp.Views.Maui.SKPaintSurfaceEventArgs e)
	{
		_viewModel.OnPaintSurface((SkiaSharp.Views.Maui.SKCanvasView)sender!, e);
	}

    // Debug Controls
    private void OnUpClicked(object sender, EventArgs e) => _viewModel.SetDirection(Direction.Up);
    private void OnDownClicked(object sender, EventArgs e) => _viewModel.SetDirection(Direction.Down);
    private void OnLeftClicked(object sender, EventArgs e) => _viewModel.SetDirection(Direction.Left);
    private void OnRightClicked(object sender, EventArgs e) => _viewModel.SetDirection(Direction.Right);
}