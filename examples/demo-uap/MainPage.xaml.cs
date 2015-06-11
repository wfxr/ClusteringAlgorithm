using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace ClusteringDemo {
    public sealed partial class MainPage : Page {
        public static MainPage Current;

        public MainPage() {
            this.InitializeComponent();

            Current = this;
            SampleTitle.Text = AppTitle;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            ScenarioControl.ItemsSource = Scenarios;
            ScenarioControl.SelectedIndex = Window.Current.Bounds.Width > 640 ? 0 : -1;
        }

        private void ScenarioControl_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            // 重置状态栏
            ResetStatus();  
            // 导航到新的新的Scenario
            var newScenario = ((sender as ListBox)?.SelectedItem as Scenario)?.ClassType;
            ScenarioFrame.Navigate(newScenario);
            // 根据窗口大小选择展开/收起面板
            Splitter.IsPaneOpen = Window.Current.Bounds.Width > 640;
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
            => Splitter.IsPaneOpen = !Splitter.IsPaneOpen;

        private void UpdateStatus(string message, Color backgroundColor) {
            StatusBlock.Text = message;
            StatusBorder.Background = new SolidColorBrush(backgroundColor);
            StatusBorder.Visibility = Visibility.Visible;
        }
        public void ResetStatus() => StatusBorder.Visibility = Visibility.Collapsed;
        public void NotifyResult(string resultMessage) => UpdateStatus(resultMessage, Colors.Green);
        public void NotifyError(string errorMessage) => UpdateStatus(errorMessage, Colors.Red);
    }

    // 转换器：将列表中的Scenario转换成可显示的字符串
    public class ScenarioBindingConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            var s = value as Scenario;
            return $"{MainPage.Current.Scenarios.IndexOf(s) + 1}) {s?.Title}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) => true;
    }
}