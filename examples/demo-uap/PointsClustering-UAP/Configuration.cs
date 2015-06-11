using System;
using System.Collections.Generic;
using System.IO;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace PointsClustering_UAP
{
    public partial class MainPage : Page
    {
        public const string AppTitle = "App Title";

        // 此处添加Scenario以在左侧列表显示
        public List<Scenario> Scenarios { get; } = new List<Scenario>
        {
            new Scenario {Title = "Scenario1 Title", ClassType = typeof (Scenario1)},
        };
    }
    
    public class Scenario
    {
        public string Title { get; set; }
        public Type ClassType { get; set; }
    }
}