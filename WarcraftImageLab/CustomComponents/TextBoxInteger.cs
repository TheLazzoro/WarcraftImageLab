using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WarcraftImageLabV2.CustomComponents
{
    public class TextBoxInteger : TextBox
    {
        public TextBoxInteger()
        {
            Style style = this.FindResource("TextBoxStyle") as Style;
            Style = style;
            TextChanged += TextBoxInteger_TextChanged;
        }

        int previousNumber = 1;
        private void TextBoxInteger_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                int newNumber = int.Parse(Text);
                previousNumber = newNumber;
            }
            catch (Exception)
            {
                Text = previousNumber.ToString();
            }
        }
    }
}
