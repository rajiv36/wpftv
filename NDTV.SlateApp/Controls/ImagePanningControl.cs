using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace NDTV.SlateApp.Controls
{
   /// <summary>
   /// Control for panning.
   /// </summary>
    public class ImagePanningControl : Selector
    {
        private ContentPresenter current;
        private TranslateTransform currentTransform;
        public static readonly DependencyProperty FlickToleranceProperty = DependencyProperty.Register("FlickTolerance", typeof(double), typeof(ImagePanningControl), new PropertyMetadata(0.25, new PropertyChangedCallback(ImagePanningControl.flickToleranceChanged), new CoerceValueCallback(ImagePanningControl.coerceFlickTolerance)));
        private bool isDragging;
        public static readonly DependencyProperty LoopContentsProperty = DependencyProperty.Register("LoopContents", typeof(bool), typeof(ImagePanningControl), new PropertyMetadata(false));
        private ContentPresenter next;
        public static readonly DependencyProperty NextItemProperty = DependencyProperty.Register("NextItem", typeof(object), typeof(ImagePanningControl), new PropertyMetadata(null));
        private TranslateTransform nextTransform;
        private ContentPresenter previous;
        public static readonly DependencyProperty PreviousItemProperty = DependencyProperty.Register("PreviousItem", typeof(object), typeof(ImagePanningControl), new PropertyMetadata(null));
        private TranslateTransform previousTransform;
        public static readonly DependencyProperty ScrollDirectionProperty = DependencyProperty.Register("ScrollDirection", typeof(Orientation), typeof(ImagePanningControl), new PropertyMetadata(Orientation.Horizontal));
        public static readonly DependencyProperty SliderValueProperty = DependencyProperty.Register("SliderValue", typeof(double), typeof(ImagePanningControl), new PropertyMetadata(0.0, new PropertyChangedCallback(ImagePanningControl.sliderValueChanged)));
        private Point touchDown;

        /// <summary>
        /// Constructor.
        /// </summary>
        static ImagePanningControl()
        {
            Selector.SelectedItemProperty.OverrideMetadata(typeof(ImagePanningControl), new FrameworkPropertyMetadata(new PropertyChangedCallback(ImagePanningControl.selectedItemChanged)));
            FrameworkElementFactory factory = new FrameworkElementFactory(typeof(Grid));
            factory.SetValue(UIElement.ClipToBoundsProperty, true);
            FrameworkElementFactory child = new FrameworkElementFactory(typeof(ContentPresenter));
            Binding binding = new Binding
            {
                RelativeSource = RelativeSource.TemplatedParent,
                Path = new PropertyPath(PreviousItemProperty)
            };
            child.SetBinding(ContentPresenter.ContentProperty, binding);
            Binding binding2 = new Binding
            {
                RelativeSource = RelativeSource.TemplatedParent,
                Path = new PropertyPath(ItemsControl.ItemTemplateProperty)
            };
            child.SetBinding(ContentPresenter.ContentTemplateProperty, binding2);
            child.Name = "Previous";
            factory.AppendChild(child);
            FrameworkElementFactory factory3 = new FrameworkElementFactory(typeof(ContentPresenter));
            Binding binding3 = new Binding
            {
                RelativeSource = RelativeSource.TemplatedParent,
                Path = new PropertyPath(Selector.SelectedItemProperty)
            };
            factory3.SetBinding(ContentPresenter.ContentProperty, binding3);
            Binding binding4 = new Binding
            {
                RelativeSource = RelativeSource.TemplatedParent,
                Path = new PropertyPath(ItemsControl.ItemTemplateProperty)
            };
            factory3.SetBinding(ContentPresenter.ContentTemplateProperty, binding4);
            factory3.Name = "Current";
            factory.AppendChild(factory3);
            FrameworkElementFactory factory4 = new FrameworkElementFactory(typeof(ContentPresenter));
            Binding binding5 = new Binding
            {
                RelativeSource = RelativeSource.TemplatedParent,
                Path = new PropertyPath(NextItemProperty)
            };
            factory4.SetBinding(ContentPresenter.ContentProperty, binding5);
            Binding binding6 = new Binding
            {
                RelativeSource = RelativeSource.TemplatedParent,
                Path = new PropertyPath(ItemsControl.ItemTemplateProperty)
            };
            factory4.SetBinding(ContentPresenter.ContentTemplateProperty, binding6);
            factory4.Name = "Next";
            factory.AppendChild(factory4);
            ControlTemplate template = new ControlTemplate(typeof(ImagePanningControl))
            {
                VisualTree = factory
            };
            Style defaultValue = new Style(typeof(ImagePanningControl));
            Setter item = new Setter(Control.TemplateProperty, template);
            defaultValue.Setters.Add(item);
            defaultValue.Seal();
            FrameworkElement.StyleProperty.OverrideMetadata(typeof(ImagePanningControl), new FrameworkPropertyMetadata(defaultValue));
        }

        /// <summary>
        /// calling the base constructor.
        /// </summary>
        public ImagePanningControl()
        {
            base.SelectedIndex = 0;
        }

        private void AnimateSliderValueTo(double target)
        {
            DoubleAnimation animation = new DoubleAnimation(target, new Duration(TimeSpan.FromSeconds(0.25)))
            {
                FillBehavior = FillBehavior.Stop
            };
            animation.Completed += delegate(object o, EventArgs e)
            {
                this.SliderValue = 0.0;
            };
            base.BeginAnimation(SliderValueProperty, animation);
        }

        private static object coerceFlickTolerance(DependencyObject sender, object value)
        {
            double num = (double)value;
            return Math.Max(Math.Min(1.0, num), 0.0);
        }

        private static void flickToleranceChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
        }

        /// <summary>
        /// On applying the Template..
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.previousTransform = new TranslateTransform();
            this.previous = (ContentPresenter)base.Template.FindName("Previous", this);
            if (this.previous != null)
            {
                this.previous.Opacity = 0.0;
                this.previous.RenderTransform = this.previousTransform;
            }
            this.currentTransform = new TranslateTransform();
            this.current = (ContentPresenter)base.Template.FindName("Current", this);
            if (this.current != null)
            {
                this.current.RenderTransform = this.currentTransform;
            }
            this.nextTransform = new TranslateTransform();
            this.next = (ContentPresenter)base.Template.FindName("Next", this);
            if (this.next != null)
            {
                this.next.Opacity = 0.0;
                this.next.RenderTransform = this.nextTransform;
            }
        }

        private void OnGestureDown(Point point)
        {
            this.touchDown = point;
            this.isDragging = true;
        }

        private void OnGestureMove(Point point)
        {
            if (this.isDragging)
            {
                Vector vector = (Vector)(point - this.touchDown);
                if (this.ScrollDirection == Orientation.Horizontal)
                {
                    this.SliderValue = vector.X / this.current.ActualWidth;
                }
                else
                {
                    this.SliderValue = vector.Y / this.current.ActualHeight;
                }
                if (Math.Abs(this.SliderValue) >= this.FlickTolerance)
                {
                    this.isDragging = false;
                    int selectedIndex = base.SelectedIndex;
                    if (selectedIndex != -1)
                    {
                        if (this.SliderValue > 0.0)
                        {
                            if (LoopContents == false && selectedIndex > 0)
                            {
                                selectedIndex--;
                                this.SliderValue--;
                            }
                        }
                        else
                        {
                            if (LoopContents == false && (selectedIndex != totalItemCount - 1))
                            {
                                selectedIndex++;
                                this.SliderValue++;
                            }
                        }
                        selectedIndex += base.Items.Count;
                        selectedIndex = selectedIndex % base.Items.Count;
                        base.SelectedIndex = selectedIndex;
                    }
                    this.AnimateSliderValueTo(0.0);
                }
            }
        }

        private void OnGestureUp()
        {
            if (this.isDragging)
            {
                this.AnimateSliderValueTo(0.0);
            }
            this.isDragging = false;
        }

        protected override void OnLostMouseCapture(MouseEventArgs e)
        {
            this.OnGestureUp();
            base.OnLostMouseCapture(e);
        }

        protected override void OnLostTouchCapture(TouchEventArgs e)
        {
            this.OnGestureUp();
            base.OnLostTouchCapture(e);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.CaptureMouse();
            this.OnGestureDown(e.GetPosition(this));
            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            this.OnGestureMove(e.GetPosition(this));
            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            this.OnGestureUp();
            base.ReleaseMouseCapture();
            base.OnMouseUp(e);
        }

        protected override void OnTouchDown(TouchEventArgs e)
        {
            base.CaptureTouch(e.TouchDevice);
            this.OnGestureDown(e.GetTouchPoint(this).Position);
            base.OnTouchDown(e);
        }

        protected override void OnTouchMove(TouchEventArgs e)
        {
            this.OnGestureMove(e.GetTouchPoint(this).Position);
            base.OnTouchMove(e);
        }

        protected override void OnTouchUp(TouchEventArgs e)
        {
            this.OnGestureUp();
            base.ReleaseTouchCapture(e.TouchDevice);
            base.OnTouchUp(e);
        }
        private static int totalItemCount;
        private static void selectedItemChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ImagePanningControl items = (ImagePanningControl)sender;
            int selectedIndex = items.SelectedIndex;

            totalItemCount = items.Items.Count;

            if ((selectedIndex == -1) || (items.Items.Count == 0))
            {
                items.PreviousItem = null;
                items.NextItem = null;
            }
            else
            {
                if (selectedIndex == 0)
                {
                    if (items.LoopContents)
                    {
                        items.PreviousItem = items.Items[items.Items.Count - 1];
                    }
                    else
                    {
                        items.PreviousItem = null;
                    }
                }
                else
                {
                    items.PreviousItem = items.Items[selectedIndex - 1];
                }
                if (selectedIndex == (items.Items.Count - 1))
                {
                    if (items.LoopContents)
                    {
                        items.NextItem = items.Items[0];
                    }
                    else
                    {
                        items.NextItem = null;
                    }
                }
                else
                {
                    items.NextItem = items.Items[selectedIndex + 1];
                }
            }
        }

        private static void sliderValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ImagePanningControl items = (ImagePanningControl)sender;
            if (((items.previous != null) && (items.current != null)) && (items.next != null))
            {
                items.previous.Opacity = 1.0;
                items.next.Opacity = 1.0;
                if (items.ScrollDirection == Orientation.Horizontal)
                {
                    items.previousTransform.X = items.current.ActualWidth * (items.SliderValue - 1.0);
                    items.currentTransform.X = items.current.ActualWidth * items.SliderValue;
                    items.nextTransform.X = items.current.ActualWidth * (items.SliderValue + 1.0);
                    items.previousTransform.Y = 0.0;
                    items.currentTransform.Y = 0.0;
                    items.nextTransform.Y = 0.0;
                }
                else
                {
                    items.previousTransform.X = 0.0;
                    items.currentTransform.X = 0.0;
                    items.nextTransform.X = 0.0;
                    items.previousTransform.Y = items.current.ActualHeight * (items.SliderValue - 1.0);
                    items.currentTransform.Y = items.current.ActualHeight * items.SliderValue;
                    items.nextTransform.Y = items.current.ActualHeight * (items.SliderValue + 1.0);
                }
            }
        }

        /// <summary>
        /// Flick Tolerance.
        /// </summary>
        public double FlickTolerance
        {
            get
            {
                return (double)base.GetValue(FlickToleranceProperty);
            }
            set
            {
                base.SetValue(FlickToleranceProperty, value);
            }
        }

        /// <summary>
        /// Loop the List. Get the first item after last and Vice Versa.
        /// </summary>
        public bool LoopContents
        {
            get
            {
                return (bool)base.GetValue(LoopContentsProperty);
            }
            set
            {
                base.SetValue(LoopContentsProperty, value);
            }
        }

        /// <summary>
        /// Next Item.
        /// </summary>
        public object NextItem
        {
            get
            {
                return base.GetValue(NextItemProperty);
            }
            set
            {
                base.SetValue(NextItemProperty, value);
            }
        }

        /// <summary>
        /// Previous item.
        /// </summary>
        public object PreviousItem
        {
            get
            {
                return base.GetValue(PreviousItemProperty);
            }
            set
            {
                base.SetValue(PreviousItemProperty, value);
            }
        }

        /// <summary>
        /// Scroll direction.. Vertical or horizontal.
        /// </summary>
        public Orientation ScrollDirection
        {
            get
            {
                return (Orientation)base.GetValue(ScrollDirectionProperty);
            }
            set
            {
                base.SetValue(ScrollDirectionProperty, value);
            }
        }

        /// <summary>
        /// Slider value.
        /// </summary>
        public double SliderValue
        {
            get
            {
                return (double)base.GetValue(SliderValueProperty);
            }
            set
            {
                base.SetValue(SliderValueProperty, value);
            }
        }
    }
}


