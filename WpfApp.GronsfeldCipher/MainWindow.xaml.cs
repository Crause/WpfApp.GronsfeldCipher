using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace WpfApp.GronsfeldCipher
{
  public partial class MainWindow : Window
  {
    string Key;
    int TextCount;
    const string RussianAlphabet = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ"; 
    const string EnglishAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; 
    List<string> InputText = new List<string>();
    List<string> OutputText = new List<string>();
    public MainWindow()
    {
      InitializeComponent();
    }
    // Browse button click event to open file
    private void buttonBrowse_Click(object sender, RoutedEventArgs e)
    {
      OpenFileDialog dialog = new OpenFileDialog()
      {
        CheckFileExists = false,
        CheckPathExists = true,
        Multiselect = false,
        Title = "Choose file",
        Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*"
      };

      if (dialog.ShowDialog() == true)
      {
        tbInput.Text = File.ReadAllText(dialog.FileName, Encoding.UTF8);
      }
    }
    private void Prelaunch()
    {
      InputText.Clear();
      OutputText.Clear();
      Key = tbKey.Text;
      for (int j = 0; j < tbInput.LineCount; j++)
      {
        InputText.Add(tbInput.GetLineText(j).ToUpper());
      }
      TextCount = 0;
      foreach (var symbol in tbInput.Text)
      {
        if (RussianAlphabet.Contains(symbol.ToString().ToUpper()) || EnglishAlphabet.Contains(symbol.ToString().ToUpper()))
        {
          TextCount++;
        }
      }
      int i = 0;
      while (Key.Length < TextCount)
      {
        Key += tbKey.Text[i];
        i++;
        if (i == tbKey.Text.Length) i = 0;
      }
    }
    // Encrypt button click event
    private void buttonEncrypt_Click(object sender, RoutedEventArgs e)
    {
      Prelaunch();

      if (radioEnglish.IsChecked == true)
      {
        Encryption(EnglishAlphabet);
      }
      if (radioRussian.IsChecked == true)
      {
        Encryption(RussianAlphabet);
      }
    }
    // Encrypt function
    private void Encryption(string alphabet)
    {
      string newLine = "";
      int KeyIndex = 0;
      foreach (var line in InputText)
      {
        for (int i = 0; i < line.Length; i++)
        {
          if (alphabet.Contains(line[i].ToString()))
          {
            newLine += alphabet[(alphabet.IndexOf(line[i]) + (int)Key[KeyIndex] - 48) % alphabet.Length]; // key in ascii
            KeyIndex++;
          }
          else
          {
            newLine += line[i];
          }
        }
        OutputText.Add(newLine);
        newLine = "";
      }
      tbOutput.Clear();
      for (int i = 0; i < OutputText.Count; i++)
      {
        tbOutput.Text += OutputText[i];
      }
    }
    // Decrypt button click event
    private void buttonDecrypt_Click(object sender, RoutedEventArgs e)
    {
      Prelaunch();

      if (radioEnglish.IsChecked == true)
      {
        Decryption(EnglishAlphabet);
      }
      if (radioRussian.IsChecked == true)
      {
        Decryption(RussianAlphabet);
      }
    }
    // Decrypt function
    private void Decryption(string alphabet)
    {
      string newLine = "";
      int KeyIndex = 0;
      foreach (var line in InputText)
      {
        for (int i = 0; i < line.Length; i++)
        {
          if (alphabet.Contains(line[i].ToString()))
          {
            newLine += alphabet[(alphabet.IndexOf(line[i]) + alphabet.Length - ((int)Key[KeyIndex] - 48)) % alphabet.Length]; // key in ascii
            KeyIndex++;
          }
          else
          {
            newLine += line[i];
          }
        }
        OutputText.Add(newLine);
        newLine = "";
      }
      tbOutput.Clear();
      for (int i = 0; i < OutputText.Count; i++)
      {
        tbOutput.Text += OutputText[i];
      }
    }
    // Browse button click event to save file
    private void buttonSave_Click(object sender, RoutedEventArgs e)
    {
      SaveFileDialog dialog = new SaveFileDialog()
      {
        Title = "Save file",
        Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*"
      };
      if (dialog.ShowDialog() == true)
      {
        File.WriteAllText(dialog.FileName, tbOutput.Text, Encoding.UTF8);
      }
    }
    // Only numeric input in key textbox
    private void tbKey_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
      if (!Char.IsDigit(e.Text, 0))
      {
        e.Handled = true;
      }
    }
  }
}
