using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Project.WPF.Behavior
{
    /// <summary>
    /// テキストボックスに薄く字を入れる、フォーカスがあたると消える処理を行う
    /// </summary>
    public static class PlaceHolderBehavior
    {
        /// <summary>
        /// プレースホルダーとして表示するテキスト
        /// </summary>
        public static readonly DependencyProperty PlaceHolderTextProperty = DependencyProperty.RegisterAttached(
            "PlaceHolderText",
            typeof(String),
            typeof(PlaceHolderBehavior),
            new PropertyMetadata(null, OnPlaceHolderChanged));

        /// <summary>
        /// プレースホルダ変更処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnPlaceHolderChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
			if (sender is not TextBox textBox) {
				return;
			}

			var placeHolder = e.NewValue as String;
            var handler = CreateEventHandler(placeHolder ?? String.Empty);
            if (String.IsNullOrEmpty(placeHolder))
            {
                textBox.TextChanged -= handler;
            }
            else
            {
                textBox.TextChanged += handler;
                if (String.IsNullOrEmpty(textBox.Text))
                {
                    textBox.Background = CreateVisualBrush(placeHolder);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="placeHolder"></param>
        /// <returns></returns>
        private static TextChangedEventHandler CreateEventHandler(String placeHolder)
        {
            // TextChanged イベントをハンドルし、TextBox が未入力のときだけ
            // プレースホルダーを表示するようにする。
            return (sender, e) =>
            {
                // 背景に TextBlock を描画する VisualBrush を使って
                // プレースホルダーを実現
                var textBox = (TextBox)sender;
                if (String.IsNullOrEmpty(textBox.Text))
                {
                    textBox.Background = CreateVisualBrush(placeHolder);
                }
                else
                {
                    textBox.Background = new SolidColorBrush(Colors.Transparent);
                }
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="placeHolder"></param>
        /// <returns></returns>
        private static VisualBrush CreateVisualBrush(String placeHolder)
        {
            var visual = new Label()
            {
                Content = placeHolder,
                Padding = new Thickness(5, 1, 1, 1),
                Foreground = new SolidColorBrush(Colors.LightGray),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
            };
            return new VisualBrush(visual)
            {
                Stretch = Stretch.None,
                TileMode = TileMode.None,
                AlignmentX = AlignmentX.Left,
                AlignmentY = AlignmentY.Center,
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textBox"></param>
        /// <param name="placeHolder"></param>
        public static void SetPlaceHolderText(TextBox textBox, String placeHolder)
            => textBox.SetValue(PlaceHolderTextProperty, placeHolder);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textBox"></param>
        /// <returns></returns>
        public static String? GetPlaceHolderText(TextBox textBox)
            => textBox.GetValue(PlaceHolderTextProperty) as String;
    }
}
