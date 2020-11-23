#if PHOTON_BOLT
using UnityEngine;
using System.Collections;

namespace ICode{
	public class BoltEventData {
		private BoltConnection mConnection;
		public BoltConnection connection{
			get{
				return mConnection;
			}
		}
		private UdpKit.UdpEndPoint mEndpoint;
		public UdpKit.UdpEndPoint endpoint{
			get{
				return mEndpoint;
			}
		}
		private Bolt.IProtocolToken mToken;
		private Bolt.IProtocolToken token{
			get{
				return mToken;
			}
		}
		private BoltEntity mEntity;
		public BoltEntity entity{
			get{
				return mEntity;
			}
		}
		private string mMap;
		public string map{
			get{
				return mMap;
			}
		}

		public BoltEventData(){
		}

		public BoltEventData(string map){
			this.mMap = map;
		}

		public BoltEventData(BoltConnection connection){
			this.mConnection = connection;
		}

		public BoltEventData(BoltEntity entity){
			this.mEntity = entity;
		}

		public BoltEventData(UdpKit.UdpEndPoint endpoint, Bolt.IProtocolToken token){
			this.mEndpoint = endpoint;
			this.mToken = token;
		}

		public BoltEventData(BoltConnection connection, UdpKit.UdpEndPoint endpoint, Bolt.IProtocolToken token,BoltEntity entity, string map){
			this.mConnection = connection;
			this.mEndpoint = endpoint;
			this.mToken = token;
			this.mEntity = entity;
			this.mMap = map;
		}
	}
}
#endif