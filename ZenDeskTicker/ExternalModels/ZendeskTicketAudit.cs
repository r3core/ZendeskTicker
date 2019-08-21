using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZenDeskTicker.ExternalModels
{
    public class ZendeskTicketAudit
    {
        public List<Audit> audits { get; set; }
        public object next_page { get; set; }
        public object previous_page { get; set; }
        public long count { get; set; }
    }

    public class System
    {
        public string client { get; set; }
        public string ip_address { get; set; }
        public string location { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public string message_id { get; set; }
        public string raw_email_identifier { get; set; }
        public string json_email_identifier { get; set; }
    }

    public class Custom
    {
    }

    public class Metadata
    {
        public System system { get; set; }
        public Custom custom { get; set; }
        public List<object> notifications_suppressed_for { get; set; }
        public object suspension_type_id { get; set; }
    }

    public class AuditFrom
    {
        public bool deleted { get; set; }
        public string title { get; set; }
        public long id { get; set; }
        public long revision_id { get; set; }
    }

    public class AuditSource
    {
        public AuditFrom from { get; set; }
        public string rel { get; set; }
    }

    public class AuditVia
    {
        public string channel { get; set; }
        public AuditSource source { get; set; }
        public string current_sla_policy { get; set; }
    }

    public class Event
    {
        public object id { get; set; }
        public string type { get; set; }
        public object author_id { get; set; }
        public string body { get; set; }
        public string html_body { get; set; }
        public string plain_body { get; set; }
        public bool @public { get; set; }
        public List<object> attachments { get; set; }
        public object audit_id { get; set; }
        public object value { get; set; }
        public string field_name { get; set; }
        public AuditVia via { get; set; }
        public string subject { get; set; }
        public List<object> recipients { get; set; }
        public object previous_value { get; set; }
    }

    public class From2
    {
        public string address { get; set; }
        public string name { get; set; }
        public List<string> original_recipients { get; set; }
    }

    public class AuditTo
    {
        public string name { get; set; }
        public object address { get; set; }
    }

    public class Source2
    {
        public From2 from { get; set; }
        public AuditTo to { get; set; }
        public object rel { get; set; }
    }

    public class Via2
    {
        public string channel { get; set; }
        public Source2 source { get; set; }
    }

    public class Audit
    {
        public object id { get; set; }
        public long ticket_id { get; set; }
        public DateTime created_at { get; set; }
        public object author_id { get; set; }
        public Metadata metadata { get; set; }
        public List<Event> events { get; set; }
        public Via2 via { get; set; }
    }
}
