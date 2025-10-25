using System.IO;
using System.Windows;
using System.Windows.Media;

namespace Sinav_Takvim;

public partial class OgrenciListesiYukle
{
    private Bolum bolum;
    public OgrenciListesiYukle(Bolum bolum)
    {
        this.bolum = bolum;
        InitializeComponent();
    }
    
    private void Border_DragOver(object sender, DragEventArgs e)
    {
        if (e.Data.GetDataPresent(DataFormats.FileDrop))
            e.Effects = DragDropEffects.Copy;
        else
            e.Effects = DragDropEffects.None;

        e.Handled = true;
    }

    private string filePath;
    private void Border_Drop(object sender, DragEventArgs e)
    {
        if (e.Data.GetDataPresent(DataFormats.FileDrop))
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            
            filePath = files[0];
            InfoText.Text = $"Seçilen dosya:\n{filePath}";

        }
    }

    private void OgrenciYukleButton_Click(object sender, RoutedEventArgs e)
    {

        if (File.Exists(filePath))
        {
            var result = MessageBox.Show(
                $"Yeni bir liste yüklenirken eski liste silinecektir. Devam etmek istiyor musunuz?",
                "Uyarı",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                ExcelReader reader = new ExcelReader();

                int sonuc = reader.OgrenciListesiYukle(filePath, bolum);

                if (sonuc == -1)
                {
                    ResultText.Foreground = new SolidColorBrush(Colors.Green);
                    ResultText.Text = "Öğrenci listesi yükleme işlemi başarılı!";
                }
                else
                {
                    ResultText.Foreground = new SolidColorBrush(Colors.Red);
                    ResultText.Text = $"Hata: Satır {sonuc}";
                }
            }
        }
        else
        {
            ResultText.Foreground = new SolidColorBrush(Colors.Red);
            ResultText.Text = $"Dosya bulunamadı.";
        }
    }
}