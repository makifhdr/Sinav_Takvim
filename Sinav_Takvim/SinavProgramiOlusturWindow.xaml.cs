using System.Windows;
using System.Windows.Media;

namespace Sinav_Takvim;

public partial class SinavProgramiOlusturWindow
{
    private Bolum bolum;
    private DersDAO dersDAO;
    private DerslikDAO deslikDAO;
    private OgrenciDAO ogrenciDAO;
    private OgrenciDersDAO ogrenciDersDAO;

    public SinavProgramiOlusturWindow() { }
    public SinavProgramiOlusturWindow(Bolum bolum)
    {
        this.bolum = bolum;
        InitializeComponent();
    }

    private void DersSecimiButton_Click(object sender, RoutedEventArgs e)
    {
        var window = new DersSecimiWindow(bolum);
        window.Show();
    }

    private void SinavSuresiButton_Click(object sender, RoutedEventArgs e)
    {
        var list =  DersSecimiWindow.GetDersSinav();

        if (list.Count == 0)
        {
            MessageBox.Show(
                "Lütfen önce ders seçimi yapınız!",
                "Uyarı",
                MessageBoxButton.OK);
            return;
        }
        
        var window = new SinavSuresiWindow(list, bolum);
        window.Show();
    }

    private SinavTuru GetSinavSelection()
    {
        if (VizeRadioButton.IsChecked == true)
        {
            return SinavTuru.Vize;
        }
        if (FinalRadioButton.IsChecked == true)
        {
            return SinavTuru.Final;
        }
        if (ButunlemeRadioButton.IsChecked == true)
        {
            return SinavTuru.Butunleme;
        }

        return SinavTuru.Null;
    }

    private void ProgramiOlusturButton_Click(object sender, RoutedEventArgs e)
    {
        var dersVeSureListesi = SinavSuresiWindow.GetDersSureListesi();
        if (dersVeSureListesi.Count == 0)
        {
            ResultText.Foreground = new SolidColorBrush(Colors.Red);
            ResultText.Text = "Lütfen sınavlar için süre ayarlaması yapınız!";
            return;
        }
        
        if (BaslangicDatePicker.SelectedDate == null || BitisDatePicker.SelectedDate == null)
        {
            ResultText.Foreground = new SolidColorBrush(Colors.Red);
            ResultText.Text = "Lütfen tarih seçimlerini kontrol ediniz!";
            return;
        }

        var baslangicTarihi = BaslangicDatePicker.SelectedDate.Value;
        var bitisTarihi = BitisDatePicker.SelectedDate.Value;
        if (baslangicTarihi > bitisTarihi)
        {
            ResultText.Foreground = new SolidColorBrush(Colors.Red);
            ResultText.Text = "Başlangıç tarihi bitiş tarihinden önce olmalı!";
            return;
        }

        int baslangicSaati;
        if(int.TryParse(BaslangicSaatiBox.Text, out var result1))
        {
            baslangicSaati = result1;
        }
        else
        {
            ResultText.Foreground = new SolidColorBrush(Colors.Red);
            ResultText.Text = "Lütfen girilen saat değerlerini kontrol ediniz!";
            return;
        }
        
        int bitisSaati;
        if(int.TryParse(BitisSaatiBox.Text, out var result2))
        {
            bitisSaati = result2;
        }
        else
        {
            ResultText.Foreground = new SolidColorBrush(Colors.Red);
            ResultText.Text = "Lütfen girilen saat değerlerini kontrol ediniz!";
            return;
        }

        if (baslangicSaati > bitisSaati)
        {
            ResultText.Foreground = new SolidColorBrush(Colors.Red);
            ResultText.Text = "Başlangıç saati bitiş saatinden önce olmalı!";
            return;
        }
        
        var sinavTuru = GetSinavSelection();
        if (sinavTuru == SinavTuru.Null)
        {
            ResultText.Foreground = new SolidColorBrush(Colors.Red);
            ResultText.Text = "Lütfen sınav türü için seçim yapın!";
            return;
        }

        var beklemeSuresi = 0;
        if (int.TryParse(BeklemeSuresiTextBox.Text, out var result))
            beklemeSuresi = result;
        else
        {
            ResultText.Foreground = new SolidColorBrush(Colors.Red);
            ResultText.Text = "Lütfen bekleme süresini yazınız!";
            return;
        }
        
        var ayniZaman = AyniZamanCheckBox.IsChecked == true;

        var window = new SinavProgramiWindow(dersVeSureListesi, baslangicTarihi, bitisTarihi, 
            sinavTuru, beklemeSuresi, ayniZaman, (baslangicSaati, bitisSaati), bolum);
        window.Show();
        Close();
    }
}