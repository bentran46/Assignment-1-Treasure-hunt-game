using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TreasureHuntGame
{
    public partial class MainPage : ContentPage
    {
        private List<string> _tileImageData;
        private List<ImageButton> _gameTiles;
        private ImageButton _firstFlippedTile;
        private ImageButton _secondFlippedTile;
        private int _matchCount;

        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            SetupNewGameSession();
        }

        /// <summary>
        /// Shuffle images and reset the board for a new game.
        /// </summary>
        private void SetupNewGameSession()
        {
            var faces = new List<string>
            {
                "tile1.jpg", "tile1.jpg",
                "tile2.jpg", "tile2.jpg",
                "tile3.jpg", "tile3.jpg",
                "tile4.gif", "tile4.gif"
            };

            Random r = new Random();
            _tileImageData = faces.OrderBy(x => r.Next()).ToList();

            _gameTiles = new List<ImageButton>
            {
                _imgTile1, _imgTile2, _imgTile3, _imgTile4,
                _imgTile5, _imgTile6, _imgTile7, _imgTile8
            };

            foreach (var tile in _gameTiles)
            {
                tile.Source = "question_mark.jpg";
                tile.IsEnabled = true;
                tile.IsVisible = true;
            }

            _imgMatch1.IsVisible = false;
            _imgMatch2.IsVisible = false;
            _imgMatch3.IsVisible = false;
            _imgMatch4.IsVisible = false;

            _firstFlippedTile = null;
            _secondFlippedTile = null;
            _matchCount = 0;

            _btnPlayAgain.IsVisible = false;
            _imgTreasure.IsVisible = false;
            _lblTitle.IsVisible = true;
        }

        /// <summary>
        /// Flip tiles and check for a match when the user taps an image button.
        /// </summary>
        private async void OnTileClicked(object sender, EventArgs e)
        {
            var clicked = sender as ImageButton;
            if (clicked == null || clicked == _firstFlippedTile || !clicked.IsEnabled)
                return;

            int index = _gameTiles.IndexOf(clicked);
            clicked.Source = _tileImageData[index];

            if (_firstFlippedTile == null)
            {
                _firstFlippedTile = clicked;
                return;
            }

            _secondFlippedTile = clicked;

            int index1 = _gameTiles.IndexOf(_firstFlippedTile);
            int index2 = _gameTiles.IndexOf(_secondFlippedTile);

            if (_tileImageData[index1] == _tileImageData[index2])
            {
                await DisplayAlert("Match Found!", "You have a match!", "OK");

                ShowMatchInTreasureRow(_tileImageData[index1]);

                _firstFlippedTile.Source = "smiley.jpg";
                _secondFlippedTile.Source = "smiley.jpg";
                _firstFlippedTile.IsEnabled = false;
                _secondFlippedTile.IsEnabled = false;

                _matchCount++;

                if (_matchCount == 4)
                {
                    await DisplayAlert("Victory!", "You have completed the game. Congratulations!", "OK");
                    ShowTreasureScreen();
                }
            }
            else
            {
                await Task.Delay(2000);
                await DisplayAlert("No Match", "Sorry, the tiles do not match. Please try again.", "OK");

                _firstFlippedTile.Source = "question_mark.jpg";
                _secondFlippedTile.Source = "question_mark.jpg";
            }

            _firstFlippedTile = null;
            _secondFlippedTile = null;
        }

        private void ShowMatchInTreasureRow(string imageName)
        {
            if (!_imgMatch1.IsVisible)
            {
                _imgMatch1.Source = imageName;
                _imgMatch1.IsVisible = true;
            }
            else if (!_imgMatch2.IsVisible)
            {
                _imgMatch2.Source = imageName;
                _imgMatch2.IsVisible = true;
            }
            else if (!_imgMatch3.IsVisible)
            {
                _imgMatch3.Source = imageName;
                _imgMatch3.IsVisible = true;
            }
            else if (!_imgMatch4.IsVisible)
            {
                _imgMatch4.Source = imageName;
                _imgMatch4.IsVisible = true;
            }
        }

        private void ShowTreasureScreen()
        {
            foreach (var tile in _gameTiles)
                tile.IsVisible = false;

            _imgMatch1.IsVisible = false;
            _imgMatch2.IsVisible = false;
            _imgMatch3.IsVisible = false;
            _imgMatch4.IsVisible = false;

            _lblTitle.IsVisible = false;
            _imgTreasure.IsVisible = true;
            _btnPlayAgain.IsVisible = true;
        }

        private void OnPlayAgainClicked(object sender, EventArgs e)
        {
            SetupNewGameSession();
        }
    }
}
