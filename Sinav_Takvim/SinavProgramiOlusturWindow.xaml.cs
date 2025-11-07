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
        var dersListesi = DersSecimiWindow.GetDersSinav();
        if (dersListesi.Count == 0)
        {
            MessageBox.Show(
                "Lütfen sınavlar için ders seçimi yapınız!",
                "Uyarı",
                MessageBoxButton.OK);
            return;
        }
        
        var dersVeSureListesi = SinavSuresiWindow.GetDersSureListesi();
        if (dersVeSureListesi.Count == 0)
        {
            MessageBox.Show(
                "Lütfen sınavlar için süre ayarlaması yapınız!",
                "Uyarı",
                MessageBoxButton.OK);
            return;
        }
        
        if (BaslangicDatePicker.SelectedDate == null || BitisDatePicker.SelectedDate == null)
        {
            MessageBox.Show(
                "Lütfen tarih seçimlerini kontrol ediniz!",
                "Uyarı",
                MessageBoxButton.OK);
            return;
        }

        var baslangicTarihi = BaslangicDatePicker.SelectedDate.Value;
        var bitisTarihi = BitisDatePicker.SelectedDate.Value;
        if (baslangicTarihi > bitisTarihi)
        {
            MessageBox.Show(
                "Başlangıç tarihi bitiş tarihinden önce olmalı!",
                "Uyarı",
                MessageBoxButton.OK);
            return;
        }

        int baslangicSaati;
        if(int.TryParse(BaslangicSaatiBox.Text, out var result1))
        {
            baslangicSaati = result1;
        }
        else
        {
            MessageBox.Show(
                "Lütfen girilen saat değerlerini kontrol ediniz!",
                "Uyarı",
                MessageBoxButton.OK);
            return;
        }
        
        int bitisSaati;
        if(int.TryParse(BitisSaatiBox.Text, out var result2))
        {
            bitisSaati = result2;
        }
        else
        {
            MessageBox.Show(
                "Lütfen girilen saat değerlerini kontrol ediniz!",
                "Uyarı",
                MessageBoxButton.OK);
            return;
        }

        if (baslangicSaati > bitisSaati)
        {
            MessageBox.Show(
                "Başlangıç saati bitiş saatinden önce olmalı!",
                "Uyarı",
                MessageBoxButton.OK);
            return;
        }
        
        var sinavTuru = GetSinavSelection();
        if (sinavTuru == SinavTuru.Null)
        {
            MessageBox.Show(
                "Lütfen sınav türü için seçim yapın!",
                "Uyarı",
                MessageBoxButton.OK);
            return;
        }

        var beklemeSuresi = 0;
        if (int.TryParse(BeklemeSuresiTextBox.Text, out var result))
            beklemeSuresi = result;
        else
        {
            MessageBox.Show(
                "Lütfen bekleme süresini yazınız!",
                "Uyarı",
                MessageBoxButton.OK);
            return;
        }
        
        var ayniZaman = AyniZamanCheckBox.IsChecked == true;

        var window = new SinavProgramiWindow(dersVeSureListesi, baslangicTarihi, bitisTarihi, 
            sinavTuru, beklemeSuresi, ayniZaman, (baslangicSaati, bitisSaati), bolum);
        window.Show();
        Close();
    }
}