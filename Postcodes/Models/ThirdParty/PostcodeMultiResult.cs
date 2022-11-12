namespace Postcodes.Models.ThirdParty
{
    public class PostcodeMultiResult
    {
        public int status { get; set; }
        public List<PostcodeMultiResultQuery> result { get; set; }
    }

    public class PostcodeMultiResultQuery
    {
        public string query { get; set; } = string.Empty;
        public PostcodeResultItem? result { get; set; }
    }

}
