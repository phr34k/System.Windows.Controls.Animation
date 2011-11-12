using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Controls.Animation;
using System.Collections.ObjectModel;

namespace Demo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private ObservableCollection<AnimationChannel> employees = new ObservableCollection<AnimationChannel>();


        public AnimationChannel Initialize(AnimationChannel channel, int type)
        {
            switch (type)
            {
                case 0:
                    {
                        channel.curve1 = new Curve();
                        channel.curve1.Keys.Add(new CurveKey(10, 10) { Continuity = CurveContinuity.Step });
                        channel.curve1.Keys.Add(new CurveKey(210, 100));
                        channel.curve1.Keys.Add(new CurveKey(420, 10));
                        channel.curve1.ComputeTangents(CurveTangent.Smooth);
                    }
                    break;
                case 1:
                    {
                        channel.curve1 = new Curve();
                        channel.curve1.Keys.Add(new CurveKey(10, 10) { Continuity = CurveContinuity.Step });
                        channel.curve1.Keys.Add(new CurveKey(310, 100));
                        channel.curve1.Keys.Add(new CurveKey(520, 10));
                        channel.curve1.ComputeTangents(CurveTangent.Smooth);
                    }
                    break;
                case 2:
                    {
                        channel.curve1 = new Curve();
                        channel.curve1.Keys.Add(new CurveKey(10, 10) { Continuity = CurveContinuity.Step });
                        channel.curve1.Keys.Add(new CurveKey(810, 100));
                        channel.curve1.Keys.Add(new CurveKey(1420, 10));
                        channel.curve1.ComputeTangents(CurveTangent.Smooth);
                    }
                    break;
            }



            channel.curve2 = new Curve();
            for (int i = 0; i < channel.curve1.Keys.Count; i++)
                channel.curve2.Keys.Add(new CurveKey(channel.curve1.Keys[i].Position, channel.curve1.Keys[i].Position) { Continuity = channel.curve1.Keys[i].Continuity });
            channel.curve2.ComputeTangents(CurveTangent.Smooth);
            return channel;
        }

        public MainWindow()
        {
            InitializeComponent();

            try
            {

                employees.Add(Initialize(new AnimationChannel() { Department = "Atmosphere", Name = "Fog Red Channel", Color = Colors.Red }, 0));
                employees.Add(Initialize(new AnimationChannel() { Department = "Atmosphere", Name = "Fog Blue Channel", Color = Colors.Blue }, 1));
                employees.Add(Initialize(new AnimationChannel() { Department = "Atmosphere", Name = "Fog Green Channel", Color = Colors.Green }, 2));

                CollectionView cv = (CollectionView)CollectionViewSource.GetDefaultView(employees);
                PropertyGroupDescription pgd = new PropertyGroupDescription("Department");
                cv.GroupDescriptions.Add(pgd);
                cv.Refresh();

                Part_AnimationEditor.ItemsSource = employees;

            }
            catch (Exception ex)
            {
            }
        }
    }
}
