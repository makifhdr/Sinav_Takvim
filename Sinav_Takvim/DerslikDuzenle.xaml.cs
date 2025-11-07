using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Sinav_Takvim;

public partial class DerslikDuzenle
{
    private Derslik derslik;
    public DerslikDuzenle(Derslik derslik)
    {
        this.derslik = derslik;
        InitializeComponent();
        TextBoxes();
    }

    private void TextBoxes()
    {
        derslikKodBox.Text = derslik.GetKod();
        derslikAdBox.Text = derslik.GetAd();
        satirSayisiBox.Text = derslik.GetSatir().ToString();
        sutunSayisiBox.Text = derslik.GetSutun().ToString();
        siraYapisiBox.Text = derslik.GetSira_yapisi().ToString();
        KapasiteBox.Text = derslik.GetKapasite().ToString();
    }
    
    private void OnaylaButton_Click(object sender, RoutedEventArgs e)
    {
        var DerslikDAO = new DerslikDAO();
        
        if (derslikKodBox.Text.Length == 0 || derslikAdBox.Text.Length == 0 || !int.TryParse(satirSayisiBox.Text, out _)
            || !int.TryParse(sutunSayisiBox.Text, out _) || !int.TryParse(siraYapisiBox.Text, out _)
            || !int.TryParse(KapasiteBox.Text, out _))
        {
            MessageBox.Show(
                "Hata: Lütfen girdiğiniz bilgileri kontrol ediniz",
                "Uyarı",
                MessageBoxButton.OK);
            return;
        }
        
        if (!DerslikDAO.DerslikDuzenle(new Derslik(derslik.GetKod(), derslikAdBox.Text, int.Parse(satirSayisiBox.Text),
                int.Parse(sutunSayisiBox.Text), int.Parse(siraYapisiBox.Text), 
                int.Parse(KapasiteBox.Text), derslik.GetBolum())))
        {
            MessageBox.Show(
                "Hata: Lütfen bilgileri kontrol ediniz",
                "Uyarı",
                MessageBoxButton.OK);
            return;
        }
        MessageBox.Show(
            "Derslik Düzenleme Başarılı!",
            "Uyarı",
            MessageBoxButton.OK);
    }
    
    private void TextBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        if (!int.TryParse(satirSayisiBox.Text, out _) || !int.TryParse(sutunSayisiBox.Text, out _)
                                                      || !int.TryParse(siraYapisiBox.Text, out _))
        {
            return;
        }
        var satir = !satirSayisiBox.Text.Equals("") ? int.Parse(satirSayisiBox.Text) : 0;
        var sutun = !sutunSayisiBox.Text.Equals("") ? int.Parse(sutunSayisiBox.Text) : 0;
        var sira = !siraYapisiBox.Text.Equals("") ? int.Parse(siraYapisiBox.Text) : 0;

        if(sira != 0)
            KapasiteBox.Text = (satir * sutun *((sira + 1) / 2)).ToString();
    }
}