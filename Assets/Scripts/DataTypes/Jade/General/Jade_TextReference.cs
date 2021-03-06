﻿
using System;
using System.Collections.Generic;
using BinarySerializer;

namespace R1Engine.Jade {
	public class Jade_TextReference : BinarySerializable {
		public Jade_Key Key { get; set; }
		public bool IsNull => Key.IsNull;

		public Dictionary<int, TEXT_AllText> Text { get; set; } = new Dictionary<int, TEXT_AllText>();

		public override void SerializeImpl(SerializerObject s) {
			Key = s.SerializeObject<Jade_Key>(Key, name: nameof(Key));
		}

		public Jade_TextReference() { }
		public Jade_TextReference(Context c, Jade_Key key) {
			Context = c;
			Key = key;
		}
		
		// Dummy resolve method for now
		public Jade_TextReference Resolve() {
			return this;
		}

		public TEXT_AllText GetTextForLanguage(int languageID) {
			return Text.TryGetValue(languageID, out TEXT_AllText value) ? value : null;
		}

		public Jade_TextReference LoadText(int languageID,
			Action<SerializerObject, TEXT_AllText> onPreSerialize = null,
			Action<SerializerObject, TEXT_AllText> onPostSerialize = null) {
			if (IsNull) return this;
			LOA_Loader loader = Context.GetStoredObject<LOA_Loader>(Jade_BaseManager.LoaderKey);
			loader.RequestFile(Key, (s, configureAction) => {
				Text[languageID] = s.SerializeObject<TEXT_AllText>(GetTextForLanguage(languageID), onPreSerialize: f => {
					configureAction(f); onPreSerialize?.Invoke(s, f);
				}, name: nameof(Text));
				onPostSerialize?.Invoke(s, Text[languageID]);
			}, (f) => {
				Text[languageID] = f?.ConvertType<TEXT_AllText>();
			}, immediate: false,
			queue: LOA_Loader.QueueType.Current,
			cache: LOA_Loader.CacheType.Current,
			flags: LOA_Loader.ReferenceFlags.Log | LOA_Loader.ReferenceFlags.DontCache | LOA_Loader.ReferenceFlags.DontUseCachedFile,
			name: typeof(TEXT_AllText).Name);
			return this;
		}

		public override bool IsShortLog => true;
		public override string ShortLog => Key.ToString();
	}
}
