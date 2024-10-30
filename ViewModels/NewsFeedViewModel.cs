using System.Collections.ObjectModel;
using System.Windows.Input;
using UsenetProgram.Applications;
using UsenetProgram.Models;
using UsenetProgram.Services;
using UsenetProgram.Utilities;

namespace UsenetProgram.ViewModels
{
    public class NewsFeedViewModel : Bindable, IWindowService
    {
        public Action Close { get; set; }
        public ICommand PostCMD { get; set; }


        private ObservableCollection<Group> _groupList;

        public ObservableCollection<Group> GroupList
        {
            get { return _groupList; }
            set
            {
                _groupList = value;
                OnPropertyChanged();
            }
        }

        private Group _selectedGroup;

        public Group SelectedGroup
        {
            get { return _selectedGroup; }
            set
            {
                _selectedGroup = value;
                OnPropertyChanged();
                this.ArticleContent.Clear();
                PopulateHeaderList();
            }
        }

        private ObservableCollection<Header> _headerList;

        public ObservableCollection<Header> HeaderList
        {
            get { return _headerList; }
            set
            {
                _headerList = value;
                OnPropertyChanged();
            }
        }

        private Header _selectedHeader;

        public Header SelectedHeader
        {
            get { return _selectedHeader; }
            set
            {
                _selectedHeader = value;
                OnPropertyChanged();
                PopulateArticleContentList();
            }
        }

        private ObservableCollection<string> _articleContent;

        public ObservableCollection<string> ArticleContent
        {
            get { return _articleContent; }
            set
            {
                _articleContent = value;
                OnPropertyChanged();
            }
        }

        public NewsFeedViewModel()
        {
            this._groupList = new ObservableCollection<Group>();
            this._headerList = new ObservableCollection<Header>();
            this._articleContent = new ObservableCollection<string>();

            this.PostCMD = new RelayCommand(PostArticle, CanPostArticle);

            PopulateGroupList();
        }

        private async void PopulateGroupList()
        {
            this.GroupList.Clear();
            var groups = await GetGroups.ActionAsync();
            foreach (var group in groups)
            {
                GroupList.Add(group); 
            }
        }

        private async void PopulateHeaderList()
        {
            if (_selectedGroup != null)
            {
                this.HeaderList.Clear();
                var headers = await GetHeaders.ActionAsync(_selectedGroup.Name);
                foreach (var header in headers)
                    this.HeaderList.Add(header);
            }
        }

        private async void PopulateArticleContentList()
        {
            if (_selectedHeader != null)
            {
                this.ArticleContent.Clear();
                var articleContent = await GetArticleContent.ActionAsync(_selectedHeader.ArticleNumber);
                foreach (var line in articleContent)
                    this.ArticleContent.Add(line);
            }
        }

        private void PostArticle()
        {}

        private bool CanPostArticle()
        {
            return _selectedGroup?.Postable ?? false;
        }
    }
}
