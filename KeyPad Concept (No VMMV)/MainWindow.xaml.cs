using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KeyPad_Concept
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isDragging;
        private Point startPoint;
        private readonly List<Button> keypadButtonCollection = new List<Button>();
  
        public MainWindow()
        {
            InitializeComponent();
            InitializeButtonHoverEffect();
        }

        private void InitializeButtonHoverEffect() 
        {
            keypadButtonCollection.Add(BtnNumeric_0);
            keypadButtonCollection.Add(BtnNumeric_1);
            keypadButtonCollection.Add(BtnNumeric_2);
            keypadButtonCollection.Add(BtnNumeric_3);
            keypadButtonCollection.Add(BtnNumeric_4);
            keypadButtonCollection.Add(BtnNumeric_5);
            keypadButtonCollection.Add(BtnNumeric_6);
            keypadButtonCollection.Add(BtnNumeric_7);
            keypadButtonCollection.Add(BtnNumeric_8);
            keypadButtonCollection.Add(BtnNumeric_9);
            keypadButtonCollection.Add(BtnClear);
            keypadButtonCollection.Add(BtnDelete);
            keypadButtonCollection.Add(BtnEnter);
            keypadButtonCollection.Add(BtnDecimal);

            //apply ScaleButtonOnHover() to each button in the list
            foreach (Button btn in keypadButtonCollection)
            {
                ScaleButtonOnHover(btn);
            }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            isDragging = true;
            startPoint = e.GetPosition(this);
        }

        private void Window_MouseMove(object sender,MouseEventArgs e) 
        { 
            //while boolean value primarily indicates the start and end of the dragging operation, additional check needed 
            //to prevent unintentional dragging that might occur if the mouse moves after releasing the left button but
            //before the 'MouseLeftButtonUp' event is registered.
            if(isDragging && e.LeftButton == MouseButtonState.Pressed)
            {
                Point newPoint = e.GetPosition(this);

                //the term 'delta' is commonly used to represent change or difference between two values
                double deltaX = newPoint.X - startPoint.X;
                double deltaY = newPoint.Y - startPoint.Y;

                //adjusting the window's top-left corner
                Left += deltaX;
                Top += deltaY;
            }
        }

        private void Window_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) 
        {
            isDragging = false;
        }

        //additional hover animation function for the button, letting the button shrink immediately after growing
        private void ScaleButtonOnHover(Button button)
        {
            ScaleTransform scaleTransform = new ScaleTransform(1.0, 1.0);
            //animation setup: scales the button from 1.0 to 1.05, creating a slight enlargement effect
            DoubleAnimation growAnimation = new DoubleAnimation(1.0, 1.05, TimeSpan.FromSeconds(0.1));
            //animation setup: scales the button back from 1.05 to 1.0, returning to its original size
            DoubleAnimation shrinkAnimation = new DoubleAnimation(1.05, 1.0, TimeSpan.FromSeconds(0.1));
            
            button.RenderTransformOrigin = new Point(0.5, 0.5);
            button.RenderTransform = scaleTransform;

            growAnimation.Completed += (sender, e) =>
            {
                scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, shrinkAnimation);
                scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, shrinkAnimation);
            };

            button.MouseEnter += (sender, e) =>
            {
                scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, growAnimation);
                scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, growAnimation);
            };
        }

        private void CloseApp_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); 
        }

        private void MinimizeApp_Click(object sender, RoutedEventArgs e) 
        {
            this.WindowState = WindowState.Minimized;
        }

        private void ToggleNumericButtonContent_Click(object sender, RoutedEventArgs e)
        { 
             SwapButtonContent(BtnNumeric_0, BtnNumeric_6);
             SwapButtonContent(BtnNumeric_1, BtnNumeric_7);
             SwapButtonContent(BtnNumeric_2, BtnNumeric_8);   
        }

        private void SwapButtonContent(Button btn1, Button btn2) 
        {
            string tempContent = btn1.Content.ToString();
            btn1.Content = btn2.Content;
            btn2.Content = tempContent;
        }

        private void KeypadButton_Click(object sender, RoutedEventArgs e) 
        { 
            if(sender is Button button) 
            {
                string buttonText = button.Content.ToString();

                //delete button not included since its content is an image
                switch(buttonText) 
                {
                    case "0":
                    case "1":
                    case "2":
                    case "3":
                    case "4":
                    case "5":
                    case "6":
                    case "7":
                    case "8":
                    case "9":
                    case "." when !Display.Text.Contains("."):
                        Display.Text += buttonText;
                        break;

                    //clear button
                    case "Esc":
                        Display.Text = "";
                        break;
                  
                    default:
                        break;
                }
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (Display.Text.Length > 0)
            {
                Display.Text = Display.Text.Substring(0, Display.Text.Length - 1);
            }
        }
    }
}
