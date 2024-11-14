using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;

namespace IOLab.Core.Network.Data
{
    public class WorkshopContent : INotifyPropertyChanged
    {
        private string _authorName;
        private string _description;
        private string _title;
        private string _jsonFileID;
        private string _createAt;
        private string _tags;
        private JObject _myContent;

        public event PropertyChangedEventHandler PropertyChanged;

        public string AuthorName
        {
            get => _authorName;
            set { _authorName = value; OnPropertyChanged(nameof(AuthorName)); }
        }

        public string Description
        {
            get => _description;
            set { _description = value; OnPropertyChanged(nameof(Description)); }
        }

        public string Title
        {
            get => _title;
            set { _title = value; OnPropertyChanged(nameof(Title)); }
        }

        public string JsonFileID
        {
            get => _jsonFileID;
            set { _jsonFileID = value; OnPropertyChanged(nameof(JsonFileID)); }
        }

        public string CreateAt
        {
            get => _createAt;
            set { _createAt = value; OnPropertyChanged(nameof(CreateAt)); }
        }

        public string Tags
        {
            get => _tags;
            set { _createAt = value; OnPropertyChanged(nameof(Tags)); }
        }

        public JObject MyContent
        {
            get => _myContent;
            set { _myContent = value; OnPropertyChanged(nameof(MyContent)); }
        }

        public WorkshopContent() { }

        public WorkshopContent(JObject content)
        {
            Parse(content);
        }

        public void Parse(JObject content)
        {
            Title = content["title"]?.ToString();
            AuthorName = content["author"]?.ToString();
            Description = content["content"]?.ToString();
            CreateAt = content["created_at"]?.ToString();
            Tags = "智慧学习"; // TODO 适配标签
            MyContent = content;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool IsEmpty()
        {
            return MyContent == null;
        }
    }
}
