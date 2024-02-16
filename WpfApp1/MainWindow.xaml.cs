using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;

namespace WpfApp1
{
   
    public class Note
    {
        public string Title { get; set; } 
        public string Description { get; set; } 
        public DateTime Date { get; set; } 
    }

    
    public static class JsonHelper
    {
       
        public static void Serialize<T>(T data, string filePath)
        {
            string json = JsonConvert.SerializeObject(data);
            File.WriteAllText(filePath, json);
        }

        public static T Deserialize<T>(string filePath)
        {
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<T>(json);
        }
    }

    public partial class MainWindow : Window
    {
        private List<Note> notes; 
        private string filePath = "notes.json"; 

        public MainWindow()
        {
            InitializeComponent();
            LoadNotes(); 
            DatePicker.SelectedDate = DateTime.Today; 
            DisplayNotes(); 
        }

        private void LoadNotes()
        {
            if (File.Exists(filePath))
            {
                notes = JsonHelper.Deserialize<List<Note>>(filePath); 
            }
            else
            {
                notes = new List<Note>(); 
            }
        }

        private void SaveNotes()
        {
            JsonHelper.Serialize(notes, filePath); 
        }

        private void DisplayNotes()
        {
            NotesListBox.ItemsSource = notes.FindAll(note => note.Date.Date == DatePicker.SelectedDate.Value.Date);
        }

        private void DatePicker_SelectedDateChanged(object sender, RoutedEventArgs e)
        {
            DisplayNotes(); 
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Note newNote = new Note
            {
                Title = "Новая заметка", 
                Description = "Описание", 
                Date = DatePicker.SelectedDate.Value 
            };
            notes.Add(newNote); 
            SaveNotes(); 
            DisplayNotes(); 
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (NotesListBox.SelectedItem != null)
            {
                Note selectedNote = (Note)NotesListBox.SelectedItem;
                // я в душе не чаю, как это сделать, идей 0   
		//И хотел бы тут задать вопрос не по теме(потом могу забыть), на чём лучше всего писать desktop-ное приложение сразу и под Windows, и под Mac OS
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (NotesListBox.SelectedItem != null)
            {
                Note selectedNote = (Note)NotesListBox.SelectedItem; 
                notes.Remove(selectedNote); 
                SaveNotes(); 
                DisplayNotes(); 
            }
        }
    }
}