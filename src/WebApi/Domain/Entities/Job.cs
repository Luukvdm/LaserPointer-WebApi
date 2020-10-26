using System.Collections.Generic;
using LaserPointer.WebApi.Domain.Common;
using LaserPointer.WebApi.Domain.Enums;
using LaserPointer.WebApi.Domain.Events;

namespace LaserPointer.WebApi.Domain.Entities {
	public class Job : AuditableEntity, IHasDomainEvent {
        public Job()
        {
            HashesToCrack = new List<Hash>();
            DomainEvents = new List<DomainEvent>();
        }        
        
		public int Id { get; set; }
		public HashType HashType { get; set; }
        public IList<Hash> HashesToCrack { get; private set; }
		private JobStatus _status = JobStatus.InQueue;
		public JobStatus Status {
			get => _status;
			set {
                if(this.Status != _status) DomainEvents.Add(new JobStatusChangedEvent(this, value, _status));
				_status = value;
			} 
		}
		
		public List<DomainEvent> DomainEvents { get; set; }
	}
}
