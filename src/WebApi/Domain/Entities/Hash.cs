namespace LaserPointer.WebApi.Domain.Entities {
	public class Hash {
		public int Id { get; set; }
        public Job Job { get; set; }
		public byte[] Value { get; set; }
        public string PlainValue { get; set; }
	}
}
