using UnityEngine;
using System.Collections.Generic;

namespace Summoner.Util.Event {
	public class SubscriberOfAttribute : System.Attribute {
		public readonly System.Type containerType;
		public SubscriberOfAttribute( System.Type containerType ) {
			this.containerType = containerType;
		}
	}
}