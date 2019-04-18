using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Drawing;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;

namespace Lab4_5_TextRedactor
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<string> path = new List<string>();
        public MainWindow()
        {
            InitializeComponent();
            simbols_count.Content = "0";
            word_count.Content = "0";
            if (count == 0)
                Title += " Window " + 1;
            count++;
            DocBox.AddHandler(RichTextBox.DragOverEvent, new DragEventHandler(DocBox_DragOver), true);
            DocBox.AddHandler(RichTextBox.DropEvent, new DragEventHandler(DocBox_Drop), true);
            // LastFiles.ItemsSource = path;
            ReadLastFiles();
        }
        static int count = 0;
        private void New_Click(object sender, RoutedEventArgs e)
        {
            MainWindow w = new MainWindow();
            w.Title += " Window " + count;
            w.Show();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Text Files (*.txt)|*.txt|Rich Text Files (*.rtf)|*.rtf|XAML Files (*.xaml)|*.xaml|All files (*.*)|*.*";

            if (sfd.ShowDialog() == true)
            {
                path.Add(sfd.FileName);
                AddLastFiles();
                ReadLastFiles();
                TextRange doc = new TextRange(DocBox.Document.ContentStart, DocBox.Document.ContentEnd);
                using (FileStream fs = File.Create(sfd.FileName))
                {
                    if (System.IO.Path.GetExtension(sfd.FileName).ToLower() == "1.txt")
                        doc.Save(fs, DataFormats.Text);
                    else
                        doc.Save(fs, DataFormats.Xaml);
                }
            }
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Text Files (*.txt)| *.txt | Rich Text Files(*.rtf) | *.rtf | All files(*.*) | *.*";
            if (ofd.ShowDialog() == true)
            {
                path.Add(ofd.FileName);
                AddLastFiles();
                ReadLastFiles();

                TextRange doc = new TextRange(DocBox.Document.ContentStart, DocBox.Document.ContentEnd);
                using (FileStream fs = new FileStream(ofd.FileName, FileMode.Open))
                {
                    if (System.IO.Path.GetExtension(ofd.FileName).ToLower() == ".txt")
                        doc.Load(fs, DataFormats.Text);
                    else
                        doc.Load(fs, DataFormats.Xaml);
                }
            }
        }

        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            DocBox.Copy();
        }
        private void Paste_Click(object sender, RoutedEventArgs e)
        {
            DocBox.Paste();
        }
        private void FontType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var fontName = FontType.SelectedItem;

                if (fontName != null)
                {
                    DocBox.Selection.ApplyPropertyValue(FontFamilyProperty, fontName);
                    DocBox.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ToggleButton_Checked_Italic(object sender, RoutedEventArgs e)
        {
            DocBox.Selection.ApplyPropertyValue(TextElement.FontStyleProperty, FontStyles.Italic);
        }

        private void ToggleButton_Checked_Bold(object sender, RoutedEventArgs e)
        {
            DocBox.Selection.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
        }

        private void ToggleButton_Checked_Underline(object sender, RoutedEventArgs e)
        {
            DocBox.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Underline);
        }

        private void ToggleButton_Unchecked_Italic(object sender, RoutedEventArgs e)
        {
            DocBox.Selection.ApplyPropertyValue(TextElement.FontStyleProperty, FontStyles.Normal);

        }

        private void ToggleButton_Unchecked_Bold(object sender, RoutedEventArgs e)
        {
            DocBox.Selection.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Normal);

        }

        private void ToggleButton_Unchecked_Underline(object sender, RoutedEventArgs e)
        {
            DocBox.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, null);

        }

        private void Russia_Click(object sender, RoutedEventArgs e)
        {
            this.Resources.MergedDictionaries[0] = new ResourceDictionary() { Source = new Uri(@"E:\Учеба\2 курс\ООП\OOP2\Lab4-5_TextRedactor\Lab4-5_TextRedactor\Resources\LocalaizeResources.ru.xaml") };
        }

        private void English_Click(object sender, RoutedEventArgs e)
        {
            this.Resources.MergedDictionaries[0] = new ResourceDictionary() { Source = new Uri(@"E:\Учеба\2 курс\ООП\OOP2\Lab4-5_TextRedactor\Lab4-5_TextRedactor\Resources\LocalaizeResources.xaml") };
        }

        private void DocBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = new TextRange(DocBox.Document.ContentStart, DocBox.Document.ContentEnd).Text;
            Regex reg = new Regex(@"\W+");
            simbols_count.Content = text.Length - 2;
            word_count.Content = ((reg.Matches(text)).Count).ToString();
        }

        private void slider_size_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (DocBox != null)
                DocBox.Selection.ApplyPropertyValue(TextElement.FontSizeProperty, slider1.Value);
        }

        private void fontcolor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (fontcolor.SelectedItem.ToString().Contains("Yellow") || fontcolor.SelectedItem.ToString().Contains("Желтый"))
                DocBox.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Yellow);
            if (fontcolor.SelectedItem.ToString().Contains("Black") || fontcolor.SelectedItem.ToString().Contains("Черный"))
                DocBox.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Black);
            if (fontcolor.SelectedItem.ToString().Contains("Blue") || fontcolor.SelectedItem.ToString().Contains("Синий"))
                DocBox.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Blue);
            if (fontcolor.SelectedItem.ToString().Contains("Red") || fontcolor.SelectedItem.ToString().Contains("Красный"))
                DocBox.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Red);
            if (fontcolor.SelectedItem.ToString().Contains("Green") || fontcolor.SelectedItem.ToString().Contains("Зеленый"))
                DocBox.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Green);
        }

        private void DocBox_Drop(object sender, DragEventArgs e)
        {
            MainWindow win = new MainWindow();
            win.Show();
            string[] docPath = (string[])e.Data.GetData(DataFormats.FileDrop);

            TextRange range = new System.Windows.Documents.TextRange(win.DocBox.Document.ContentStart, win.DocBox.Document.ContentEnd);

            FileStream fStream = new System.IO.FileStream(docPath[0], System.IO.FileMode.OpenOrCreate);
            range.Load(fStream, DataFormats.Text);

            path.Add(docPath[0]);
            AddLastFiles();
            ReadLastFiles();

            fStream.Close();
            win.Title = docPath[0];
        }

        private void DocBox_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.All;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
            e.Handled = false;
        }

        private void OpenLast_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 1; i <= path.Count; i++)
            {
                MainWindow wind = new MainWindow();
                wind.Title = path[i];
                TextRange doc = new TextRange(DocBox.Document.ContentStart, DocBox.Document.ContentEnd);
                using (FileStream fs = new FileStream(path[i], FileMode.Open))
                {
                    doc.Load(fs, DataFormats.Text);
                    fs.Close();
                }
            }
        }

        private void Dark_Click(object sender, RoutedEventArgs e)
        {
            this.Resources.MergedDictionaries[1] = new ResourceDictionary() { Source = new Uri(@"E:\Учеба\2 курс\ООП\OOP2\Lab4-5_TextRedactor\Lab4-5_TextRedactor\Resources\DesignDark.xaml") };
        }

        private void Light_Click(object sender, RoutedEventArgs e)
        {
            this.Resources.MergedDictionaries[1] = new ResourceDictionary() { Source = new Uri(@"E:\Учеба\2 курс\ООП\OOP2\Lab4-5_TextRedactor\Lab4-5_TextRedactor\Resources\DesignLight.xaml") };
        }

        private void Optim_Click(object sender, RoutedEventArgs e)
        {
            this.Resources.MergedDictionaries[1] = new ResourceDictionary() { Source = new Uri(@"E:\Учеба\2 курс\ООП\OOP2\Lab4-5_TextRedactor\Lab4-5_TextRedactor\Resources\DesignOptim.xaml") };
        }

        String pathLF = null;

        private void LastFileOp(object sender, SelectionChangedEventArgs e)
        {
            pathLF = LastFiles.SelectedItem.ToString();

            MainWindow win = new MainWindow();
            win.Title = pathLF;
            FileStream fileStream = new FileStream(pathLF, FileMode.Open);
            TextRange range = new TextRange(win.DocBox.Document.ContentStart, win.DocBox.Document.ContentEnd);
            range.Load(fileStream, DataFormats.Text);
            fileStream.Close();


            win.Show();
        }

        void AddLastFiles()
        {
            using (StreamWriter fileStream = new StreamWriter(@"E:\Учеба\2 курс\ООП\OOP2\Lab4-5_TextRedactor\Lab4-5_TextRedactor\bin\Debug\WriteLines.txt"))
            {
                foreach (string ph in path)
                {
                    fileStream.WriteLine(ph);
                }
            }
        }

        void ReadLastFiles()
        {
            if (File.Exists(@"E:\Учеба\2 курс\ООП\OOP2\Lab4-5_TextRedactor\Lab4-5_TextRedactor\bin\Debug\WriteLines.txt"))
            {
                path.Clear();
                string[] readText = File.ReadAllLines(@"E:\Учеба\2 курс\ООП\OOP2\Lab4-5_TextRedactor\Lab4-5_TextRedactor\bin\Debug\WriteLines.txt");
                foreach (string s in readText)
                {
                    path.Add(s);
                }
                path.Distinct();
                LastFiles.ItemsSource = null;
                LastFiles.ItemsSource = path;
            }
            else
            {
                File.Create(@"E:\Учеба\2 курс\ООП\OOP2\Lab4-5_TextRedactor\Lab4-5_TextRedactor\bin\Debug\WriteLines.txt");
            }

        }
    }
}
