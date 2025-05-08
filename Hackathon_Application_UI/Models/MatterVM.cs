namespace Hackathon_Application_UI.Models
{
    public class MatterVM
    {
        public MatterVM() 
        {
            MatterfList = new List<Matter>();
            Matter = new Matter();
            RejectedMatters = new List<Matter>();
            CompletedMatters = new List<Matter>();
            DocumentList = new List<Document>();
        }
        public Matter Matter { get; set; }
        public List<Matter> MatterfList { get; set; }
        public List<Matter> RejectedMatters { get; set; }
        public List<Matter> CompletedMatters { get; set; }
        public List<Document> DocumentList { get; set; }

    }
}
