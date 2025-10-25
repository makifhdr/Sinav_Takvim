using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Sinav_Takvim;

public partial class KullaniciEkleWindow
{
    KullaniciDAO KullaniciDAO = new KullaniciDAO();
    public KullaniciEkleWindow()
    {
        InitializeComponent();
    }
    
    private void Onayla_Button_Click(object sender, EventArgs e)
    {
        if (BolumComboBox.SelectedItem is ComboBoxItem selectedItem)
        {
            if (KullaniciDAO.KullaniciEkle(epostaBox.Text, sifreBox.Text,
                    Enum.Parse<Bolum>(selectedItem.Tag.ToString() ?? throw new InvalidOperationException())))
            {
                ReturnText.Foreground = new SolidColorBrush(Colors.Green);
                ReturnText.Content = "Kullanıcı kaydı başarılı!";
            }
            else
            {
                ReturnText.Foreground = new SolidColorBrush(Colors.Red);
                ReturnText.Content = "Hatalı veri girişi";
            }
        }
        
    }
}