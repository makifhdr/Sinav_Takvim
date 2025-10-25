using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Sinav_Takvim;

public partial class DerslikOlustur
{
    private Bolum bolum;
    public DerslikOlustur(Bolum bolum)
    {
        this.bolum = bolum;
        InitializeComponent();
    }

    private void OnaylaButton_Click(object sender, RoutedEventArgs e)
    {
        var DerslikDAO = new DerslikDAO();

        if (derslikKodBox.Text.Length == 0 || derslikAdBox.Text.Length == 0 || !int.TryParse(satirSayisiBox.Text, out _)
            || !int.TryParse(sutunSayisiBox.Text, out _) || !int.TryParse(siraYapisiBox.Text, out _)
            || !int.TryParse(KapasiteBox.Text, out _))
        {
            ReturnText.Foreground = new SolidColorBrush(Colors.Red);
            ReturnText.Content = "Hata: Lütfen girdiğiniz bilgileri kontrol ediniz";
            return;
        }
        
        if (!DerslikDAO.DerslikEkle(new Derslik(derslikKodBox.Text, derslikAdBox.Text, int.Parse(satirSayisiBox.Text),
                int.Parse(sutunSayisiBox.Text), int.Parse(siraYapisiBox.Text), 
                int.Parse(KapasiteBox.Text), bolum)))
        {
            ReturnText.Foreground = new SolidColorBrush(Colors.Red);
            ReturnText.Content = "Hata: Bu derslik kodu ile bir derslik zaten mevcut!\nFarklı bir derslik numarası girmeyi deneyin.";
            return;
        }
        ReturnText.Foreground = new SolidColorBrush(Colors.Green);
        ReturnText.Content = "Derslik ekleme başarılı!";
    }
    

    private void TextBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        var satir = !satirSayisiBox.Text.Equals("") ? int.Parse(satirSayisiBox.Text) : 0;
        var sutun = !sutunSayisiBox.Text.Equals("") ? int.Parse(sutunSayisiBox.Text) : 0;
        var sira = !siraYapisiBox.Text.Equals("") ? int.Parse(siraYapisiBox.Text) : 0;
        if(sira != 0)
            KapasiteBox.Text = (satir * sutun *((sira + 1) / 2)).ToString();
    }
}