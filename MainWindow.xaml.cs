using Sastopeit.RegexTester.Checkers;
using System;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace Sastopeit.RegexTester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {
        Presets p = new Presets();
        Reg r = new Reg();
        bool b;
        DispatcherTimer timer;

        public MainWindow()
        {
            InitializeComponent();

            // Menge von ComboBoxItems generieren, die der Menge von Presets entspricht
            int plc = p.PresetList.Count;
            for (int i = 1; i <= plc; i++)
            {
                CB_Preset.Items.Add(i);
            }
        }

        private void CB_Preset_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CHB_PresetAutoLoad.IsChecked == true)
            {
                // Für das frisch selektierte ComboBoxItem die StringCollection mit derselben Indexnummer laden
                int i = CB_Preset.SelectedIndex;
                StringCollection sc = p.PresetList[i];

                // Letztendlich die Strings der zugewiesenen StringCollection in die TextBoxen laden
                TBX_Intention.Text = sc[0];
                TBX_String.Text = sc[1];
                TBX_RegEx.Text = sc[2];
            }
        }

        private void BT_PresetLoad_Click(object sender, RoutedEventArgs e)
        {
            int i = CB_Preset.SelectedIndex;
            StringCollection sc = p.PresetList[i];

            TBX_Intention.Text = sc[0];
            TBX_String.Text = sc[1];
            TBX_RegEx.Text = sc[2];
        }

        private void CHB_PresetAutoLoad_StateChanged(object sender, RoutedEventArgs e)
        {
            if (null != this.BT_PresetAutoLoad)
            {
                BT_PresetAutoLoad.Visibility = true == CHB_PresetAutoLoad.IsChecked ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        private void CB_Preset_MouseEnter(object sender, MouseEventArgs e)
        {
            CB_Preset.Focus();
        }



        private void BT_Clipboard_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(TBX_RegEx.Text);
        }



        private void TB_String_TextChanged(object sender, TextChangedEventArgs e)
        {
            PreCheck();
        }
        private void TB_Regex_TextChanged(object sender, TextChangedEventArgs e)
        {
            PreCheck();
        }
        private void PreCheck()
        {
            if (TBX_String.Text != "")
            {
                if (TBX_RegEx.Text != "")
                {
                    LBL_Nostring.Visibility = Visibility.Collapsed;
                    // TODO: Hier den String VOR dem Check() prüfen, aktuell z.B. zum Verhindern der Exception "Invalid pattern '(.)\' Illegal \\ at end of pattern."
                    Check();
                }
                else
                {
                    TBX_RegEx.Background = Brushes.White;
                    LBL_RegEx_True.Visibility = LBL_RegEx_False.Visibility = LBL_Nostring.Visibility = TBL_Suggest.Visibility = BT_Clipboard.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                TBX_RegEx.Background = Brushes.White;
                LBL_RegEx_True.Visibility = LBL_RegEx_False.Visibility = LBL_Nostring.Visibility = TBL_Suggest.Visibility = BT_Clipboard.Visibility = Visibility.Collapsed;
                if (TBX_RegEx.Text != "") LBL_Nostring.Visibility = Visibility.Visible;
            }
        }



        private void Check()
        {
            b = r.RegExp(TBX_String.Text, TBX_RegEx.Text);

            if (b)
            {
                TBX_RegEx.Background = Brushes.PaleGreen;
                LBL_RegEx_False.Visibility = TBL_Suggest.Visibility = Visibility.Collapsed;
                LBL_RegEx_True.Visibility = BT_Clipboard.Visibility = Visibility.Visible;
            }
            else
            {
                TBX_RegEx.Background = Brushes.LightPink;
                LBL_RegEx_True.Visibility = BT_Clipboard.Visibility = Visibility.Collapsed;
                LBL_RegEx_False.Visibility = TBL_Suggest.Visibility = Visibility.Visible;
                Suggest();
            }
        }



        private void Suggest()
        {
            // https://stackoverflow.com/questions/541954/how-would-you-count-occurrences-of-a-string-actually-a-char-within-a-string/541976
            // ... Die unten genutzte Methode mit Vorkommen-Zählungen braucht recht viel Code, ist aber am performantesten

            TBL_Suggest.Content = "";


            int countBracketOpen = 0;
            int countBracketClose = 0;
            int countCurlyOpen = 0;
            int countCurlyClose = 0;

            foreach (char c in TBX_RegEx.Text)
            {
                if (c == '[') countBracketOpen++;
                if (c == ']') countBracketClose++;
                if (c == '{') countCurlyOpen++;
                if (c == '}') countCurlyClose++;
            }

            // Eckige Klammer(n) für Bereich(e) - mind. ein [ bzw. ] fehlt
            if (countBracketOpen < countBracketClose) TBL_Suggest.Content += "❌ Wahrscheinlich fehlt ein [ (Zeichenauswahl)\n";
            if (countBracketOpen > countBracketClose) TBL_Suggest.Content += "❌ Wahrscheinlich fehlt ein ] (Zeichenauswahl)\n";

            // Geschweifte Klammer(n) für Quantifizierer - { bzw. } fehlt x-mal
            int i = Math.Abs(countCurlyClose - countCurlyOpen);
            if (countCurlyOpen < countCurlyClose) TBL_Suggest.Content += $"❌ Quantifizierer: {{ fehlt {i}x oder }} ist {i}x überflüssig\n";
            if (countCurlyOpen > countCurlyClose) TBL_Suggest.Content += $"❌ Quantifizierer: }} fehlt {i}x oder {{ ist {i}x überflüssig\n";
            // TODO: Kleines Problem beheben - Information, wenn MEHRERE Quantifizierer auftauchen + bei EINEM ein { fehlt und bei einem ANDEREN ein }

            // The easy ones - TODO: #2 und folgende ggf.umsetzen wie #1
            if (r.RegExp(TBX_RegEx.Text, "{[ ]*}"))
                TBL_Suggest.Content += "❌ Quantifizierer: {} Klammern müssen Zahlenwert(e) beinhalten\n";
            if (TBX_RegEx.Text.Contains("[]"))
                TBL_Suggest.Content += "❌ Zeichenauswahl: [] Klammern dürfen nicht leer sein\n";
            if (TBX_RegEx.Text.Contains("[^]") | TBX_RegEx.Text.Contains("[^ ]") | TBX_RegEx.Text.Contains("[^  ]"))
                TBL_Suggest.Content += "\n❌ Zeichenauswahl: Nach Negierung ^ müssen Zeichenangaben folgen";
        }



        private void TBX_RegEx_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            int cursor = TBX_RegEx.CaretIndex;

            // RegEx-Syntaxfehler schon bei der Eingabe unterbinden, anschließend ggf. Autocomplete

            // Quantifizierer
            if (e.Text == "{" | e.Text == "+" | e.Text == "?" | e.Text == "*")
            {
                e.Handled = true;

                // (new foobar()).Methode() = Kurzform für Objekterstellung, wenn Methode nur einmal gebraucht wird
                if ((new QuantifierCheckerList()).Check(cursor, TBX_RegEx.Text) == false)
                {
                    LBL_Denyinfo.Visibility = Visibility.Visible;
                    LBL_Denyinfo.Content = "❌ Quantifizierer kann nicht auf Quantifizierer folgen";
                    Timer(3);
                }
                else if (CBX_Autocomplete.IsChecked == true)
                {
                    TBX_RegEx.Text = TBX_RegEx.Text.Insert(cursor, "{2}");
                    TBX_RegEx.Select(cursor + 1, 1);
                }
            }

            // Zeichenauswahl
            if (e.Text == "[" && CBX_Autocomplete.IsChecked == true)
            {
                e.Handled = true;
                TBX_RegEx.Text = TBX_RegEx.Text.Insert(cursor, "[A-Za-zÄÖÜäöüß]");
                TBX_RegEx.Select(cursor + 1, 13);
            }

            // TODO: Weitere Fehleingaben unterbinden und/oder Autocompletes, sofern sinnvoll!
        }

        private void Timer(int seconds)
        {
            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(Timer_Tick);
            timer.Interval = new TimeSpan(0, 0, seconds);
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            LBL_Denyinfo.Visibility = Visibility.Collapsed;
            timer.IsEnabled = false;
        }



        // TODO: Unbehandelte Exceptions (System.Text.RegularExpressions.RegexParseException) abfangen und behandeln:
        // ... Kann ich (alle) Fehleingaben, die zu den folgenden Exceptions führen würden, schon in TBX_RegEx_PreviewTextInput() entschärfen ?!?
        // ... Kompletter Log einer Exception siehe _exception.txt im Stammverzeichnis dieses Projekts
        //
        // #1 "Invalid pattern '{2}' Quantifier {x,y} following nothing."
        // #2 "Invalid pattern '[]' Unterminated [] set."
        // #3 "Invalid pattern '[A-z' Unterminated [] set."
        // #4 "Invalid pattern '(.)\' Illegal \\ at end of pattern."
        // #5 "Invalid pattern '^[A-0-9\.!#$%&'*+/=?^`{|}~-]{1,64}@[A-z0-9_\.-]+[a-z]{2,}$' [x-y] range in reverse order."
        // #6 "Invalid pattern '^([01][0-6]|2[0-3]:[0-5][0-9]:[0-5][0-9]$' Not enough )'s."
        //
        // ... SCHON BEHANDELT (aber nicht konventionell, sondern proaktiv qua Eingabesperrung in TBX_RegEx_PreviewTextInput() !!1!):
        // - "Invalid pattern '^[1-9][0-9]{3}{2,}$' Nested quantifier '{'."
        // 
        // ... MÖGLICHE LÖSUNG FÜR obige Exception #4 siehe TODO in PreCheck()!
    }
}
