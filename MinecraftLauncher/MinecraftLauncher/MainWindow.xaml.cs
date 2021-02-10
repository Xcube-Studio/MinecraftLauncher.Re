using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace MinecraftLauncher
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AcrylicWindow
    {
       
        private void Mini(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized; 
            GC.Collect();
        }

        private void Close(object sender, RoutedEventArgs e)
        {Process.GetCurrentProcess().Kill();}


    }
}
