using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZenDeskTicker.ExternalModels
{
    public class ZendeskSearch
    {
        public List<ZendeskSearchResult> results { get; set; }
        public object facets { get; set; }
        public string next_page { get; set; }
        public object previous_page { get; set; }
        public long count { get; set; }
    }

    public class ZendeskSearchResult
    {
        public string url { get; set; }
        public long id { get; set; }
        public object external_id { get; set; }
        public Via via { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public string type { get; set; }
        public string subject { get; set; }
        public string raw_subject { get; set; }
        public string description { get; set; }
        public string priority { get; set; }
        public string status { get; set; }
        public string recipient { get; set; }
        public object requester_id { get; set; }
        public object submitter_id { get; set; }
        public object assignee_id { get; set; }
        public object organization_id { get; set; }
        public long group_id { get; set; }
        public List<object> collaborator_ids { get; set; }
        public List<object> follower_ids { get; set; }
        public List<object> email_cc_ids { get; set; }
        public object forum_topic_id { get; set; }
        public object problem_id { get; set; }
        public bool has_incidents { get; set; }
        public bool is_public { get; set; }
        public object due_at { get; set; }
        public List<string> tags { get; set; }
        public List<CustomField> custom_fields { get; set; }
        public SatisfactionRating satisfaction_rating { get; set; }
        public List<object> sharing_agreement_ids { get; set; }
        public List<Field> fields { get; set; }
        public List<object> followup_ids { get; set; }
        public long ticket_form_id { get; set; }
        public long brand_id { get; set; }
        public object satisfaction_probability { get; set; }
        public bool allow_channelback { get; set; }
        public bool allow_attachments { get; set; }
        public string result_type { get; set; }
    }

    public class From
    {
        public string address { get; set; }
        public string name { get; set; }
        public long? ticket_id { get; set; }
        public string subject { get; set; }
    }

    public class To
    {
        public string name { get; set; }
        public string address { get; set; }
    }

    public class Source
    {
        public From from { get; set; }
        public To to { get; set; }
        public string rel { get; set; }
    }

    public class Via
    {
        public string channel { get; set; }
        public Source source { get; set; }
    }

    public class CustomField
    {
        public long id { get; set; }
        public object value { get; set; }
    }

    public class SatisfactionRating
    {
        public string score { get; set; }
        public long? id { get; set; }
        public object comment { get; set; }
        public string reason { get; set; }
        public long? reason_id { get; set; }
    }

    public class Field
    {
        public long id { get; set; }
        public object value { get; set; }
    }
}
