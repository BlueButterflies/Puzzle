using System;
using System.Collections;
using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace Puzzle
{
    [Activity(Label = "Puzzle", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        #region Variables
        private Button buttonReset;
        private GridLayout gridLayout;

        private int viewGameWidth;
        private int tileWidth;

        ArrayList tileArr;
        ArrayList coordsTile;

        Point emptyTile;
        #endregion

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.content_main);

            //Call Set Game View
            SetGameView();

            // Call Tile Method to stat game
            TilesMethod();

            //Call the Random Method to start game 
            RandomMethod();
        }

        #region Create gerid layout and crete rows and cols
        private void SetGameView()
        {
            buttonReset = FindViewById<Button>(Resource.Id.btnResetId);
            buttonReset.Click += ResetMethod;

            gridLayout = FindViewById<GridLayout>(Resource.Id.gameGridLayoutId);

            viewGameWidth = Resources.DisplayMetrics.WidthPixels;

            gridLayout.ColumnCount = 4;
            gridLayout.RowCount = 4;

            gridLayout.LayoutParameters = new LinearLayout.LayoutParams(viewGameWidth, viewGameWidth);

            gridLayout.SetBackgroundColor(Color.DeepSkyBlue);
        }
        #endregion

        #region Set color text and backgroun color, create tiles and them position, params, width and height
        private void TilesMethod()
        {
            tileWidth = viewGameWidth / 4;

            tileArr = new ArrayList();
            coordsTile = new ArrayList();

            int counter = 1;

            for (int rows = 0; rows < 4; rows++)
            {
                for (int colls = 0; colls < 4; colls++)
                {
                    MyTextView txtTile = new MyTextView(this);
                    txtTile.Text = counter.ToString();
                    txtTile.SetTextColor(Color.White);
                    txtTile.Gravity = GravityFlags.Center;
                    txtTile.TextSize = 30;

                    GridLayout.Spec rowSpec = GridLayout.InvokeSpec(txtTile.xPos);
                    GridLayout.Spec collSpec = GridLayout.InvokeSpec(txtTile.yPos);

                    GridLayout.LayoutParams tileLayoutParams = new GridLayout.LayoutParams(rowSpec, collSpec);

                    tileLayoutParams.Width = tileWidth - 10;
                    tileLayoutParams.Height = tileWidth - 10;
                    tileLayoutParams.SetMargins(6, 6, 6, 6);

                    txtTile.LayoutParameters = tileLayoutParams;

                    txtTile.SetBackgroundColor(Color.Green);

                    Point point = new Point(colls, rows);
                    coordsTile.Add(point);

                    txtTile.xPos = point.X;
                    txtTile.yPos = point.Y;

                    txtTile.Touch += TxtTile_Touch;

                    tileArr.Add(txtTile);

                    gridLayout.AddView(txtTile);

                    counter++;
                }
            }

            gridLayout.RemoveView((MyTextView)tileArr[15]);
            tileArr.RemoveAt(15);
        }
        #endregion

        #region Touch Event
        private void TxtTile_Touch(object sender, View.TouchEventArgs e)
        {
            if (e.Event.Action == MotionEventActions.Up)
            {
                if (tileArr.Contains(sender))
                {
                    MyTextView thisTile = (MyTextView)sender;

                    float xDifference = (float)Math.Pow(thisTile.xPos - emptyTile.X, 2);
                    float yDifference = (float)Math.Pow(thisTile.yPos - emptyTile.Y, 2);

                    float distanceXY = (float)Math.Sqrt(xDifference + yDifference);

                    //Tile can move
                    if (distanceXY == 1)
                    {
                        //Memorize where the tile use to be
                        Point currentPoint = new Point(thisTile.xPos, thisTile.yPos);

                        //take the empty tile
                        GridLayout.Spec rowSpec = GridLayout.InvokeSpec(emptyTile.Y);
                        GridLayout.Spec collSpec = GridLayout.InvokeSpec(emptyTile.X);

                        GridLayout.LayoutParams newLayoutParams = new GridLayout.LayoutParams(rowSpec, collSpec);

                        thisTile.xPos = emptyTile.X;
                        thisTile.yPos = emptyTile.Y;

                        newLayoutParams.Width = tileWidth - 10;
                        newLayoutParams.Height = tileWidth - 10;
                        newLayoutParams.SetMargins(6, 6, 6, 6);

                        thisTile.LayoutParameters = newLayoutParams;

                        emptyTile = currentPoint;
                    }

                    //#region Test positions touch tile and empty tile
                    //Console.WriteLine($"tile position\nx: {thisTile.xPos}\ny: {thisTile.yPos} ");
                    //Console.WriteLine($"empty tile position\nx: {emptyTile.X}\ny: {emptyTile.Y} ");
                    //#endregion
                }
            }
        }
        #endregion

        #region Random Positin 
        private void RandomMethod()
        {
            ArrayList tempCoords = new ArrayList(coordsTile);

            Random random = new Random();

            foreach (MyTextView any in tileArr)
            {
                int randomIndex = random.Next(0, tempCoords.Count);

                Point thisRandomLocation = (Point)tempCoords[randomIndex];


                GridLayout.Spec rowSpec = GridLayout.InvokeSpec(thisRandomLocation.Y);
                GridLayout.Spec collSpec = GridLayout.InvokeSpec(thisRandomLocation.X);

                GridLayout.LayoutParams randomLayoutParams = new GridLayout.LayoutParams(rowSpec, collSpec);

                any.xPos = thisRandomLocation.X;
                any.yPos = thisRandomLocation.Y;

                randomLayoutParams.Width = tileWidth - 10;
                randomLayoutParams.Height = tileWidth - 10;
                randomLayoutParams.SetMargins(6, 6, 6, 6);


                any.LayoutParameters = randomLayoutParams;

                tempCoords.RemoveAt(randomIndex);
            }

            emptyTile = (Point)tempCoords[0];
        }
        #endregion

        #region Reset Game
        private void ResetMethod(object sender, EventArgs e)
        {
            RandomMethod();
        }
        #endregion
    }

    #region Create class for touch position and use in class MainActivity
    class MyTextView : TextView
    {
        Activity myContext;

        public MyTextView(Activity context) : base(context)
        {
            myContext = context;
        }

        public int xPos { set; get; }
        public int yPos { set; get; }
    }
    #endregion
}
