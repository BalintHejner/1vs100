using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using System.Drawing.Text;


namespace _1_a_100_ellen
{
    public partial class MainWindow : Window
    {
        public struct Kérdés
        {
            public int Sorszám;
            public string Kérdésmaga;
            public string A_válasz;
            public string B_válasz;
            public string C_válasz;
            public string Helyes;
        }

        private const string FontPath = "Digital7-1e1Z.ttf";

        public List<Kérdés> kérdések = new List<Kérdés>();
        public List<Label> labelök = new List<Label>();
        public List<Label> labelökjó = new List<Label>();
        public List<string> labelök_nevei = new List<string>();
        public List<Label> rossz_labelök = new List<Label>();
        public List<int> hányrosszkéthatár = new List<int>();
        public List<string> változók = new List<string>();
        public List<string> sorsoltválaszok = new List<string>();
        public List<int> rosszrandomszámok = new List<int>();

        public int forduló = 1;
        public bool segítségethasznál = false;
        public int menekülőútszám = 0;
        int fordulókérdésszám = 0;
        public int összeg = 0;
        public int falkaszám = 100;
        public int szám = 0;
        public int aktkérdésszám = 0;
        public int rosszdb = 0;
        private bool valasz1megnyomva = false;
        private bool valasz2megnyomva = false;
        private bool valasz3megnyomva = false;
        private bool segitsegmegnyomva = false;
        public int összrosszválasz;
        public Kérdés akt = new Kérdés();
        public string[] válaszok = { "A", "B", "C" };
        int[] nyereményekperkérdés = { 1000, 4000, 8000, 16000, 32000, 50000, 60000, 80000, 100000, 200000, 400000, 500000 };
        Random r = new Random();

        public void Változók()
        {
            foreach (var item in File.ReadAllLines("változók.txt"))
            {
                string[] adat1 = item.Split(';');
                változók.Add(adat1[1]);
            }
            forduló = int.Parse(változók[0]);
            fordulókérdésszám = int.Parse(változók[1]);
            összeg = int.Parse(változók[2]);
            falkaszám = int.Parse(változók[3]);
            szám = int.Parse(változók[4]);
            aktkérdésszám = int.Parse(változók[5]);
            rosszdb = int.Parse(változók[6]);
            menekülőútszám = int.Parse(változók[7]);
        }
        public void Kérdésbeolvasás()
        {
            foreach (var kérdés in File.ReadAllLines("Kérdések.txt").Skip(1))
            {
                Kérdés új = new Kérdés();
                string[] kérdésadatok = kérdés.Split('\t');
                új.Sorszám = int.Parse(kérdésadatok[0]);
                új.Kérdésmaga = kérdésadatok[1];
                új.A_válasz = kérdésadatok[2];
                új.B_válasz = kérdésadatok[3];
                új.C_válasz = kérdésadatok[4];
                új.Helyes = kérdésadatok[5];
                kérdések.Add(új);
            }
        }
        public void Kérdésfeltétel()
        {
            for (int i = 0; i < kérdések.Count; i++)
            {
                if (kérdések[i].Sorszám == forduló)
                {
                    fordulókérdésszám++;
                }
            }
            szám = 0;
            aktkérdésszám = r.Next(0, fordulókérdésszám);
            for (int i = 0; i < kérdések.Count; i++)
            {
                if (kérdések[i].Sorszám == forduló)
                {
                    if (szám == aktkérdésszám)
                    {
                        akt = kérdések[i];
                        break;
                    }
                    else
                    {
                        szám++;
                    }
                }
            }
        }
        public string Összegkiírás(string összeg)
        {
            string újösszeg = "";
            int jegyfigyelő = összeg.Length % 3;
            string próbaösszeg = összeg.ToString();
            //int számláló = 0;
            if (jegyfigyelő == 1)
            {
                próbaösszeg = "//" + próbaösszeg;
            }
            if (jegyfigyelő == 2)
            {
                próbaösszeg = "/" + próbaösszeg;
            }
            for (int i = 0; i <= összeg.Length + 1; i++)
            {
                if (i % 3 == 0 && i > 0)
                {
                    újösszeg += " ";
                }
                try
                {
                    újösszeg += próbaösszeg[i];
                }
                catch (Exception)
                {
                    újösszeg += "";
                }
            }
            újösszeg = újösszeg.Replace("/", "");
            if (jegyfigyelő == 2)
            {
                újösszeg = újösszeg.Remove(újösszeg.Length - 1);
            }
            return újösszeg;
        }
        public void Szövegek()
        {
            //Random r = new Random();
            kérdés.Content = akt.Kérdésmaga;
            valasz1.Content = akt.A_válasz;
            valasz2.Content = akt.B_válasz;
            valasz3.Content = akt.C_válasz;
            xy_ellen.Content = falkaszám.ToString() + "\nELLEN";
            jelenlegi_összeg.Content = Összegkiírás(összeg.ToString()) + " Ft";
            if (forduló <= 11)
            {
                nyereményfa.Content = Összegkiírás(nyereményekperkérdés[forduló - 1].ToString()) + " Ft";
            }
            else
            {
                nyereményfa.Content = "500 000 Ft";
            }
            //Kérdés aktkérdés = kérdések[r.Next(0, fordulókérdésszám + 1)];
        }
        public void Elhelyezés()
        {
            //A_100_spártai
            int képernyőszél = 1360;
            int képernyőmag = 628;
            valasz1.Width = (A_100_spártai.Width - képernyőszél / 40 * 2) / 3;
            valasz2.Width = valasz1.Width;
            valasz3.Width = valasz1.Width;
            A_100_spártai.Margin = new Thickness(képernyőszél / 40, képernyőmag / 20, 0, 0);
            kérdés.Margin = new Thickness(képernyőszél / 40, képernyőmag / 15 + A_100_spártai.Height, 0, 0);
            valasz1.Margin = new Thickness(képernyőszél / 40, képernyőmag / 40 * 5 + kérdés.Height + A_100_spártai.Height, 0, 0);
            valasz2.Margin = new Thickness(képernyőszél / 40 * 2 + valasz1.Width, képernyőmag / 40 * 5 + kérdés.Height + A_100_spártai.Height, 0, 0);
            valasz3.Margin = new Thickness(képernyőszél / 40 * 3 + valasz1.Width * 2, képernyőmag / 40 * 5 + kérdés.Height + A_100_spártai.Height, 0, 0);
            A.Margin = new Thickness(képernyőszél / 40 + valasz1.Width / 2 - A.Width / 2, képernyőmag / 10 + A_100_spártai.Height + kérdés.Height, 0, 0);
            B.Margin = new Thickness(540, 337, 0, 0);
            C.Margin = new Thickness(895, 337, 0, 0);
            emberke.Margin = new Thickness(képernyőszél / 40, képernyőmag / 40 * 7 + kérdés.Height + A_100_spártai.Height + valasz1.Height, 0, 0);
            A_vagy_Az.Margin = new Thickness(képernyőszél / 40 - képernyőszél / 200 + emberke.Width, képernyőmag / 40 * 7 + kérdés.Height + A_100_spártai.Height + valasz1.Height + emberke.Height / 2 - A_vagy_Az.Height / 2, 0, 0);
            xy_ellen.Margin = new Thickness(képernyőszél / 40 - képernyőszél / 150 + emberke.Width + A_vagy_Az.Width, képernyőmag / 40 * 7 + kérdés.Height + A_100_spártai.Height + valasz1.Height, 0, 0);
            //emberke.Margin = new Point(képernyőszél / 40, képernyőmag / 40 * 7 + kérdés.Height + A_100_spártai.Height + valasz1.Height);        
            //emberke.Margin = new Point(képernyőszél / 40, képernyőmag / 40 * 7 + kérdés.Height + A_100_spártai.Height + valasz1.Height);        
            jelenlegi_összeg.Margin = new Thickness(képernyőszél / 40 - képernyőszél / 150 + képernyőszél / 40 + emberke.Width + A_vagy_Az.Width + xy_ellen.Width, képernyőmag / 40 * 7 + kérdés.Height + A_100_spártai.Height + valasz1.Height, 0, 0);
            nyereményfa.Margin = new Thickness(képernyőszél / 40 - képernyőszél / 150 + képernyőszél / 40 * 2 + emberke.Width + A_vagy_Az.Width + xy_ellen.Width + jelenlegi_összeg.Width, képernyőmag / 40 * 7 + kérdés.Height + A_100_spártai.Height + valasz1.Height, 0, 0);
            segitseg.Margin = new Thickness(képernyőszél / 40 - képernyőszél / 150 + képernyőszél / 40 * 3 + emberke.Width + A_vagy_Az.Width + xy_ellen.Width + jelenlegi_összeg.Width + nyereményfa.Width, képernyőmag / 40 * 7 + kérdés.Height + A_100_spártai.Height + valasz1.Height, 0, 0);
            falka.Margin = new Thickness(képernyőszél / 40 - képernyőszél / 150 + képernyőszél / 10 + emberke.Width + A_vagy_Az.Width + xy_ellen.Width + jelenlegi_összeg.Width + nyereményfa.Width + segitseg.Width, képernyőmag / 40 * 7 + kérdés.Height + A_100_spártai.Height + valasz1.Height, 0, 0);
            penz.Margin = new Thickness(képernyőszél / 40 - képernyőszél / 150 + képernyőszél / 10 + emberke.Width + A_vagy_Az.Width + xy_ellen.Width + jelenlegi_összeg.Width + nyereményfa.Width + segitseg.Width, képernyőmag / 40 * 6 - penz.Height + kérdés.Height + A_100_spártai.Height + valasz1.Height, 0, 0);
        }
        public void Labelök()
        {
            labelök.Add(label1);
            labelök.Add(label2);
            labelök.Add(label3);
            labelök.Add(label4);
            labelök.Add(label5);
            labelök.Add(label6);
            labelök.Add(label7);
            labelök.Add(label8);
            labelök.Add(label9);
            labelök.Add(label10);
            labelök.Add(label11);
            labelök.Add(label12);
            labelök.Add(label13);
            labelök.Add(label14);
            labelök.Add(label15);
            labelök.Add(label16);
            labelök.Add(label17);
            labelök.Add(label18);
            labelök.Add(label19);
            labelök.Add(label20);
            labelök.Add(label21);
            labelök.Add(label22);
            labelök.Add(label23);
            labelök.Add(label24);
            labelök.Add(label25);
            labelök.Add(label26);
            labelök.Add(label27);
            labelök.Add(label28);
            labelök.Add(label29);
            labelök.Add(label30);
            labelök.Add(label31);
            labelök.Add(label32);
            labelök.Add(label33);
            labelök.Add(label34);
            labelök.Add(label35);
            labelök.Add(label36);
            labelök.Add(label37);
            labelök.Add(label38);
            labelök.Add(label39);
            labelök.Add(label40);
            labelök.Add(label41);
            labelök.Add(label42);
            labelök.Add(label43);
            labelök.Add(label44);
            labelök.Add(label45);
            labelök.Add(label46);
            labelök.Add(label47);
            labelök.Add(label48);
            labelök.Add(label49);
            labelök.Add(label50);
            labelök.Add(label51);
            labelök.Add(label52);
            labelök.Add(label53);
            labelök.Add(label54);
            labelök.Add(label55);
            labelök.Add(label56);
            labelök.Add(label57);
            labelök.Add(label58);
            labelök.Add(label59);
            labelök.Add(label60);
            labelök.Add(label61);
            labelök.Add(label62);
            labelök.Add(label63);
            labelök.Add(label64);
            labelök.Add(label65);
            labelök.Add(label66);
            labelök.Add(label67);
            labelök.Add(label68);
            labelök.Add(label69);
            labelök.Add(label70);
            labelök.Add(label71);
            labelök.Add(label72);
            labelök.Add(label73);
            labelök.Add(label74);
            labelök.Add(label75);
            labelök.Add(label76);
            labelök.Add(label77);
            labelök.Add(label78);
            labelök.Add(label79);
            labelök.Add(label80);
            labelök.Add(label81);
            labelök.Add(label82);
            labelök.Add(label83);
            labelök.Add(label84);
            labelök.Add(label85);
            labelök.Add(label86);
            labelök.Add(label87);
            labelök.Add(label88);
            labelök.Add(label89);
            labelök.Add(label90);
            labelök.Add(label91);
            labelök.Add(label92);
            labelök.Add(label93);
            labelök.Add(label94);
            labelök.Add(label95);
            labelök.Add(label96);
            labelök.Add(label97);
            labelök.Add(label98);
            labelök.Add(label99);
            labelök.Add(label100);
            //foreach (var item in labelök)
            //{
            //    File.AppendAllText("labellomásolat.txt",  item.Name + "\n");
            //}
            if (forduló == 1)
            {
                List<string> másolandó = new List<string>();
                foreach (var item in File.ReadAllLines("labellomásolat.txt"))
                {
                    másolandó.Add(item);
                }
                File.WriteAllLines("labello.txt", másolandó);
                foreach (var item in labelök)
                {
                    labelök_nevei.Add(item.Name);
                }
            }
            else
            {
                foreach (var item in File.ReadAllLines("labello.txt"))
                {
                    labelök_nevei.Add(item);
                }
                foreach (var item in labelök)
                {
                    if (labelök_nevei.Contains(item.Name))
                    {
                        labelökjó.Add(item);
                    }
                    else
                    {
                        item.Background = Brushes.Black;
                    }
                }
                labelök = labelökjó;
                List<string> beírnólabel = new List<string>();
                foreach (var item in labelök)
                {
                    beírnólabel.Add(item.Name);
                }
                File.WriteAllLines("labello.txt", beírnólabel);
            }
        }
        public List<int> Válaszhatár(int forduló)
        {
            List<int> visszaad = new List<int>();
            if (forduló == 1)
            {
                visszaad.Add(0);
                visszaad.Add(3);
            }
            if (forduló == 2)
            {
                visszaad.Add(3);
                visszaad.Add(6);

            }
            if (forduló == 3)
            {
                visszaad.Add(5);
                visszaad.Add(10);
            }
            if (forduló == 4 || forduló == 5)
            {
                visszaad.Add(8);
                visszaad.Add(13);
            }
            if (forduló == 6 || forduló == 7)
            {
                visszaad.Add(10);
                visszaad.Add(15);
            }
            if (forduló == 8)
            {
                visszaad.Add(14);
                visszaad.Add(17);
            }
            if (forduló > 8)
            {
                forduló = forduló - 9;
                visszaad.Add(0);
                visszaad.Add(falkaszám + 1);
            }
            //1. 0-2
            //2. 3-5
            //3. 5-9
            //4. 8-12
            //5. 8-12
            //6. 10-14
            //7. 10-14
            //---44-68
            //8. 14-16
            //---58-84
            //9 ---0-x
            //10 ---0-x
            //11 ---0-x
            //12 ---0-x
            return visszaad;
        }
        public void Helyeseválasz(Button betűjel, string betű)
        {
            if (segítségethasznál)
            {
                Segítség(betű);
            }
            else
            {
                if (betű.ToUpper() == akt.Helyes)
                {
                    MessageBox.Show("A válasz helyes!");
                    betűjel.Background = Brushes.Green;
                    Válaszfelfedő();
                    if (forduló <= 11)
                    {
                        if (rosszdb == 0)
                        {
                            MessageBox.Show("Mindenki tudta a helyes választ, így nem gazdagodik egyetlen forinttal sem!");
                        }
                        else
                        {
                            MessageBox.Show($"{rosszdb} db játékos rosszul válaszolt, így {Összegkiírás((rosszdb * nyereményekperkérdés[forduló - 1]).ToString())} Ft üti a markát!");
                            összeg += rosszdb * nyereményekperkérdés[forduló - 1];
                        }

                    }
                    else
                    {
                        if (rosszdb == 0)
                        {
                            MessageBox.Show("Mindenki tudta a helyes választ, így nem gazdagodik egyetlen forinttal sem!");
                        }
                        else
                        {
                            MessageBox.Show($"{rosszdb} db játékos rosszul válaszolt, így {Összegkiírás((rosszdb * 500000).ToString())} Ft üti a markát!");
                            összeg += rosszdb * 500000;
                        }
                    }
                    jelenlegi_összeg.Content = Összegkiírás(összeg.ToString()).ToString() + " Ft";
                    jelenlegi_összeg.HorizontalContentAlignment = HorizontalAlignment.Center;
                    jelenlegi_összeg.VerticalContentAlignment = VerticalAlignment.Center;
                    falkaszám -= rosszdb;
                    xy_ellen.Content = falkaszám + " \n ELLEN";
                    penz.Content = Összegkiírás(összeg.ToString()) + " Ft";
                    penz.Visibility = Visibility.Visible;
                    falka.Visibility = Visibility.Visible;
                    foreach (var item in rossz_labelök)
                    {
                        if (labelök.Contains(item))
                        {
                            labelök.Remove(item);
                        }
                    }
                    foreach (var item in rossz_labelök)
                    {
                        if (labelök_nevei.Contains(item.Name))
                        {
                            labelök_nevei.Remove(item.Name);
                        }
                    }
                    if (falkaszám == 0)
                    {
                        MessageBox.Show("Nem maradt senki a falkában, mind a 100 embert legyőzted!");
                        MessageBox.Show("A játéknak vége, a nyereményed: 50.000.000 Ft! Gratulálunk!");
                        File.WriteAllText("változók.txt", "forduló;1");
                        File.AppendAllText("változók.txt", "\nfordulókérdésszám;0");
                        File.AppendAllText("változók.txt", "\nösszeg;0");
                        File.AppendAllText("változók.txt", "\nfalkaszám;100");
                        File.AppendAllText("változók.txt", "\nszám;0");
                        File.AppendAllText("változók.txt", "\naktkérdésszám;0");
                        File.AppendAllText("változók.txt", "\nrosszdb;0");
                        File.AppendAllText("változók.txt", "\nmenekülőútszám;0");
                        Application.Current.Shutdown();
                    }
                    //rosszdb = labelök.Count;
                    //MessageBox.Show($"A rossz válaszok száma: {rosszdb}");
                }
                else
                {
                    MessageBox.Show("A válasz helytelen!");
                    betűjel.Background = Brushes.Red;
                    if ("A" == akt.Helyes)
                    {
                        valasz1.Background = Brushes.Green;
                    }
                    else
                    {
                        if ("B" == akt.Helyes)
                        {
                            valasz2.Background = Brushes.Green;
                        }
                        else
                        {
                            valasz3.Background = Brushes.Green;
                        }
                    }
                    MessageBox.Show($"A helyes válasz: {akt.Helyes}");
                    MessageBox.Show("A játéknak vége! A nyereményed 0 Ft");
                    File.WriteAllText("változók.txt", "forduló;1");
                    File.AppendAllText("változók.txt", "\nfordulókérdésszám;0");
                    File.AppendAllText("változók.txt", "\nösszeg;0");
                    File.AppendAllText("változók.txt", "\nfalkaszám;100");
                    File.AppendAllText("változók.txt", "\nszám;0");
                    File.AppendAllText("változók.txt", "\naktkérdésszám;0");
                    File.AppendAllText("változók.txt", "\nrosszdb;0");
                    File.AppendAllText("változók.txt", "\nmenekülőútszám;0");
                    Application.Current.Shutdown();
                }
            }
        }

        public void Segítség(string megnyomott)
        {
            menekülőútszám++;
            int segítség1 = 0;
            if (menekülőútszám == 1)
            {
                for (int i = 0; i < sorsoltválaszok.Count; i++)
                {
                    if (sorsoltválaszok[i] == megnyomott)
                    {
                        segítség1++;
                    }
                }
                MessageBox.Show($"{segítség1} db versenyző nyomta meg ezt a választ!");
            }
            if (menekülőútszám == 2)
            {
                int random3;
                bool nemahelyes = true;
                do
                {
                    random3 = r.Next(0, 3);
                    if (válaszok[random3] != akt.Helyes)
                    {
                        nemahelyes = false;
                    }
                } while (nemahelyes);
                if (random3 == 0)
                {
                    valasz1.Content = "";
                }
                if (random3 == 1)
                {
                    valasz2.Content = "";


                }
                if (random3 == 2)
                {
                    valasz3.Content = "";


                }
            }
            if (menekülőútszám == 3)
            {
                összeg = összeg / 4;
                MessageBox.Show($"A játéknak vége, a nyereményed: {Összegkiírás(összeg.ToString())} Ft. Gratulálunk!");
                File.WriteAllText("változók.txt", "forduló;1");
                File.AppendAllText("változók.txt", "\nfordulókérdésszám;0");
                File.AppendAllText("változók.txt", "\nösszeg;0");
                File.AppendAllText("változók.txt", "\nfalkaszám;100");
                File.AppendAllText("változók.txt", "\nszám;0");
                File.AppendAllText("változók.txt", "\naktkérdésszám;0");
                File.AppendAllText("változók.txt", "\nrosszdb;0");
                File.AppendAllText("változók.txt", "\nmenekülőútszám;0");
                Application.Current.Shutdown();
            }
            segítségethasznál = false;
        }

        public void Válaszfelfedő()
        {
            for (int i = 0; i < labelök.Count; i++)
            {
                labelök[i].Content = sorsoltválaszok[i];
            }
            for (int i = 0; i < sorsoltválaszok.Count; i++)
            {
                if (sorsoltválaszok[i] != akt.Helyes)
                {
                    rosszdb++;
                    rossz_labelök.Add(labelök[i]);
                    labelök[i].Background = Brushes.Red;
                }
            }
        }
        public void Válaszsorsoló()
        {
            hányrosszkéthatár = Válaszhatár(forduló);
            összrosszválasz = r.Next(hányrosszkéthatár[0], hányrosszkéthatár[1]);
            foreach (var label in labelök)
            {
                sorsoltválaszok.Add(akt.Helyes);
                //1. 0-2
                //2. 3-5
                //3. 5-9
                //4. 8-12
                //5. 8-12
                //6. 10-14
                //7. 10-14
                //---44-68
                //8. 14-16
                //---58-84
                //9 ---0-x
                //10 ---2-x
                //11 ---4-x
                //12 ---6-x
            }
            for (int i = 0; i < összrosszválasz; i++)
            {
                bool nincsbenne = true;
                while (nincsbenne)
                {
                    int randomszám = r.Next(0, falkaszám);
                    bool bennevan2 = true;
                    if (!rosszrandomszámok.Contains(randomszám))
                    {
                        while (bennevan2)
                        {
                            int random2 = r.Next(0, 3);
                            if (válaszok[random2] != akt.Helyes)
                            {
                                bennevan2 = false;
                                sorsoltválaszok[randomszám] = válaszok[random2];
                                nincsbenne = false;
                            }
                        }
                    }
                }
            }
        }
        public void Láthatóság()
        {
            A_100_spártai.Visibility = Visibility.Visible;
            tvkettőlogo.Visibility = Visibility.Visible;
            egyaszázellenlogo.Visibility = Visibility.Visible;
            kérdés.Visibility = Visibility.Visible;
            valasz1.Visibility = Visibility.Visible;
            valasz2.Visibility = Visibility.Visible;
            valasz3.Visibility = Visibility.Visible;
            A.Visibility = Visibility.Visible;
            B.Visibility = Visibility.Visible;
            C.Visibility = Visibility.Visible;
            emberke.Visibility = Visibility.Visible;
            A_vagy_Az.Visibility = Visibility.Visible;
            xy_ellen.Visibility = Visibility.Visible;
            jelenlegi_összeg.Visibility = Visibility.Visible;
            nyereményfa.Visibility = Visibility.Visible;
            segitseg.Visibility = Visibility.Visible;
            penz.Visibility = Visibility.Hidden;
            falka.Visibility = Visibility.Hidden;
            if (falkaszám == 1)
            {
                nyereményfa.Visibility = Visibility.Hidden;
                ötvenmilka.Visibility = Visibility.Visible;
                ötvenmilka.Margin = nyereményfa.Margin;
                tét.Visibility = Visibility.Visible;
                tét.Margin = new Thickness(620, 483, 0, 0);
            }
            else
            {
                nyereményfa.Visibility = Visibility.Visible;
                ötvenmilka.Visibility = Visibility.Hidden;
                tét.Visibility = Visibility.Hidden;
            }
        }
        public void Újkör()
        {
            MessageBox.Show($"A {forduló + 1}. kör következik!");
            forduló++;
            File.WriteAllText("változók.txt", "forduló;" + forduló);
            File.AppendAllText("változók.txt", "\nfordulókérdésszám;0");
            File.AppendAllText("változók.txt", "\nösszeg;" + összeg);
            File.AppendAllText("változók.txt", "\nfalkaszám;" + falkaszám);
            File.AppendAllText("változók.txt", "\nszám;0");
            File.AppendAllText("változók.txt", "\naktkérdésszám;" + aktkérdésszám);
            File.AppendAllText("változók.txt", "\nrosszdb;0");
            File.AppendAllText("változók.txt", "\nmenekülőútszám;" + menekülőútszám);

            File.WriteAllLines("labello.txt", labelök_nevei);

            Process.Start(Process.GetCurrentProcess().MainModule.FileName);
            Application.Current.Shutdown();


        }
        public MainWindow()
        {
            InitializeComponent();
            Elhelyezés();
            Változók();
            Kérdésbeolvasás();
            Kérdésfeltétel();
            Szövegek();
            Labelök();
            Láthatóság();
            Válaszsorsoló();
        }
        private void grid_Loaded(object sender, RoutedEventArgs e)
        {
            WindowStyle = WindowStyle.SingleBorderWindow;
            ResizeMode = ResizeMode.CanMinimize;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }
        private void valasz1_Click_1(object sender, RoutedEventArgs e)
        {
            Helyeseválasz(valasz1, "A");
            valasz1megnyomva = true;
            if (valasz1megnyomva == true && segitsegmegnyomva == false)
            {
                valasz1.IsEnabled = false;
                valasz2.IsEnabled = false;
                valasz3.IsEnabled = false;
                segitseg.IsEnabled = false;
                falka.IsEnabled = true;
                penz.IsEnabled = true;
            } else if(valasz1megnyomva == true && segitsegmegnyomva == true)
            {
                valasz1.IsEnabled = true;
                valasz2.IsEnabled = true;
                valasz3.IsEnabled = true;
                segitseg.IsEnabled = true;
                falka.IsEnabled = true;
                penz.IsEnabled = true;
            }

        }

        private void valasz2_Click_1(object sender, RoutedEventArgs e)
        {
            Helyeseválasz(valasz2, "B");
            valasz2megnyomva = true;
            if (valasz2megnyomva == true && segitsegmegnyomva == false)
            {
                valasz1.IsEnabled = false;
                valasz2.IsEnabled = false;
                valasz3.IsEnabled = false;
                falka.IsEnabled = true;
                penz.IsEnabled = true;
                segitseg.IsEnabled = false;
            }
            else if (valasz2megnyomva == true && segitsegmegnyomva == true)
            {
                valasz1.IsEnabled = true;
                valasz2.IsEnabled = true;
                valasz3.IsEnabled = true;
                segitseg.IsEnabled = true;
                falka.IsEnabled = true;
                penz.IsEnabled = true;
            }
        }

        private void valasz3_Click_1(object sender, RoutedEventArgs e)
        {
            Helyeseválasz(valasz3, "C");
            valasz3megnyomva = true;
            if (valasz3megnyomva == true && segitsegmegnyomva == false)
            {
                valasz1.IsEnabled = false;
                valasz2.IsEnabled = false;
                valasz3.IsEnabled = false;
                falka.IsEnabled = true;
                penz.IsEnabled = true;
                segitseg.IsEnabled = false;

            }
            else if (valasz3megnyomva == true && segitsegmegnyomva == true)
            {
                valasz1.IsEnabled = true;
                valasz2.IsEnabled = true;
                valasz3.IsEnabled = true;
                segitseg.IsEnabled = true;
                falka.IsEnabled = true;
                penz.IsEnabled = true;
            }
        }

        private void penz_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"A nyereményed: {Összegkiírás(összeg.ToString())} Ft! Gratulálunk!");
            File.WriteAllText("változók.txt", "forduló;1");
            File.AppendAllText("változók.txt", "\nfordulókérdésszám;0");
            File.AppendAllText("változók.txt", "\nösszeg;0");
            File.AppendAllText("változók.txt", "\nfalkaszám;100");
            File.AppendAllText("változók.txt", "\nszám;0");
            File.AppendAllText("változók.txt", "\naktkérdésszám;0");
            File.AppendAllText("változók.txt", "\nrosszdb;0");
            File.AppendAllText("változók.txt", "\nmenekülőútszám;0");
            Application.Current.Shutdown();
        }

        private void falka_Click_1(object sender, RoutedEventArgs e)
        {
            Újkör();
        }

        private void segitseg_Click_1(object sender, RoutedEventArgs e)
        {
            segítségethasznál = true;
            if (menekülőútszám > 0)
            {
                Segítség("rgeuioz");
            }
            if (menekülőútszám == 0)
            {
                segitseg.Background = Brushes.Gold;
            }
            if (menekülőútszám == 2)
            {
                segitseg.Background = Brushes.Red;
                segitseg.Content = összeg / 4 + " Ft";
                segitseg.Foreground = Brushes.White;
            }
            segitsegmegnyomva = true;
            if (segitsegmegnyomva == true)
            {
                valasz1.IsEnabled = true;
                valasz2.IsEnabled = true;
                valasz3.IsEnabled = true;
                segitseg.IsEnabled = true;
                falka.IsEnabled = true;
                penz.IsEnabled = true;
            }
        }
    }
}