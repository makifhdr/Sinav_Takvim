using System.IO;
using System.Windows;
using System.Windows.Media;

namespace Sinav_Takvim;

public partial class DersListesiYukle
{
    private Bolum bolum;
    public DersListesiYukle(Bolum bolum)
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

    private void DersYukleButton_Click(object sender, RoutedEventArgs e)
    {
        if (File.Exists(filePath))
        {
            var result = MessageBox.Show(
                $"Yeni bir liste yüklenirken eski liste silinecektir. " +
                $"Ayrıca ders listesi silindiği için öğrenci-ders ilişkisi kaybolacağından öğrenci listesini " +
                $"yeniden yüklemeniz gerekmektedir. Devam etmek istiyor musunuz?",
                "Uyarı",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                ExcelReader reader = new ExcelReader();

                int sonuc = reader.DersListesiYukle(filePath, bolum);

                if (sonuc == -1)
                {
                    MessageBox.Show(
                        $"Ders listesi başarıyla yüklendi.",
                        "Uyarı",
                        MessageBoxButton.OK);
                    Close();
                }
                else
                {
                    MessageBox.Show(
                        $"Hata: Satır {sonuc}",
                        "Uyarı",
                        MessageBoxButton.OK);
                }
            }
        }
        else
        {
            MessageBox.Show(
                "Dosya bulunamadı.",
                "Uyarı",
                MessageBoxButton.OK);
        }
    }
}