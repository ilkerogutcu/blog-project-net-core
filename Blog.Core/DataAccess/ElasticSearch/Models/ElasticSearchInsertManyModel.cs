namespace Blog.Core.DataAccess.ElasticSearch.Models
{
    public class ElasticSearchInsertManyModel : ElasticSearchModel
    {

        public object[] Items { get; set; }
    }
}
