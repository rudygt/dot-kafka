namespace DotKafka.Prototype.Common.Config.Types {
    public class Password {
        public static string Hidden = "[hidden]";
        private readonly string _value;

        public Password(string value) {
            _value = value;
        }

        public int HashCode() {
            return GetHashCode();
        }

        public override int GetHashCode() {
            return _value.GetHashCode();
        }

        public override bool Equals(object obj) {
            if (!(obj is Password))
                return false;
            var other = (Password) obj;
            return _value.Equals(other._value);
        }

        public override string ToString() {
            return Hidden;
        }

        public string Value() {
            return _value;
        }
    }
}
